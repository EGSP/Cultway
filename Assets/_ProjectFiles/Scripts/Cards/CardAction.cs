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

            /// <summary>
            /// Получение всех видов ресурсов из операций.
            /// </summary>
            public List<ResourceInfo> ResourceInfos()
            {
                if(ResourceOperationBindings == null)
                    throw new NullReferenceException();
                
                var list = new List<ResourceInfo>();

                foreach (var resourceOperationBinding in ResourceOperationBindings)
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

        [TextArea]
        public string description;

        public string GetDecription()
        {
            return description;
        }

        public bool Precodnition(IResourcesStorage storage)
        {
            if (OperationGroup == null || OperationGroup.ResourceOperationBindings == null)
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
                    Debug.LogWarning($"В хранилище нет ресурса {operationBinding.resourceInfo.Name}");
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

        public static List<ResourceInfo> UniqueResourceInfos(IEnumerable<CardAction> cardActions)
        {
            var list = new List<ResourceInfo>();
            // Проходимся по всем действиям и добавляем уникальные ресурсы.
            foreach (var cardAction in cardActions)
            {
                var cardList = cardAction.OperationGroup.ResourceInfos();
                for (var i = 0; i < cardList.Count; i++)
                {
                    if(!list.Contains(cardList[i]))
                        list.Add(cardList[i]);
                }
            }

            return list;
        }
    }
}