﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace WickedEngine.GUI
{
    public interface IGameObject
    {
        SpriteBatch SpriteBatch { get; }
    }
}