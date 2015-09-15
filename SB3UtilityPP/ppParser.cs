using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.ComponentModel;
using SB3Utility;

namespace SB3Utility
{
	public class ppParser
	{
		public string FilePath { get; protected set; }
		public ppFormat Format { get; set; }
		public List<IWriteFile> Subfiles { get; protected set; }

		private string destPath;
		private bool keepBackup;
		private string backupExt;

		public ppParser(string path, ppFormat format)
		{
			this.Format = format;
			this.FilePath = path;
            if (File.Exists(path))
            {
			    this.Subfiles = format.ppHeader.ReadHeader(path, format);
            }
            else
            {
                this.Subfiles = new List<IWriteFile>();
            }
		}

		public BackgroundWorker WriteArchive(string destPath, bool keepBackup, string backupExtension, bool background)
		{
			this.destPath = destPath;
			this.keepBackup = keepBackup;
			this.backupExt = backupExtension;

			BackgroundWorker worker = new BackgroundWorker();
			worker.WorkerSupportsCancellation = true;
			worker.WorkerReportsProgress = true;

			worker.DoWork += new DoWorkEventHandler(writeArchiveWorker_DoWork);

			if (!background)
			{
				writeArchiveWorker_DoWork(worker, new DoWorkEventArgs(null));
			}

			return worker;
		}

		void writeArchiveWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker worker = (BackgroundWorker)sender;
			string backup = null;

			string dirName = Path.GetDirectoryName(destPath);
			if (dirName == String.Empty)
			{
				dirName = @".\";
			}
			DirectoryInfo dir = new DirectoryInfo(dirName);
			if (!dir.Exists)
			{
				dir.Create();
			}

			if (File.Exists(destPath))
			{
				backup = Utility.GetDestFile(dir, Path.GetFileNameWithoutExtension(destPath) + ".bak", backupExt);
				File.Move(destPath, backup);

				if (destPath.Equals(this.FilePath, StringComparison.InvariantCultureIgnoreCase))
				{
					for (int i = 0; i < Subfiles.Count; i++)
					{
						ppSubfile subfile = Subfiles[i] as ppSubfile;
						if ((subfile != null) && subfile.ppPath.Equals(this.FilePath, StringComparison.InvariantCultureIgnoreCase))
						{
							subfile.ppPath = backup;
						}
					}
				}
			}

			try
			{
				using (BinaryWriter writer = new BinaryWriter(File.Create(destPath)))
				{
					writer.BaseStream.Seek(Format.ppHeader.HeaderSize(Subfiles.Count), SeekOrigin.Begin);
					long offset = writer.BaseStream.Position;
					uint[] sizes = new uint[Subfiles.Count];
					object[] metadata = new object[Subfiles.Count];

					for (int i = 0; i < Subfiles.Count; i++)
					{
						if (worker.CancellationPending)
						{
							e.Cancel = true;
							break;
						}

						worker.ReportProgress(i * 100 / Subfiles.Count);

						ppSubfile subfile = Subfiles[i] as ppSubfile;
						if ((subfile != null) && (subfile.ppFormat == this.Format))
						{
							using (BinaryReader reader = new BinaryReader(File.OpenRead(subfile.ppPath)))
							{
								reader.BaseStream.Seek(subfile.offset, SeekOrigin.Begin);

								uint readSteps = subfile.size / Utility.BufSize;
								for (int j = 0; j < readSteps; j++)
								{
									writer.Write(reader.ReadBytes(Utility.BufSize));
								}
								writer.Write(reader.ReadBytes(subfile.size % Utility.BufSize));
							}
							metadata[i] = subfile.Metadata;
						}
						else
						{
							Stream stream = Format.WriteStream(writer.BaseStream);
							Subfiles[i].WriteTo(stream);
							metadata[i] = Format.FinishWriteTo(stream);
						}

						long pos = writer.BaseStream.Position;
						sizes[i] = (uint)(pos - offset);
						offset = pos;
					}

					if (!e.Cancel)
					{
						writer.BaseStream.Seek(0, SeekOrigin.Begin);
						Format.ppHeader.WriteHeader(writer.BaseStream, Subfiles, sizes, metadata);
						offset = writer.BaseStream.Position;
						for (int i = 0; i < Subfiles.Count; i++)
						{
							ppSubfile subfile = Subfiles[i] as ppSubfile;
							if (subfile != null)
							{
								subfile.offset = offset;
								subfile.size = sizes[i];
							}
							offset += sizes[i];
						}
					}
				}

				if (e.Cancel)
				{
					RestoreBackup(destPath, backup);
				}
				else
				{
					if (destPath.Equals(this.FilePath, StringComparison.InvariantCultureIgnoreCase))
					{
						for (int i = 0; i < Subfiles.Count; i++)
						{
							ppSubfile subfile = Subfiles[i] as ppSubfile;
							if ((subfile != null) && subfile.ppPath.Equals(backup, StringComparison.InvariantCultureIgnoreCase))
							{
								subfile.ppPath = this.FilePath;
							}
						}
					}
					else
					{
						this.FilePath = destPath;
					}

					if ((backup != null) && !keepBackup)
					{
						File.Delete(backup);
					}
				}
			}
			catch (Exception ex)
			{
				RestoreBackup(destPath, backup);
				Utility.ReportException(ex);
			}
		}

		void RestoreBackup(string destPath, string backup)
		{
			if (File.Exists(destPath) && File.Exists(backup))
			{
				File.Delete(destPath);

				if (backup != null)
				{
					File.Move(backup, destPath);

					if (destPath.Equals(this.FilePath, StringComparison.InvariantCultureIgnoreCase))
					{
						for (int i = 0; i < Subfiles.Count; i++)
						{
							ppSubfile subfile = Subfiles[i] as ppSubfile;
							if ((subfile != null) && subfile.ppPath.Equals(backup, StringComparison.InvariantCultureIgnoreCase))
							{
								subfile.ppPath = this.FilePath;
							}
						}
					}
				}
			}
		}
	}
}
