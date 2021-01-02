using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Game.Cards
{
    public interface ICardBehaviour
    {
        [CanBeNull]
        IEnumerable<ICardAction> GetActions();

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

        public string GetDescription()
        {
            return Description;
        }
    }
}