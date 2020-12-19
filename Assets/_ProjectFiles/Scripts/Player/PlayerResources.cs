using System.Collections.Generic;
using Game.Resources;

namespace Game.Player
{
    public class PlayerResources
    {
        public PlayerResources()
        {
            Resources = new Dictionary<string, Resource>();
        }
        
        /// <summary>
        /// Ресурсы игрока.
        /// </summary>
        public Dictionary<string, Resource> Resources { get; set; }
        
    }
}