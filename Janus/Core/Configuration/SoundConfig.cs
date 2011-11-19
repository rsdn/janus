using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Rsdn.Janus
{
	/// <summary>
	/// Настройки звуков нотификации.
	/// </summary>
	public class SoundConfig : SubConfigBase
	{
		private const bool _defaultMakeSound = true;
		private bool _makeSound = _defaultMakeSound;

		[JanusDisplayName(SR.Config.Sound.Use.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Sound.Use.DescriptionResourceName)]
		[DefaultValue(_defaultMakeSound)]
		[SortIndex(5)]
		public bool MakeSound
		{
			get { return _makeSound; }
			set { _makeSound = value; }
		}

		private const string _defaultSoundFile = @"sound\Alarm.wav";
		private string _soundFile = _defaultSoundFile;

		[JanusDisplayName(SR.Config.Sound.File.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Sound.File.DescriptionResourceName)]
		[Editor(typeof (WavFileEditor), typeof (UITypeEditor))]
		[DefaultValue(_defaultSoundFile)]
		[SortIndex(6)]
		public string SoundFile
		{
			get { return _soundFile; }
			set { _soundFile = value; }
		}

		private class WavFileEditor : FileNameEditor
		{
			protected override void InitializeDialog(OpenFileDialog ofd)
			{
				ofd.Filter = "Wav file (*.wav)|*.wav";
			}
		}
	}
}
