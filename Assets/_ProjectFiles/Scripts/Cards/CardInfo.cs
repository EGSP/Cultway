
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game.Cards
{
    [InlineEditor()]
    public class CardInfo : SerializedScriptableObject, ICardBehaviour
    {
        [OdinSerialize][PreviewField]
        public Sprite Sprite { get; private set; }
        
        [OdinSerialize]
        public string Name { get; private set; }
        
        [OdinSerialize][MultiLineProperty(2)]
        public string Description { get; private set; }
        
        /// <summary>
        /// Действия которые может совершить карта.
        /// </summary>
        [OdinSerialize][TableList(AlwaysExpanded = true)][CanBeNull]
        public List<ICardAction> CardActions { get; private set; }

        public override string ToString()
        {
            return Name == null ? "nullName" : Name;
        }
        
        
        // Runtime ---------
        /// <summary>
        /// Текущее поведение карточки. По умолчанию ссылается на CardInfo as ICardBehaviour.
        /// </summary>
        [NotNull]
        public ICardBehaviour RuntimeBehaviour
        {
            get
            {
                // Если нет поведения, то используем базовое.
                if (_runtimeBehaviour == null)
                    _runtimeBehaviour = this;

                return _runtimeBehaviour;
            }
            set
            {
                // Если у переданного состояния не будет продолжения, то остается текущее состояние.
                if (_runtimeBehaviour != null)
                    _runtimeBehaviour = value;
            }
        }

        [CanBeNull]
        private ICardBehaviour _runtimeBehaviour;

        public IEnumerable<ICardAction> GetActions()
        {
            if (IsRuntimeBehaviour())
                return CardActions;

            return RuntimeBehaviour.GetActions();
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
            if(IsRuntimeBehaviour())
                return Description ?? "Null description";
            
            return RuntimeBehaviour.GetDescription();
        }

        private bool IsRuntimeBehaviour()
        {
            if (ReferenceEquals(this, RuntimeBehaviour))
                return true;

            return false;
        }
    }
}