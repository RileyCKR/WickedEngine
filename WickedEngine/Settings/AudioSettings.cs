using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedEngine.Settings
{
    [Serializable]
    public class AudioSettings : GameSettings, IGameSettings
    {
        private string _SettingsFile = "AudioSettings.xml";
        public new string SettingsFile { get { return _SettingsFile; } }

        private bool _SoundEnabled = true;
        /// <summary>
        /// Sound Enabled Flag
        /// </summary>
        public bool SoundEnabled
        {
            get { return _SoundEnabled; }
            set
            {
                if (_SoundEnabled != value)
                {
                    NeedSave = true;
                    _SoundEnabled = value;
                }
            }
        }

        private int _SoundVolume = 100;
        /// <summary>
        /// Sound volume.
        /// </summary>
        public int SoundVolume
        {
            get { return _SoundVolume; }
            set
            {
                if (_SoundVolume != value)
                {
                    NeedSave = true;
                    _SoundVolume = value;
                    if (_SoundVolume < 0) _SoundVolume = 0;
                    if (_SoundVolume > 100) _SoundVolume = 100;
                }
            }
        }
        public float SoundVolumePercent
        {
            get { return SoundVolume / 100f; }
        }

        private bool _MusicEnabled = true;
        public bool MusicEnabled
        {
            get { return _MusicEnabled; }
            set
            {
                if (_MusicEnabled != value)
                {
                    NeedSave = true;
                    _MusicEnabled = value;
                }
            }
        }
        private int _musicVolume = 30;
        /// <summary>
        /// Music volume.
        /// </summary>
        public int MusicVolume
        {
            get { return _musicVolume; }
            set
            {
                if (_musicVolume != value)
                {
                    NeedSave = true;
                    _musicVolume = value;
                    if (_musicVolume < 0) _musicVolume = 0;
                    if (_musicVolume > 100) _musicVolume = 100;
                }
            }
        }
    }
}
