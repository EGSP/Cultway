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
            ExtractResources(resourceInfos);
        }
        
        /// <summary>
        /// Ресурсы игрока.
        /// </summary>
        public Dictionary<string, Resource> Resources { get; private set; }

        public void ExtractResources(IEnumerable<ResourceInfo> resourceInfos)
        {
            Resources = this.GetResourcesDictionary(resourceInfos);
        }

        public Resource GetResourceByInfo(ResourceInfo resourceInfo)
        {
            return this.GetResource(resourceInfo);
        }
    }
}