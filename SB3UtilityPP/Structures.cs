using System;
using System.Collections.Generic;
using System.IO;

namespace SB3Utility
{
	public interface IObjInfo
	{
		void WriteTo(Stream stream);
	}
}
