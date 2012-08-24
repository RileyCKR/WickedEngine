using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedEngine.Settings
{
    [Serializable]
    public class GameSettings : IGameSettings
    {
        private bool _NeedSave = false;
        public bool NeedSave
        {
            get { return _NeedSave; }
            protected set { _NeedSave = value; }
        }

        private string _SettingsFile = "GameSettings.xml";
        public string SettingsFile { get { return _SettingsFile; } }
    }
}
