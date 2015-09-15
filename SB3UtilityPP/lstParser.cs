using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SB3Utility
{
	public class lstParser : IWriteFile
	{
		public string Name { get; set; }
		public string Text { get; set; }

		public lstParser(Stream stream, string name)
		{
			this.Name = name;

			using (BinaryReader reader = new BinaryReader(stream))
			{
				List<byte> byteList = new List<byte>();
				try
				{
					for (; true; )
					{
						byteList.Add(reader.ReadByte());
					}
				}
				catch (EndOfStreamException) { }
				this.Text += Utility.EncodingShiftJIS.GetString(byteList.ToArray());
			}
		}

		public void WriteTo(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(Utility.EncodingShiftJIS.GetBytes(this.Text.ToCharArray()));
		}
	}
}
