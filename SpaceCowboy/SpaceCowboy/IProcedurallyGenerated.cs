using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceCowboy
{
    public interface IProcedurallyGenerated
    {
        void Generate(Random rng, int mass);
    }
}