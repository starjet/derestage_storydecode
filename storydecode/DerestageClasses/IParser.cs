using System;
using System.Runtime.InteropServices;

namespace Story.Data
{
	[Guid("DDA224F0-5F93-4F3C-9D65-0F0204B76DC0"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IParser
	{
		void ConvertStoryDataTextToBinary(ref string srcPath, ref string dstPath);

		void Init();
	}
}
