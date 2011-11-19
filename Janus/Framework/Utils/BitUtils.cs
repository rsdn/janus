using System;
using System.Diagnostics;

namespace Rsdn.Janus
{
	public static class BitUtils
	{
		[DebuggerHidden]
		public static int LoWordAsInt(IntPtr n)
		{
			return LoWordAsInt((int)n);
		}

		[DebuggerHidden]
		public static int LoWordAsInt(int n)
		{
			return (short)(n & 0xffff);
		}

		[DebuggerHidden]
		public static int HiWordAsInt(IntPtr n)
		{
			return HiWordAsInt((int)n);
		}

		[DebuggerHidden]
		public static int HiWordAsInt(int n)
		{
			return (short)((n >> 0x10) & 0xffff);
		}
	}
}