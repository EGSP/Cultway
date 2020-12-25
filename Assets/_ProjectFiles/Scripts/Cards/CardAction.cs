using System;
using System.Collections.Generic;
using Game.Resources;
using UnityEngine;

namespace Game.Cards
{
    [System.Serializable]
    public class CardAction
    {
        /// <summary>
        /// Группа операций данной карты.
        /// </summary>
        public class CardOperationGroup
        {
            public CardOperationGroup()
            {
                ResourceOperationBindings = new List<ResourceOperationBinding>();
            }
            
            public List<ResourceOperationBinding> ResourceOperationBindings;

            public void InvokeOperationsAll(IResourcesStorage storage)
            {
                foreach (var resourceOperation in ResourceOperationBindings)
                {
                    resourceOperation.InvokeOperation(storage);
                }
            }
        }

        /// <summary>
        /// Класс связывает информацию о ресурсе и операцию над ресурсом.
        /// </summary>
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
                
                if(resource == null)
                    throw new NullReferenceException();
                
                Operation.Invoke(resource);
            }
        }

        /// <summary>
        /// Группа операций с разными ресурсами для текущего действия.
        /// </summary>
        public CardOperationGroup OperationGroup;
        
        /// <summary>
        /// Ресурс с которым данная операция работает.
        /// </summary>
        public ResourceInfo resourceInfo;
        
        /// <summary>
        /// Операция совершаемая этим действием.
        /// </summary>
        public IResourceOperation Operation;

        [TextArea]
        public string description;

        public string GetDecription()
        {
            return description + Operation?.GetDescriptionSymbols();
        }

        public bool Precodnition(IResourcesStorage storage)
        {
            if (OperationGroup == null)
            {
                Debug.LogWarning("В карте не определены операции!");
                return false;   
            }

            // Проходимся по всем операциям.
            for (var i = 0; i < OperationGroup.ResourceOperationBindings.Count; i++)
            {
                var operationBinding = OperationGroup.ResourceOperationBindings[i];
                var resource = storage.GetResource(operationBinding.resourceInfo);

                // Если не существует данного ресурса.
                if (resource == null)
                {
                    Debug.LogWarning($"В хранилище нет ресурса {resourceInfo.Name}");
                    return false;
                }

                // Если операция не может быть совершена.
                if (operationBinding.Operation.Precondition(resource) == false)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Совершение операции.
        /// </summary>
        public void Invoke(IResourcesStorage storage)
        {
            OperationGroup.InvokeOperationsAll(storage);
        }
    }
}