using System;
using System.Collections.Generic;
using Game.Resources;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game.Cards
{
    public interface ICardAction
    {
        [CanBeNull]
        ICardBehaviour NewCardBehaviour { get; }

        [OdinSerialize]
        CardOperationGroup OperationGroup { get; }
        
        string GetDecription();
        
        bool Precodnition(IResourcesStorage storage);

        /// <summary>
        /// Совершение операции.
        /// </summary>
        void Invoke(IResourcesStorage storage);
    }

    [Serializable]
    public class CardAction : ICardAction
    {
        [OdinSerialize][PropertyOrder(3)]
        public ICardBehaviour NewCardBehaviour { get; private set; }

        /// <summary>
        /// Группа операций с разными ресурсами для текущего действия.
        /// </summary>
        [OdinSerialize][PropertyOrder(1)][CanBeNull]
        public CardOperationGroup OperationGroup { get; private set; }

        [TextArea][PropertyOrder(2)]
        public string description;

        public string GetDecription()
        {
            return description;
        }

        public bool Precodnition(IResourcesStorage storage)
        {
            if (OperationGroup == null || OperationGroup.resourceOperationBindings == null)
            {
                Debug.LogWarning("В карте не определены операции!");
                return false;   
            }

            // Проходимся по всем операциям.
            for (var i = 0; i < OperationGroup.resourceOperationBindings.Count; i++)
            {
                var operationBinding = OperationGroup.resourceOperationBindings[i];
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
            if (OperationGroup != null)
            {
                OperationGroup.InvokeOperationsAll(storage);
            }
            else
            {
                Debug.LogWarning("Invoked action with null operation group!");
            }
        }

        /// <summary>
        /// Извлекает из действий уникальный список ресурсов.
        /// </summary>
        public static List<ResourceInfo> UniqueResourceInfos(IEnumerable<ICardAction> cardActions)
        {
            var list = new List<ResourceInfo>();
            // Проходимся по всем действиям и добавляем уникальные ресурсы.
            foreach (var cardAction in cardActions)
            {
                if(cardAction.OperationGroup == null)
                    continue;
                
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