using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace SB3Utility
{
	public enum ppFormatIdx
	{
        Wakeari = 29,
        AA2 = 39
	}

	public abstract class ppFormat
	{
		public abstract Stream ReadStream(Stream stream);
		public abstract Stream WriteStream(Stream stream);
		public abstract object FinishWriteTo(Stream stream);

		private string Name { get; set; }
		public ppHeader ppHeader { get; protected set; }
		public ppFormatIdx ppFormatIdx { get; protected set; }

		protected ppFormat(string name, ppHeader header, ppFormatIdx idx)
		{
			this.Name = name;
			this.ppHeader = header;
			this.ppFormatIdx = idx;
		}

		public override string ToString()
		{
			return Name;
		}
	}

	public abstract class ppFormatCrypto : ppFormat
	{
		protected abstract ICryptoTransform CryptoTransform();

		protected ppFormatCrypto(string name, ppHeader header, ppFormatIdx idx) : base(name, header, idx) { }

		public override Stream ReadStream(Stream stream)
		{
			return new CryptoStream(stream, CryptoTransform(), CryptoStreamMode.Read);
		}

		public override Stream WriteStream(Stream stream)
		{
			return new CryptoStream(stream, CryptoTransform(), CryptoStreamMode.Write);
		}

		public override object FinishWriteTo(Stream stream)
		{
			((CryptoStream)stream).FlushFinalBlock();
			return null;
		}
	}

	public abstract class ppFormat_WakeariHeader : ppFormatCrypto
	{
		public ppFormat_WakeariHeader(string name, ppFormatIdx idx)
			: base(name, new ppHeader_Wakeari(), idx)
		{
		}

		public override Stream WriteStream(Stream stream)
		{
			return new WakeariStream(stream, CryptoTransform(), CryptoStreamMode.Write);
		}

		public override object FinishWriteTo(Stream stream)
		{
			base.FinishWriteTo(stream);

			ppHeader_Wakeari.Metadata metadata = new ppHeader_Wakeari.Metadata();
			metadata.LastBytes = ((WakeariStream)stream).LastBytes;
			return metadata;
		}
	}

	public class ppFormat_AA2 : ppFormat_WakeariHeader
	{
		public ppFormat_AA2() : base("AA2", ppFormatIdx.AA2) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x4D, 0x2D, 0xBF, 0x6A, 0x5B, 0x4A, 0xCE, 0x9D,
				0xF4, 0xA5, 0x16, 0x87, 0x92, 0x9B, 0x13, 0x03,
				0x8F, 0x92, 0x3C, 0xF0, 0x98, 0x81, 0xDB, 0x8E,
				0x5F, 0xB4, 0x1D, 0x2B, 0x90, 0xC9, 0x65, 0x00 });
		}
	}

	#region CryptoTransform

	public class CryptoTransformOneCode : ICryptoTransform
	{
		#region ICryptoTransform Members
		public bool CanReuseTransform
		{
			get { return true; }
		}

		public bool CanTransformMultipleBlocks
		{
			get { return true; }
		}

		public int InputBlockSize
		{
			get { return code.Length; }
		}

		public int OutputBlockSize
		{
			get { return code.Length; }
		}

		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			int transformCount = 0;
			while (transformCount < inputCount)
			{
				for (int i = 0; i < code.Length; i++, transformCount++)
				{
					outputBuffer[outputOffset + transformCount] = (byte)(inputBuffer[inputOffset + transformCount] ^ code[i]);
				}
			}
			return transformCount;
		}

		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			byte[] outputBuffer = new byte[inputCount];
			int remainder = inputCount % 4;
			int transformLength = inputCount - remainder;
			for (int i = 0; i < transformLength; i++)
			{
				outputBuffer[i] = (byte)(inputBuffer[inputOffset + i] ^ code[i]);
			}
			Array.Copy(inputBuffer, inputOffset + transformLength, outputBuffer, transformLength, remainder);
			return outputBuffer;
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			throw new NotImplementedException();
		}
		#endregion

		private byte[] code = null;

		public CryptoTransformOneCode(byte[] code)
		{
			this.code = code;
		}
	}
	#endregion
}
