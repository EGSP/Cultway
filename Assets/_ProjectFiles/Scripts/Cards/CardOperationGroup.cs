using System;
using System.Collections.Generic;
using Game.Resources;

namespace Game.Cards
{
    /// <summary>
    /// Группа операций данной карты.
    /// </summary>
    [Serializable]
    public class CardOperationGroup
    {
        public List<ResourceOperationBinding> resourceOperationBindings;
            
        public CardOperationGroup()
        {
            resourceOperationBindings = new List<ResourceOperationBinding>();
        }

        public void InvokeOperationsAll(IResourcesStorage storage)
        {
            foreach (var resourceOperation in resourceOperationBindings)
            {
                resourceOperation.InvokeOperation(storage);
            }
        }

        /// <summary>
        /// Получение всех видов ресурсов из операций.
        /// </summary>
        public List<ResourceInfo> ResourceInfos()
        {
            if(resourceOperationBindings == null)
                throw new NullReferenceException();
                
            var list = new List<ResourceInfo>();

            foreach (var resourceOperationBinding in resourceOperationBindings)
            {
                if (!list.Contains(resourceOperationBinding.resourceInfo))
                {
                    list.Add(resourceOperationBinding.resourceInfo);
                }
            }

            return list;
        }
    }
    
    /// <summary>
    /// Класс связывает информацию о ресурсе и операцию над ресурсом.
    /// </summary>
    [Serializable]
    public class ResourceOperationBinding
    {
        /// <summary>
        /// Ресурс с которым данная операция работает.
        /// </summary>
        public ResourceInfo resourceInfo;

        /// <summary>
        /// Операция совершаемая этим действием.
        /// </summary>
        public IResourceOperation Operation;

        public void InvokeOperation(IResourcesStorage storage)
        {
            var resource = storage.GetResource(resourceInfo);

            if (resource == null)
                throw new NullReferenceException();

            Operation.Invoke(resource);
        }
    }
}