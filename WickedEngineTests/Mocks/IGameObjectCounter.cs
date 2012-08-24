using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedEngineTests.Mocks
{
    internal interface IGameObjectCounter
    {
        GameObjectStatistics Stats { get; }
    }
}