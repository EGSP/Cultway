using System;
using System.Collections.Generic;

namespace Game.Resources
{
    public interface IResourcesStorage
    {
        Dictionary<string, Resource> Resources { get; }

        /// <summary>
        /// Получение ресурса по ResourceInfo. Null при отсутствии.
        /// </summary>
        Resource GetResource(ResourceInfo resourceInfo);
        
        /// <summary>
        /// Создает экземпляры ресурсов по коллекции ResourceInfo
        /// </summary>
        void InitResourcesByInfo(IEnumerable<ResourceInfo> resourceInfos);
    }

    public static class ResourceStorageExtensions
    {
        /// <summary>
        /// Получение ресурса по ResourceInfo. Null при отсутствии.
        /// </summary>
        public static Resource GetResourceFromDictionary(this IResourcesStorage storage, ResourceInfo resourceInfo)
        {
            if (storage.Resources.ContainsKey(resourceInfo.Name))
            {
                return storage.Resources[resourceInfo.Name];
            }
            return null;
        }
        
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

        public static Resource CreateResource(this IResourcesStorage storage, ResourceInfo resourceInfo)
        {
            if (storage.Resources.ContainsKey(resourceInfo.Name))
            {
                return storage.Resources[resourceInfo.Name];
            }

            return null;
        }
    }
}