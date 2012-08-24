using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace WickedEngine.Settings
{
    public static class SettingsManager
    {
        public static GameSettings GameSettings { get; set; }
        public static AudioSettings AudioSettings { get; set; }

        public static void Initialize()
        {
            GameSettings = new GameSettings();
            AudioSettings = new AudioSettings();
        }

        public static void Load()
        {
        }

        public static void Save()
        {
        }

        public static void Reset()
        {
        }
    }
}
