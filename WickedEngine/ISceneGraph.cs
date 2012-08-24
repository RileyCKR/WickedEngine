using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace WickedEngine
{
    public interface ISceneGraph
    {
        ReadOnlyCollection<GameObject> RootGraph { get; }

        void Add(GameObject obj);
    }
}