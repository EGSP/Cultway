using System;
using UnityEngine;

namespace Game.World
{
    public interface ITown
    {
        event Action<ITown> OnVisited;
        
        /// <summary>
        /// Был ли город посещен.
        /// </summary>
        bool Visited { get; set; }
        
        /// <summary>
        /// Текущая позиция города в пространстве.
        /// </summary>
        Vector3 Position { get; set; }
    }
}