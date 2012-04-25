using System;
using System.Runtime.InteropServices;

namespace Rsdn.Janus.Framework
{
	/// <summary>
	/// Проигрыватель звука.
	/// </summary>
	public static class Beeper
	{
		private const string _defaultSound = ".Default";

		[DllImport("winmm.dll")]
		private static extern bool sndPlaySound(string lpszSound, SoundFlags fuSound);

		[DllImport("kernel32.dll")]
		private static extern bool Beep(int dwFreq, int dwDuration);

		public static void DoBeep(string sound = _defaultSound)
		{
			if (string.IsNullOrEmpty(sound))
				Beep(1000, 500);
			else
				sndPlaySound(sound, SoundFlags.Asynchronous);
		}

		#region Nested type: SoundFlags
		[Flags]
		private enum SoundFlags
		{
			/// <summary>
			/// The sound is played synchronously and 
			/// the function does not return until the sound ends.
			/// </summary>
			//Synchronous = 0x0000,
			/// <summary>
			/// The sound is played asynchronously and
			/// the function returns immediately after beginning the sound.
			/// To terminate an asynchronously played sound,
			/// call sndPlaySound with lpszSoundName set to <c>null</c>.
			/// </summary>
			Asynchronous = 0x0001,
			
		}
		#endregion
	}
}