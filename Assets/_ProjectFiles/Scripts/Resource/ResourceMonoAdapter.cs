using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Game.Resources
{
    public class ResourceMonoAdapter : SerializedMonoBehaviour, IResourcesStorage
    {
        [OdinSerialize]
        private List<ResourceInfo> _resourceInfos;

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
        
        public void ExtractResources(IEnumerable<ResourceInfo> resourceInfos)
        {
            Resources = this.GetResourcesDictionary(resourceInfos);
        }

        public Resource GetResourceByInfo(ResourceInfo resourceInfo)
        {
            return this.GetResource(resourceInfo);
        }

        [Button("Convert info to resources")]
        private void ConvertInfoToResources()
        {
            if(Resources == null)
                Resources = new Dictionary<string, Resource>();
            
            Resources = this.GetResourcesDictionary(_resourceInfos);
        }
    }
}