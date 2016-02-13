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
		SB3,
		SMFigure,
		SMTrial,
		SMRetail,
		AG3Welcome,
		AG3Retail,
		DG,
		HakoTrial,
		HakoRetail,
		SMSweets,
		EskMate,
		AHMFigure,
		AHMTrial,
		AHMRetail,
		YuushaTrial,
		YuushaRetail,
		RGF,
		SM2Trial,
		SM2Retail,
		SBZ,
		CharacolleAlicesoft,
		CharacolleBaseson,
		CharacolleKey,
		CharacolleLumpOfSugar,
		CharacolleWhirlpool,
		BestCollection,
		AATrial,
		AARetail,
		AAJCH,
		Wakeari,
		LoveGirl,
		Hero,
		HET,
		HETDTL,
		PPD,
		Musumakeup,
		ImmoralWard,
		RealPlayTrial,
		RealPlay,
		AA2
	}

	public abstract class ppFormat
	{
		public static ppFormat[] Array = new ppFormat[] {
			new ppFormat_SB3(),
			new ppFormat_SMFigure(),
			new ppFormat_SMTrial(),
			new ppFormat_SMRetail(),
			new ppFormat_AG3Welcome(),
			new ppFormat_AG3Retail(),
			new ppFormat_DG(),
			new ppFormat_HakoTrial(),
			new ppFormat_HakoRetail(),
			new	ppFormat_SMSweets(),
			new ppFormat_EskMate(),
			new ppFormat_AHMFigure(),
			new ppFormat_AHMTrial(),
			new ppFormat_AHMRetail(),
			new ppFormat_YuushaTrial(),
			new ppFormat_YuushaRetail(),
			new ppFormat_RGF(),
			new ppFormat_SM2Trial(),
			new ppFormat_SM2Retail(),
			new ppFormat_SBZ(),
			new ppFormat_CharacolleAlicesoft(),
			new ppFormat_CharacolleBaseson(),
			new ppFormat_CharacolleKey(),
			new ppFormat_CharacolleLumpOfSugar(),
			new ppFormat_CharacolleWhirlpool(),
			new ppFormat_BestCollection(),
			new ppFormat_AATrial(),
			new ppFormat_AARetail(),
			new ppFormat_AAJCH(),
			new ppFormat_Wakeari(),
			new ppFormat_LoveGirl(),
			new ppFormat_Hero(),
			new ppFormat_HET(),
			new ppFormat_HETDTL(),
			new ppFormat_PPD(),
			new ppFormat_Musumakeup(),
			new ppFormat_ImmoralWard(),
			new ppFormat_RealPlayTrial(),
			new ppFormat_RealPlay(),
			new ppFormat_AA2()
		};

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

	public class ppFormat_SB3 : ppFormatCrypto
	{
		public ppFormat_SB3() : base("OS2 / RL / SB3", ppHeader.Array[(int)ppHeaderIdx.SB3], ppFormatIdx.SB3) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformSB3();
		}
	}

	public class ppFormat_SMFigure : ppFormatCrypto
	{
		public ppFormat_SMFigure() : base("SM Figure", ppHeader.Array[(int)ppHeaderIdx.SMFigure], ppFormatIdx.SMFigure) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x58,0x62,0x86,0xD2,0x3B,0x2F,0xC4,0x5F,
				0xEE,0x58,0x76,0x2D,0xB4,0x02,0x02,0xCD,
				0x0A,0x08,0x40,0x30,0x08,0x66,0x1D,0xE8,
				0x9B,0xA6,0x61,0xCB,0x63,0xF3,0xF3,0xB4 });
		}
	}

	public abstract class ppFormat_SMHeader : ppFormatCrypto
	{
		public ppFormat_SMHeader(string name, ppFormatIdx idx)
			: base(name, ppHeader.Array[(int)ppHeaderIdx.SMRetail], idx)
		{
		}
	}

	public abstract class ppFormat_AG3Header : ppFormatCrypto
	{
		public ppFormat_AG3Header(string name, ppFormatIdx idx)
			: base(name, ppHeader.Array[(int)ppHeaderIdx.AG3], idx)
		{
		}
	}

	public class ppFormat_SMTrial : ppFormat_SMHeader
	{
		public ppFormat_SMTrial() : base("SM Trial", ppFormatIdx.SMTrial) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x7C,0xF2,0x35,0x77,0x54,0x18,0x20,0x6E,
				0x9C,0x7B,0x9E,0x85,0x1F,0xB5,0x71,0x40,
				0x25,0xAD,0x71,0x43,0x64,0x20,0x20,0x7E,
				0xCF,0xE3,0x85,0xC0,0x41,0xDE,0x23,0x12 });
		}
	}

	public class ppFormat_SMRetail : ppFormat_SMHeader
	{
		public ppFormat_SMRetail() : base("SM Retail", ppFormatIdx.SMRetail) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x1E,0x5D,0x13,0xDD,0x7D,0x4C,0x4F,0xA7,
				0xDB,0xA7,0x29,0x14,0x10,0xF8,0xC0,0xBE,
				0x44,0x7F,0xD0,0x63,0x1C,0x22,0x7C,0x9F,
				0xE8,0xB9,0xF8,0xBE,0x58,0xB3,0xEF,0xF4 });
		}
	}

	public class ppFormat_AG3Welcome : ppFormat_SMHeader
	{
		public ppFormat_AG3Welcome() : base("AG3 Welcome", ppFormatIdx.AG3Welcome) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0xE5,0x77,0x64,0x05,0xD2,0x37,0x4D,0x2E,
				0xB7,0x4A,0xB7,0x2B,0x22,0x70,0xF1,0xD6,
				0xC7,0xE7,0x61,0x6D,0x10,0xED,0xF5,0xC1,
				0xD9,0x08,0x28,0xEC,0xE2,0x09,0xEA,0xD7 });
		}
	}

	public class ppFormat_AG3Retail : ppFormat_AG3Header
	{
		public ppFormat_AG3Retail() : base("AG3 Retail", ppFormatIdx.AG3Retail) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformTwoCodes(
				new int[] { 0x00CA, 0x006E, 0x000D, 0x00B3 },
				new int[] { 0x009C, 0x0036, 0x001E, 0x00E8 });
		}
	}

	public class ppFormat_DG : ppFormat_AG3Header
	{
		public ppFormat_DG() : base("Digital Girl", ppFormatIdx.DG) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformTwoCodes(
				new int[] { 0x2110, 0x8BD0, 0x5063, 0xD8F6 },
				new int[] { 0x7311, 0xA15A, 0x9132, 0xA8E9 });
		}
	}

	public class ppFormat_HakoTrial : ppFormat_SMHeader
	{
		public ppFormat_HakoTrial() : base("Hako Trial", ppFormatIdx.HakoTrial) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x11,0x73,0x10,0x21,0x5A,0xA1,0xD0,0x8B,
				0x32,0x91,0x63,0x50,0xE9,0xA8,0xF6,0xD8,
				0x40,0x72,0x80,0xF9,0xEC,0x79,0x6E,0x8D,
				0x36,0x72,0x2B,0xA1,0x76,0xB6,0x67,0x92 });
		}
	}

	public class ppFormat_HakoRetail : ppFormat_AG3Header
	{
		public ppFormat_HakoRetail() : base("Hako Retail", ppFormatIdx.HakoRetail) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformTwoCodes(
				new int[] { 0xCBEE, 0x1675, 0x3533, 0x4CE6 },
				new int[] { 0x2F68, 0x936D, 0xF40D, 0x0539 });
		}
	}

	public class ppFormat_SMSweets : ppFormat_AG3Header
	{
		public ppFormat_SMSweets() : base("SM Sweets", ppFormatIdx.SMSweets) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformTwoCodes(
				new int[] { 0x3F86, 0xB8D5, 0x4AB4, 0x06F4 },
				new int[] { 0x70F6, 0x078A, 0x2F26, 0x3572 });
		}
	}

	public class ppFormat_EskMate : ppFormat_SMHeader
	{
		public ppFormat_EskMate() : base("Esk Mate", ppFormatIdx.EskMate) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0xE9,0xEC,0xFC,0x9F,0x67,0x4A,0x91,0x8D,
				0x72,0x4F,0x5F,0xAE,0xBB,0xA5,0xF7,0x0A,
				0x12,0xB9,0x03,0xC5,0x4E,0x1C,0xE3,0x7A,
				0x7E,0xF4,0x05,0x48,0x51,0x18,0x16,0x99 });
		}
	}

	public class ppFormat_AHMFigure : ppFormat_SMHeader
	{
		public ppFormat_AHMFigure() : base("AHM Figure", ppFormatIdx.AHMFigure) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0xAB,0x2C,0xC4,0x4E,0x7B,0xDF,0xBD,0x17,
				0xDC,0x2E,0x23,0x1E,0x4B,0xE5,0x80,0x3C,
				0x93,0xB1,0x1D,0x8C,0x81,0x36,0xB3,0x88,
				0x35,0x2D,0x30,0x4B,0x10,0x66,0xC8,0xE6 });
		}
	}

	public class ppFormat_AHMTrial : ppFormat_SMHeader
	{
		public ppFormat_AHMTrial() : base("AHM Trial", ppFormatIdx.AHMTrial) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x67,0xF9,0x30,0x5A,0x09,0xAB,0xF5,0x60,
				0xD6,0x9F,0xFD,0x93,0xBA,0x9C,0xF5,0x60,
				0x11,0x6A,0xBA,0x79,0x4C,0x41,0x4A,0x8D,
				0xC7,0xBA,0xBB,0x9C,0x26,0x34,0x0F,0xEF });
		}
	}

	public class ppFormat_AHMRetail : ppFormat_AG3Header
	{
		public ppFormat_AHMRetail() : base("AHM Retail", ppFormatIdx.AHMRetail) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformTwoCodes(
				new int[] { 0x717E, 0x0E78, 0xAFE7, 0x8FA7 },
				new int[] { 0x9E1F, 0xC5E3, 0x0008, 0x713A });
		}
	}

	public class ppFormat_YuushaTrial : ppFormat_SMHeader
	{
		public ppFormat_YuushaTrial() : base("Yuusha MIK Trial", ppFormatIdx.YuushaTrial) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0xD1,0xEC,0x08,0xA1,0x48,0x7F,0xD6,0x8F,
				0xAD,0x34,0xB2,0xA2,0x35,0x4D,0x55,0xD1,
				0x1F,0xC1,0xB4,0x47,0x2F,0x54,0x89,0x24,
				0x61,0xCE,0xB7,0xA5,0x22,0x80,0x05,0x29 });
		}
	}

	public class ppFormat_YuushaRetail : ppFormat_AG3Header
	{
		public ppFormat_YuushaRetail() : base("Yuusha Retail", ppFormatIdx.YuushaRetail) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformTwoCodes(
				new int[] { 0xA82B, 0x1EF2, 0x1DDD, 0xC895 },
				new int[] { 0xD47E, 0x764F, 0x416F, 0xC7BF });
		}
	}

	public class ppFormat_RGF : ppFormat_SMHeader
	{
		public ppFormat_RGF() : base("RGF", ppFormatIdx.RGF) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x58,0x62,0x86,0xD2,0x3B,0x2F,0xC4,0x5F,
				0xEE,0x58,0x76,0x2D,0xB4,0x02,0x02,0xCD,
				0x0A,0x08,0x40,0x30,0x08,0x66,0x1D,0xE8,
				0x9B,0xA6,0x61,0xCB,0x63,0xF3,0xF3,0xB4 });
		}
	}

	public class ppFormat_SM2Trial : ppFormat_SMHeader
	{
		public ppFormat_SM2Trial() : base("SM2 Trial", ppFormatIdx.SM2Trial) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x85,0x45,0x1B,0xBC,0x6E,0xDA,0x0E,0xA6,
				0x3F,0xCE,0x98,0x7D,0xD7,0x68,0xD9,0xEF,
				0xB4,0x3C,0x86,0xEF,0x4B,0x0D,0x08,0x28,
				0xF7,0xDE,0x12,0xA6,0xB7,0x0A,0x61,0x7A });
		}
	}

	public class ppFormat_SM2Retail : ppFormat_AG3Header
	{
		public ppFormat_SM2Retail() : base("SM2 Retail", ppFormatIdx.SM2Retail) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformTwoCodes(
				new int[] { 0xCE43, 0x6F31, 0xFC65, 0x9D2F },
				new int[] { 0x4182, 0xC473, 0x9D75, 0xD5B7 });
		}
	}

	public class ppFormat_SBZ : ppFormat_SMHeader
	{
		public ppFormat_SBZ() : base("SBZ", ppFormatIdx.SBZ) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x14,0x6F,0x07,0xB8,0x9A,0x0E,0x84,0x44,
				0x59,0x25,0x8E,0x18,0xBC,0x39,0x9E,0x5C,
				0x99,0x7A,0xA0,0x92,0xD4,0xB7,0xBC,0x55,
				0x1E,0x2E,0x88,0x27,0x14,0xA1,0xE6,0x27 });
		}
	}

	public class ppFormat_CharacolleAlicesoft : ppFormat_SMHeader
	{
		public ppFormat_CharacolleAlicesoft() : base("Characolle AliceSoft", ppFormatIdx.CharacolleAlicesoft) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0xEB,0xD6,0x6B,0x29,0x21,0x03,0xA9,0x2C,
				0x5F,0x5F,0xEF,0xBB,0xEC,0x10,0xFC,0x4C,
				0x51,0xED,0xD4,0xBE,0x99,0x4D,0x45,0x06,
				0x65,0x51,0x8E,0x25,0x33,0x5C,0x05,0x53 });
		}
	}

	public class ppFormat_CharacolleBaseson : ppFormat_SMHeader
	{
		public ppFormat_CharacolleBaseson() : base("Characolle BaseSon", ppFormatIdx.CharacolleBaseson) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0xD4,0xED,0x6B,0x29,0x0A,0x1A,0xA9,0x2C,
				0x48,0x76,0xEF,0xBB,0xD5,0x27,0xFC,0x4C,
				0x3A,0x04,0xD5,0xBE,0x82,0x64,0x45,0x06,
				0x4E,0x68,0x8E,0x25,0x1C,0x73,0x05,0x53 });
		}
	}

	public class ppFormat_CharacolleKey : ppFormat_SMHeader
	{
		public ppFormat_CharacolleKey() : base("Characolle Key", ppFormatIdx.CharacolleKey) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0xCE,0xFA,0x6A,0x29,0x04,0x27,0xA8,0x2C,
				0x42,0x83,0xEE,0xBB,0xCF,0x34,0xFB,0x4C,
				0x34,0x11,0xD4,0xBE,0x7C,0x71,0x44,0x06,
				0x48,0x75,0x8D,0x25,0x16,0x80,0x04,0x53 });
		}
	}

	public class ppFormat_CharacolleLumpOfSugar : ppFormat_SMHeader
	{
		public ppFormat_CharacolleLumpOfSugar() : base("Characolle Lump of Sugar", ppFormatIdx.CharacolleLumpOfSugar) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0xCD,0xF2,0x6B,0x29,0x03,0x1F,0xA9,0x2C,
				0x41,0x7B,0xEF,0xBB,0xCE,0x2C,0xFC,0x4C,
				0x33,0x09,0xD5,0xBE,0x7B,0x69,0x45,0x06,
				0x47,0x6D,0x8E,0x25,0x15,0x78,0x05,0x53 });
		}
	}

	public class ppFormat_CharacolleWhirlpool : ppFormat_SMHeader
	{
		public ppFormat_CharacolleWhirlpool() : base("Characolle Whirlpool", ppFormatIdx.CharacolleWhirlpool) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0xDF,0x9F,0x6B,0x29,0x15,0xCC,0xA8,0x2C,
				0x53,0x28,0xEF,0xBB,0xE0,0xD9,0xFB,0x4C,
				0x45,0xB6,0xD4,0xBE,0x8D,0x16,0x45,0x06,
				0x59,0x1A,0x8E,0x25,0x27,0x25,0x05,0x53 });
		}
	}

	public class ppFormat_BestCollection : ppFormat_AG3Header
	{
		public ppFormat_BestCollection() : base("Best Collection", ppFormatIdx.BestCollection) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformTwoCodes(
				new int[] { 0xCD8D, 0x28AA, 0x3A3F, 0xE801 },
				new int[] { 0x55A7, 0x8D89, 0x1809, 0xF0AC });
		}
	}

	public class ppFormat_AATrial : ppFormat_SMHeader
	{
		public ppFormat_AATrial() : base("AA Trial", ppFormatIdx.AATrial) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0xC7,0xE7,0x61,0x6D,0x10,0xED,0xF5,0xC1,
				0xD9,0x08,0x28,0xEC,0xE2,0x09,0xEA,0xD7,
				0x17,0xB0,0x10,0xCE,0xA3,0x4B,0x82,0xE9,
				0x23,0x2F,0x62,0x93,0xB2,0x10,0xD8,0xE9 });
		}
	}

	public class ppFormat_AARetail : ppFormat_SMHeader
	{
		public ppFormat_AARetail() : base("AA Retail", ppFormatIdx.AARetail) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x8B,0x11,0x73,0x10,0x50,0x5A,0xA1,0xD0,
				0xD8,0x32,0x91,0x63,0xF9,0xE9,0xA8,0xF6,
				0x8D,0x40,0x72,0x80,0xA1,0xEC,0x79,0x6E,
				0x92,0x36,0x72,0x2B,0x35,0x76,0xB6,0x67 });
		}
	}

	public abstract class ppFormat_WakeariHeader : ppFormatCrypto
	{
		public ppFormat_WakeariHeader(string name, ppFormatIdx idx)
			: base(name, ppHeader.Array[(int)ppHeaderIdx.Wakeari], idx)
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

	public class ppFormat_Wakeari : ppFormat_WakeariHeader
	{
		public ppFormat_Wakeari() : base("Wakeari", ppFormatIdx.Wakeari) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x9D,0xF0,0x69,0x74,0xD6,0xB7,0x1D,0x7B,
				0x78,0xF4,0xEA,0x65,0x29,0x5F,0x96,0xB4,
				0xEE,0xE6,0x83,0x0E,0x37,0xFF,0x8D,0xEF,
				0x1C,0x3C,0x36,0x9C,0xE6,0x1F,0x01,0x58 });
		}
	}

	public class ppFormat_LoveGirl : ppFormat_WakeariHeader
	{
		public ppFormat_LoveGirl() : base("LoveGirl", ppFormatIdx.LoveGirl) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x71,0x01,0x9B,0x18,0xD7,0x25,0x0D,0xEB,
				0x02,0x0B,0x3D,0x80,0x0B,0x44,0x00,0xA9,
				0xFF,0x68,0xD5,0xAD,0xDF,0x65,0xC5,0xF8,
				0xEB,0x16,0x8D,0x10,0x20,0x18,0xFF,0xCB });
		}
	}

	public class ppFormat_Hero : ppFormat_WakeariHeader
	{
		public ppFormat_Hero() : base("Hero", ppFormatIdx.Hero) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x10,0x0C,0x20,0x7F,0xFB,0x71,0x58,0x1D,
				0x32,0x11,0x27,0x4F,0x8F,0xA8,0x6A,0xA8,
				0x70,0x1D,0xED,0x66,0x0E,0x62,0x27,0x40,
				0x0A,0x9D,0x24,0x5F,0x49,0x85,0xC2,0xAA });
		}
	}

	public class ppFormat_HET : ppFormat_WakeariHeader
	{
		public ppFormat_HET() : base("HET", ppFormatIdx.HET) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x54, 0x31, 0x47, 0x70, 0x3E, 0x12, 0xF3, 0xB2,
				0x25, 0xD2, 0xB6, 0x94, 0x44, 0x0F, 0x74, 0xA3,
				0xE0, 0xB7, 0x50, 0x05, 0x1E, 0x6D, 0xD7, 0xBB,
				0x17, 0x2E, 0x7A, 0x23, 0x2E, 0x34, 0x42, 0xC1 });
		}
	}

	public class ppFormat_HETDTL : ppFormat_WakeariHeader
	{
		public ppFormat_HETDTL() : base("HET DTL", ppFormatIdx.HETDTL) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x97, 0x03, 0x8E, 0x6E, 0x57, 0xE0, 0xEC, 0xAC,
				0x9B, 0xE4, 0x78, 0xAB, 0x19, 0x18, 0x32, 0x31,
				0x23, 0xA5, 0x13, 0xAE, 0x72, 0xE8, 0xAB, 0xBA,
				0x23, 0xC9, 0x09, 0xBF, 0xFF, 0x49, 0xDB, 0xF3 });
		}
	}

	public class ppFormat_PPD : ppFormat_WakeariHeader
	{
		public ppFormat_PPD() : base("PPD", ppFormatIdx.PPD) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x69, 0x55, 0x05, 0x29, 0x58, 0xD0, 0xA0, 0x11,
				0x7E, 0x59, 0x64, 0x6A, 0xDF, 0x21, 0x30, 0xEF,
				0x05, 0x29, 0x4A, 0xE4, 0xF6, 0x49, 0xAF, 0xF9,
				0x63, 0x77, 0xA8, 0x72, 0x22, 0x46, 0x9E, 0x85 });
		}
	}

	public class ppFormat_Musumakeup : ppFormat_WakeariHeader
	{
		public ppFormat_Musumakeup() : base("Musumakeup", ppFormatIdx.Musumakeup) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0xAD, 0x6B, 0x04, 0x3D, 0x86, 0x1B, 0x9B, 0x77,
				0xA6, 0xFB, 0x5F, 0x71, 0xB6, 0x01, 0x99, 0x17,
				0xB2, 0xB5, 0x61, 0xBE, 0x9E, 0xF0, 0xB5, 0xBF,
				0x75, 0xE3, 0xC9, 0x65, 0x63, 0x5C, 0x9E, 0x0A });
		}
	}

	public class ppFormat_ImmoralWard : ppFormat_WakeariHeader
	{
		public ppFormat_ImmoralWard() : base("Immoral Ward", ppFormatIdx.ImmoralWard) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0xE8, 0x7F, 0xB1, 0xF9, 0xD7, 0x1B, 0x35, 0x25,
				0x32, 0x55, 0x28, 0x4B, 0xEC, 0x61, 0xBD, 0x5F,
				0x57, 0xFE, 0xA5, 0xD7, 0x97, 0xFC, 0xDF, 0x0C,
				0x57, 0xFB, 0xD2, 0xE4, 0xBA, 0x75, 0x99, 0xAE });
		}
	}

	public class ppFormat_RealPlayTrial : ppFormat_WakeariHeader
	{
		public ppFormat_RealPlayTrial() : base("Real Play Trial", ppFormatIdx.RealPlayTrial) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0xDE, 0xBE, 0xEB, 0xF6, 0xB8, 0xA3, 0x25, 0xA1,
				0xDA, 0xBB, 0xFC, 0xC1, 0x79, 0xD1, 0x71, 0x5B,
				0xF2, 0x9F, 0x8C, 0x1D, 0xBC, 0xDA, 0xAB, 0x42,
				0x9B, 0x6F, 0x19, 0xB6, 0x17, 0x0D, 0xE2, 0x7A });
		}
	}

	public class ppFormat_RealPlay : ppFormat_WakeariHeader
	{
		public ppFormat_RealPlay() : base("Real Play", ppFormatIdx.RealPlay) { }

		protected override ICryptoTransform CryptoTransform()
		{
			return new CryptoTransformOneCode(new byte[] {
				0x38, 0x0C, 0x6B, 0xC1, 0x47, 0x75, 0x9D, 0xB8,
				0xAC, 0x56, 0x02, 0xF0, 0x8F, 0x14, 0x00, 0xE4,
				0xB0, 0x32, 0x9A, 0x2C, 0x4D, 0x0D, 0x37, 0x95,
				0x46, 0xC9, 0xA5, 0x77, 0x7F, 0x8B, 0x5D, 0x43 });
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

	public class ppFormat_AAJCH : ppFormat
	{
		public ppFormat_AAJCH() : base("AA JCH", ppHeader.Array[(int)ppHeaderIdx.AAJCH], ppFormatIdx.AAJCH) { }

		public override Stream ReadStream(Stream stream)
		{
			return new JchStream(stream, CompressionMode.Decompress, false);
		}

		public override Stream WriteStream(Stream stream)
		{
			return new JchStream(stream, CompressionMode.Compress, true);
		}

		public override object FinishWriteTo(Stream stream)
		{
			((JchStream)stream).Close();
			return null;
		}
	}

	#region CryptoTransform
	public class CryptoTransformSB3 : ICryptoTransform
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
			get { return 1; }
		}

		public int OutputBlockSize
		{
			get { return 1; }
		}

		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			for (int i = 0; i < inputCount; i++)
			{
				outputBuffer[outputOffset + i] = (byte)(~inputBuffer[inputOffset + i] + 1);
			}
			return inputCount;
		}

		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			byte[] outputBuffer = new byte[inputCount];
			for (int i = 0; i < inputCount; i++)
			{
				outputBuffer[i] = (byte)(~inputBuffer[inputOffset + i] + 1);
			}
			return outputBuffer;
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
		}
		#endregion

		public CryptoTransformSB3()
		{
		}
	}

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

	public class CryptoTransformTwoCodes : ICryptoTransform
	{
		#region ICryptoTransform Members
		public bool CanReuseTransform
		{
			get { return false; }
		}

		public bool CanTransformMultipleBlocks
		{
			get { return true; }
		}

		public int InputBlockSize
		{
			get { return 2; }
		}

		public int OutputBlockSize
		{
			get { return 2; }
		}

		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			for (int i = 0; i < inputCount; )
			{
				codeA[codeIdx] = codeA[codeIdx] + codeB[codeIdx];
				outputBuffer[outputOffset + i] = (byte)(inputBuffer[inputOffset + i] ^ codeA[codeIdx]);
				i++;
				outputBuffer[outputOffset + i] = (byte)(inputBuffer[inputOffset + i] ^ (codeA[codeIdx] >> 8));
				i++;
				codeIdx = (codeIdx + 1) & 0x3;
			}
			return inputCount;
		}

		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			byte[] outputBuffer = new byte[inputCount];
			int remainder = inputCount % 2;
			int transformLength = inputCount - remainder;
			for (int i = 0; i < transformLength; )
			{
				codeA[codeIdx] = codeA[codeIdx] + codeB[codeIdx];
				outputBuffer[i] = (byte)(inputBuffer[inputOffset + i] ^ codeA[codeIdx]);
				i++;
				outputBuffer[i] = (byte)(inputBuffer[inputOffset + i] ^ (codeA[codeIdx] >> 8));
				i++;
				codeIdx = (codeIdx + 1) & 0x3;
			}
			Array.Copy(inputBuffer, inputOffset + transformLength, outputBuffer, transformLength, remainder);
			return outputBuffer;
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
		}
		#endregion

		private int[] codeA = null;
		private int[] codeB = null;
		private int codeIdx = 0;

		public CryptoTransformTwoCodes(int[] codeA, int[] codeB)
		{
			this.codeA = codeA;
			this.codeB = codeB;
		}
	}
	#endregion
}
