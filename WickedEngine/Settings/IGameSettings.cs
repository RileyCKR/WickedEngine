using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedEngine.Settings
{
    public interface IGameSettings
    {
        bool NeedSave { get; }
        string SettingsFile { get; }
    }
}
