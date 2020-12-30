using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Game.World
{
    public interface ITownProvider
    {
        [CanBeNull]
        ICollection<ITown> Towns { get; } 
    }
}