using System.Collections.Generic;
using Game.Resources;

namespace Game.Player
{
    public class PlayerResources: IResourcesStorage
    {
        public PlayerResources()
        {
            Resources = new Dictionary<string, Resource>();
        }

        public PlayerResources(IEnumerable<ResourceInfo> resourceInfos):this()
        {
            InitResourcesByInfo(resourceInfos);
        }
        
        /// <summary>
        /// Ресурсы игрока.
        /// </summary>
        public Dictionary<string, Resource> Resources { get; private set; }

        public Resource GetResource(ResourceInfo resourceInfo)
        {
            return this.GetResourceFromDictionary(resourceInfo);
        }

        public void InitResourcesByInfo(IEnumerable<ResourceInfo> resourceInfos)
        {
            Resources = this.GetResourcesDictionary(resourceInfos);
        }
    }
}