using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Game.Resources
{
    public class ResourceScriptableAdapter : SerializedScriptableObject, IResourcesStorage
    {
        [OdinSerialize]
        private List<ResourceInfo> _resourceInfos = new List<ResourceInfo>();

        
        // Runtime 
        public Dictionary<string, Resource> Resources
        {
            get
            {
                if (_cachedResources == null)
                {
                    _cachedResources = this.GetResourcesDictionary(_resourceInfos);
                }

                return _cachedResources;
            }

            private set => _cachedResources = value;
        } 
        private Dictionary<string, Resource> _cachedResources;
        
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