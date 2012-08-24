using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WickedEngine
{
    //TODO: SceneGraph may be doing too many things, consider splitting object management
    //from the updating, collisions, and drawing
    public class SceneGraph : ISceneGraph
    {
        #region Fields

        private SpriteBatch _SpriteBatch;

        #endregion

        #region Properties

        public ICamera Camera { get; set; }

        public ReadOnlyCollection<GameObject> RootGraph { get; private set; }

        private List<GameObject> _RootGraphList { get; set; }

        public IMap Map { get; set; }
        
        public int NodesCulled { get; private set; }

        public Texture2D Sky { get; set; }

        #endregion

        #region Constructors

        public SceneGraph(ICamera camera, IMap map)
        {
            this._RootGraphList = new List<GameObject>();
            this.RootGraph = _RootGraphList.AsReadOnly();
            this.Camera = camera;
            this.Map = map;
        }

        #endregion

        #region Collection Methods

        public void Add(GameObject obj)
        {
            _RootGraphList.Add(obj);
        }

        public void Remove()
        {
        }

        public void Clear()
        {
        }

        #endregion

        #region Methods

        public void Update()
        {
            //Moves the nodes
            UpdateRecursive(RootGraph);

            //Prevent first-level nodes from moving off the map
            LockToMap();

            ProcessCollisionsFirstLoop(RootGraph);

            if (Camera != null)
                Camera.Update(this.Map);
        }

        private void UpdateRecursive(IList<GameObject> layer)
        {
            //TODO: Items can be added to this list while the enumeration is happening,
            //should a buffer be implemented?
            for (int x = 0; x < layer.Count; x++)
            {
                GameObject node = layer[x];

                node.Update();

                UpdateRecursive(node.Children);
            }
        }

        private void ProcessCollisionsFirstLoop(IList<GameObject> layer)
        {
            for (int x = 0; x < layer.Count; x++)
            {
                GameObject node = layer[x];

                ProcessCollisionsRecursive(RootGraph, node);
            }
        }

        private void ProcessCollisionsRecursive(IList<GameObject> layer, GameObject caller)
        {
            for (int x = 0; x < layer.Count; x++)
            {
                GameObject node = layer[x];
                //Don't check for collisions with noclip objectws or with the object you are checking
                if (!node.NoClip && !caller.NoClip && node != caller)
                {
                    if (node.Intersects(caller))
                    {
                        caller.OnObjectCollision(node);
                        //TODO: Should we call OnCollision For both objects? (both fireballs should explode)
                    }
                }

                ProcessCollisionsRecursive(node.Children, caller);
            }
        }

        //TODO: Consider moving LockToMap to the Map Class
        public virtual void LockToMap()
        {
            if (Map != null)
            {
                foreach (GameObject node in RootGraph)
                {
                    if (!node.NoClip)
                    {
                        Rectangle bounds = node.CollisionBounds;
                        Vector2 newPosition = node.AbsolutePosition;

                        newPosition.X = MathHelper.Clamp(
                            newPosition.X,
                            0,
                            Map.WidthInPixels - node.CollisionBounds.Width);

                        newPosition.Y = MathHelper.Clamp(
                            newPosition.Y, 0,
                            Map.HeightInPixels - node.CollisionBounds.Height);

                        //node.AbsolutePosition = newPosition;
                        node.Position = newPosition;
                    }
                }
            }
        }

        public void DrawSky(SpriteBatch spriteBatch)
        {
            if (Sky != null)
            {
                spriteBatch.Draw(Sky, Vector2.Zero, null, Color.White);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this._SpriteBatch = spriteBatch;

            if (Map != null)
            {
                Map.Draw(spriteBatch, Camera.ViewportRectangle);
            }

            NodesCulled = 0;

            DrawRecursive(RootGraph);
        }

        private void DrawRecursive(IList<GameObject> layer)
        {
            for (int x = 0; x < layer.Count; x++)
            {
                GameObject node = layer[x];

                if (node.Visible)
                {
                    BoundingBox nodeBox = node.DrawBounds();
                    if (nodeBox.Intersects(Camera.ViewportBounds))
                    {
                        node.Draw(_SpriteBatch);
                        DrawRecursive(node.Children);
                    }
                    else
                    {
                        ++NodesCulled;
                    }
                }
            }
        }

        public List<GameObject> GetObjectsAtWorldPosition(Point position)
        {
            List<GameObject> list = new List<GameObject>();

            foreach (GameObject obj in RootGraph)
            {
                if (obj.CollisionBounds.Contains(position))
                {
                    list.Add(obj);
                }
            }

            return list;
        }

        #endregion
    }
}