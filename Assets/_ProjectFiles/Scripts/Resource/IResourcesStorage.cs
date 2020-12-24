using System;
using System.Collections.Generic;

namespace Game.Resources
{
    public interface IResourcesStorage
    {
        Dictionary<string, Resource> Resources { get; }

        void ExtractResources(IEnumerable<ResourceInfo> resourceInfos);

        Resource GetResourceByInfo(ResourceInfo resourceInfo);
    }

    public static class ResourceStorageExtensions
    {
        /// <summary>
        /// Создания словаря ресурсов на основе объекта информации.
        /// </summary>
        public static Dictionary<string, Resource> GetResourcesDictionary(
            this IResourcesStorage storage, IEnumerable<ResourceInfo> resourceInfos)
        {
            var dict = new Dictionary<string, Resource>();
            foreach (var info in resourceInfos)
            {
                if (!dict.ContainsKey(info.Name))
                    dict.Add(info.Name, new Resource(info));
            }

            return dict;
        }

        public static Resource GetResource(this IResourcesStorage storage, ResourceInfo resourceInfo)
        {
            if (storage.Resources.ContainsKey(resourceInfo.Name))
            {
                return storage.Resources[resourceInfo.Name];
            }

            return null;
        }
    }
}