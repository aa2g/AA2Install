using System;
using System.Collections.Generic;
using System.IO;

namespace SB3Utility
{
	public class ppSwapfile : IReadFile, IWriteFile, IDisposable
	{
		static string tmpFolder = (Environment.GetEnvironmentVariable("TMP") != null ? Environment.GetEnvironmentVariable("TMP") + @"\" : "") + @"SB3Utility(G+S)_swap";

		string swapFilePath;

		public ppSwapfile(string ppPath, IWriteFile source)
		{
			this.Name = source.Name;
			this.swapFilePath = tmpFolder + @"\" + ppPath.Replace('\\', '#').Replace(':', '~') + "#" + source.Name;

			if (!Directory.Exists(tmpFolder))
			{
				Directory.CreateDirectory(tmpFolder);
			}
			else
			{
				string rnd = string.Empty;
				Random rand = new Random();
				while (File.Exists(swapFilePath + rnd))
				{
					rnd = "-" + rand.Next();
				}
				swapFilePath += rnd;
			}
			using (FileStream stream = File.OpenWrite(swapFilePath))
			{
				source.WriteTo(stream);
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				File.Delete(swapFilePath);
			}
		}

		~ppSwapfile()
		{
			try
			{
				File.Delete(swapFilePath);
			}
			catch (Exception ex)
			{
				using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(swapFilePath + "-exc.txt")))
				{
					writer.Seek(0, SeekOrigin.End);
					writer.Write(System.DateTime.Now + " " + ex);
				}
			}
		}

		public string Name { get; set; }

		public Stream CreateReadStream()
		{
			return File.OpenRead(swapFilePath);
		}

		public void WriteTo(Stream stream)
		{
			using (BinaryReader reader = new BinaryReader(CreateReadStream()))
			{
				BinaryWriter writer = new BinaryWriter(stream);
				for (byte[] buffer; (buffer = reader.ReadBytes(Utility.BufSize)).Length > 0; )
				{
					writer.Write(buffer);
				}
			}
		}
	}
}
