using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace SB3Utility
{

	public abstract class ppHeader
	{
		public abstract uint HeaderSize(int numFiles);
		public abstract List<IWriteFile> ReadHeader(string path, ppFormat format);
		public abstract void WriteHeader(Stream stream, List<IWriteFile> files, uint[] sizes, object[] metadata);
        public abstract void WriteRLEHeader(Stream stream, List<Tuple<IWriteFile, uint, uint, object>> data);
        public abstract ppHeader TryHeader(string path);
		public abstract ppFormat[] ppFormats { get; }
	}

	public class ppHeader_SMRetail
	{

		public static byte[] DecryptHeaderBytes(byte[] buf)
		{
			byte[] table = new byte[]
			{
				0xFA, 0x49, 0x7B, 0x1C, // var48
				0xF9, 0x4D, 0x83, 0x0A,
				0x3A, 0xE3, 0x87, 0xC2, // var24
				0xBD, 0x1E, 0xA6, 0xFE
			};

			byte var28;
			for (int var4 = 0; var4 < buf.Length; var4++)
			{
				var28 = (byte)(var4 & 0x7);
				table[var28] += table[8 + var28];
				buf[var4] ^= table[var28];
			}

			return buf;
		}
	}

	public class ppHeader_Wakeari : ppHeader
	{
		public override ppFormat[] ppFormats
		{
			get
			{
                return new ppFormat[] {
                    new ppFormat_AA2()
				};
			}
		}

		const byte FirstByte = 0x01;
		const int Version = 0x6C;
		byte[] ppVersionBytes = Encoding.ASCII.GetBytes("[PPVER]\0");

		public override uint HeaderSize(int numFiles)
		{
			return (uint)((288 * numFiles) + 9 + 12);
		}

		public override List<IWriteFile> ReadHeader(string path, ppFormat format)
		{
			List<IWriteFile> subfiles = null;
			using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
			{
				byte[] versionHeader = reader.ReadBytes(8);
				int version = BitConverter.ToInt32(ppHeader_SMRetail.DecryptHeaderBytes(reader.ReadBytes(4)), 0);

				ppHeader_SMRetail.DecryptHeaderBytes(reader.ReadBytes(1));  // first byte
				int numFiles = BitConverter.ToInt32(ppHeader_SMRetail.DecryptHeaderBytes(reader.ReadBytes(4)), 0);
				byte[] buf = ppHeader_SMRetail.DecryptHeaderBytes(reader.ReadBytes(numFiles * 288));

				subfiles = new List<IWriteFile>(numFiles);
				for (int i = 0; i < numFiles; i++)
				{
					int offset = i * 288;
					ppSubfile subfile = new ppSubfile(path);
					subfile.ppFormat = format;
					subfile.Name = Utility.EncodingShiftJIS.GetString(buf, offset, 260).TrimEnd(new char[] { '\0' });
					subfile.size = BitConverter.ToUInt32(buf, offset + 260);
					subfile.offset = BitConverter.ToUInt32(buf, offset + 264);

					Metadata metadata = new Metadata();
					metadata.LastBytes = new byte[20];
					System.Array.Copy(buf, offset + 268, metadata.LastBytes, 0, 20);
					subfile.Metadata = metadata;

					subfiles.Add(subfile);
				}
			}
			return subfiles;
		}

		public override void WriteHeader(Stream stream, List<IWriteFile> files, uint[] sizes, object[] metadata)
		{
			byte[] headerBuf = new byte[HeaderSize(files.Count)];
			BinaryWriter writer = new BinaryWriter(new MemoryStream(headerBuf));

			writer.Write(ppVersionBytes);
			writer.Write(ppHeader_SMRetail.DecryptHeaderBytes(BitConverter.GetBytes(Version)));
			
			writer.Write(ppHeader_SMRetail.DecryptHeaderBytes(new byte[] { FirstByte }));
			writer.Write(ppHeader_SMRetail.DecryptHeaderBytes(BitConverter.GetBytes(files.Count)));

			byte[] fileHeaderBuf = new byte[288 * files.Count];
			uint fileOffset = (uint)headerBuf.Length;
			for (int i = 0; i < files.Count; i++)
			{
				int idx = i * 288;
				Utility.EncodingShiftJIS.GetBytes(files[i].Name).CopyTo(fileHeaderBuf, idx);
				BitConverter.GetBytes(sizes[i]).CopyTo(fileHeaderBuf, idx + 260);
				BitConverter.GetBytes(fileOffset).CopyTo(fileHeaderBuf, idx + 264);

				Metadata wakeariMetadata = (Metadata)metadata[i];
				System.Array.Copy(wakeariMetadata.LastBytes, 0, fileHeaderBuf, idx + 268, 20);
				BitConverter.GetBytes(sizes[i]).CopyTo(fileHeaderBuf, idx + 284);

				fileOffset += sizes[i];
			}

			writer.Write(ppHeader_SMRetail.DecryptHeaderBytes(fileHeaderBuf));
			writer.Write(ppHeader_SMRetail.DecryptHeaderBytes(BitConverter.GetBytes(headerBuf.Length)));
			writer.Flush();
			stream.Write(headerBuf, 0, headerBuf.Length);
		}

        public override void WriteRLEHeader(Stream stream, List<Tuple<IWriteFile, uint, uint, object>> data)
        {
            byte[] headerBuf = new byte[HeaderSize(data.Count)];
            BinaryWriter writer = new BinaryWriter(new MemoryStream(headerBuf));

            writer.Write(ppVersionBytes);
            writer.Write(ppHeader_SMRetail.DecryptHeaderBytes(BitConverter.GetBytes(Version)));

            writer.Write(ppHeader_SMRetail.DecryptHeaderBytes(new byte[] { FirstByte }));
            writer.Write(ppHeader_SMRetail.DecryptHeaderBytes(BitConverter.GetBytes(data.Count)));

            List<uint> offsets = new List<uint>();

            byte[] fileHeaderBuf = new byte[288 * data.Count];
            uint fileOffset = (uint)headerBuf.Length;
            for (int i = 0; i < data.Count; i++)
            {
                IWriteFile subfile = data[i].Item1;
                uint hash = data[i].Item2;
                uint size = data[i].Item3;
                object metadata = data[i].Item4;

                bool collision = data.GetRange(0, i)
                                    .Any(x => x.Item2 == hash);

                uint currentOffset = fileOffset;

                if (collision)
                {
                    int index = data.IndexOf(data.GetRange(0, i)
                                        .First(x => x.Item2 == hash));

                    size = data[index].Item3;

                    currentOffset = offsets[index];
                }

                offsets.Add(currentOffset);

                int idx = i * 288;
                Utility.EncodingShiftJIS.GetBytes(subfile.Name).CopyTo(fileHeaderBuf, idx);
                BitConverter.GetBytes(size).CopyTo(fileHeaderBuf, idx + 260);
                BitConverter.GetBytes(currentOffset).CopyTo(fileHeaderBuf, idx + 264);

                Metadata wakeariMetadata = (Metadata)metadata;
                System.Array.Copy(wakeariMetadata.LastBytes, 0, fileHeaderBuf, idx + 268, 20);
                BitConverter.GetBytes(size).CopyTo(fileHeaderBuf, idx + 284);

                if (!collision)
                    fileOffset += size;
            }

            writer.Write(ppHeader_SMRetail.DecryptHeaderBytes(fileHeaderBuf));
            writer.Write(ppHeader_SMRetail.DecryptHeaderBytes(BitConverter.GetBytes(headerBuf.Length)));
            writer.Flush();
            stream.Write(headerBuf, 0, headerBuf.Length);
        }

		public override ppHeader TryHeader(string path)
		{
			using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
			{
				byte[] version = reader.ReadBytes(8);
				for (int i = 0; i < version.Length; i++)
				{
					if (ppVersionBytes[i] != version[i])
					{
						return null;
					}
				}
				return this;
			}
		}

		public struct Metadata
		{
			public byte[] LastBytes { get; set; }
		}
	}
}
