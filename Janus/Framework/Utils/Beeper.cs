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

		public static void DoBeep(string sound)
		{
			if (string.IsNullOrEmpty(sound))
				Beep(1000, 500);
			else
				sndPlaySound(sound, SoundFlags.Asynchronous);
		}

		public static void DoBeep()
		{
			DoBeep(_defaultSound);
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
			/// <summary>
			/// If the sound cannot be found, 
			/// the function returns silently without playing the default sound.
			/// </summary>
			//NoDefault = 0x0002,
			/// <summary>
			/// The parameter specified by lpszSoundName points 
			/// to an image of a waveform sound in memory.
			/// </summary>
			//Memory = 0x0004,
			/// <summary>
			/// The sound plays repeatedly until sndPlaySound is called 
			/// again with the lpszSoundName parameter set to <c>null</c>. 
			/// You must also specify the <see cref="Asynchronous"/> flag to loop sounds.
			/// </summary>
			//Loop = 0x0008,
			/// <summary>
			/// If a sound is currently playing, the function 
			/// immediately returns <c>false</c>, without playing the requested sound.
			/// </summary>
			//NoStop = 0x0010
		}
		#endregion
	}
}