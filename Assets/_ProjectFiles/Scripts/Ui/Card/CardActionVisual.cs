using System;
using Egsp.Core.Ui;
using Game.Cards;
using Game.Resources;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui
{
    public interface ICardActionVisual : IVisual<ICardActionVisual>
    {
        event Action<CardAction> OnClicked;
        
        void Accept(CardAction cardAction, IResourcesStorage storage);

        /// <summary>
        /// Не позволяет совершать действия.
        /// </summary>
        void Lock();
    }
    
    public class CardActionVisual : SerializedVisual<ICardActionVisual>, ICardActionVisual
    {
        [SerializeField] private TMP_Text description;

        [SerializeField] private IResourceOperationEffectVisual resourceOperationEffectVisualPrefab;
        [SerializeField] private TransformContainer resourcesIconsContainer;
        
        private CardAction _action;
        private IResourcesStorage _storage;

        public event Action<CardAction> OnClicked = delegate(CardAction action) {  };

        [Button("Accept CardAction")]
        public void Accept(CardAction cardAction, IResourcesStorage storage)
        {
            _action = cardAction;
            _storage = storage;

            // DESCRIPTION
            description.text = _action.GetDecription();

            // CLICKABLE
            var button = GetComponent<Button>();
            if (!_action.Precodnition(storage))
            {
                button.interactable = false;
            }
            else
            {
                button.onClick.AddListener(()=>OnClicked(cardAction));
            }

            // ICONS
            foreach (var resourceOperationBinding in _action.OperationGroup.ResourceOperationBindings)
            {
                var visual = resourcesIconsContainer
                    .Put<IResourceOperationEffectVisual>(resourceOperationEffectVisualPrefab);
                visual.Accept(resourceOperationBinding.resourceInfo, resourceOperationBinding.Operation);
            }
        }

        /// <summary>
        /// Не позволяет совершать действия.
        /// </summary>
        public void Lock()
        {
            var button = GetComponent<Button>();
            button.interactable = false;
        }
    }
}