﻿using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Game.Cards
{
    public interface ICardBehaviour
    {
        [CanBeNull]
        IEnumerable<ICardAction> GetActions();

        /// <summary>
        /// Получение поведений, которые можно получить из действий.
        /// </summary>
        [CanBeNull]
        List<ICardBehaviour> GetNextBehaviours();

        [NotNull]
        string GetDescription();
    }
    
    /// <summary>
    /// Поведение взаимозаменимо обычной карточкой.
    /// Оно было создано как способ создания нового описания для карточки.
    /// </summary>
    public class CardBehaviour : SerializedScriptableObject, ICardBehaviour
    {
        [OdinSerialize] [TableList] [CanBeNull]
        public List<ICardAction> CardActions { get; private set; }

        public string Description { get; private set; }

        public IEnumerable<ICardAction> GetActions()
        {
            return CardActions;
        }

        public List<ICardBehaviour> GetNextBehaviours()
        {
            if (CardActions == null)
                return new List<ICardBehaviour>();
            
            var list = new List<ICardBehaviour>();
            for (var i = 0; i < CardActions.Count; i++)
            {
                if (CardActions[i].NewCardBehaviour != null)
                {
                    list.Add(CardActions[i].NewCardBehaviour);
                }
            }
            return list;
        }

        public string GetDescription()
        {
            return Description;
        }
    }
}