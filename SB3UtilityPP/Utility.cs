using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;

namespace SB3Utility
{
	public static partial class Utility
	{
		public static Encoding EncodingShiftJIS = Encoding.GetEncoding("Shift-JIS");
		public static CultureInfo CultureUS = new CultureInfo("en-US");
		public static readonly int BufSize = 0x400000;

		public static string GetDestFile(DirectoryInfo dir, string prefix, string ext)
		{
			string dest = dir.FullName + @"\" + prefix;
			int destIdx = 0;
			while (File.Exists(dest + destIdx + ext))
			{
				destIdx++;
			}
			dest += destIdx + ext;
			return dest;
		}

		public static T[] Convert<T>(object[] array)
		{
			T[] newArray = new T[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				newArray[i] = (T)array[i];
			}
			return newArray;
		}

		public static float[] ConvertToFloatArray(object[] array)
		{
			float[] newArray = new float[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				newArray[i] = (float)(double)array[i];
			}
			return newArray;
		}

		public static string DecryptName(byte[] buf)
		{
			if (buf.Length < 1)
			{
				return String.Empty;
			}

			byte[] decrypt = new byte[buf.Length];
			for (int i = 0; i < decrypt.Length; i++)
			{
				decrypt[i] = (byte)~buf[i];
			}
			return EncodingShiftJIS.GetString(decrypt).TrimEnd(new char[] { '\0' });
		}

		public static byte[] EncryptName(string name)
		{
			if (name.Length < 1)
			{
				return new byte[0];
			}

			byte[] buf = EncodingShiftJIS.GetBytes(name);
			byte[] encrypt = new byte[buf.Length + 1];
			buf.CopyTo(encrypt, 0);
			for (int i = 0; i < encrypt.Length; i++)
			{
				encrypt[i] = (byte)~encrypt[i];
			}
			return encrypt;
		}

		public static byte[] EncryptName(string name, int length)
		{
			byte[] encrypt = new byte[length];
			EncodingShiftJIS.GetBytes(name).CopyTo(encrypt, 0);
			for (int i = 0; i < encrypt.Length; i++)
			{
				encrypt[i] = (byte)~encrypt[i];
			}
			return encrypt;
		}

		public static float ParseFloat(string s)
		{
			return Single.Parse(s, CultureUS);
		}

		public static void ReportException(Exception ex)
		{
			Exception inner = ex;
			while (inner != null)
			{
				Report.ReportLog(inner.Message);
				inner = inner.InnerException;
			}
		}

		public static string BytesToString(byte[] bytes)
		{
			if (bytes == null)
			{
				return String.Empty;
			}

			StringBuilder s = new StringBuilder(bytes.Length * 3 + 1);
			for (int i = 0; i < bytes.Length; i++)
			{
				for (int j = 0; (j < 3) && (i < bytes.Length); i++, j++)
				{
					s.Append(bytes[i].ToString("X2") + "-");
				}
				if (i < bytes.Length)
				{
					s.Append(bytes[i].ToString("X2") + " ");
				}
			}
			if (s.Length > 0)
			{
				s.Remove(s.Length - 1, 1);
			}
			return s.ToString();
		}

		public static bool ValidFilePath(string path)
		{
			try
			{
				FileInfo file = new FileInfo(path);
			}
			catch
			{
				return false;
			}

			return true;
		}

        public static void WriteBytes(this Stream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }
	}
}
