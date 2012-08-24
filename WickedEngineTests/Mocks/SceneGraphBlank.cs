using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WickedEngine;

namespace WickedEngineTests.Mocks
{
    internal class SceneGraphBlank : ISceneGraph
    {
        public ReadOnlyCollection<GameObject> RootGraph
        {
            get { return _RootGraph.AsReadOnly(); }
        }

        private List<GameObject> _RootGraph;

        public SceneGraphBlank()
        {
            _RootGraph= new List<GameObject>();
        }

        public void Add(GameObject obj)
        {
            _RootGraph.Add(obj);
        }
    }
}
