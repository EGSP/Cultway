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
    }
    
    public class CardActionVisual : SerializedVisual<ICardActionVisual>, ICardActionVisual
    {
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image resourceIcon;
        
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
            // ICON
            resourceIcon.sprite = _action.resourceInfo.Sprite;

            // CLICKABLE
            var button = GetComponent<Button>();
            var resourceInfo = cardAction.resourceInfo;
            // Если имеется подобный ресурс в хранилище.
            if (storage.Resources.ContainsKey(resourceInfo.Name))
            {
                var storageR = storage.Resources[resourceInfo.Name];
                if (cardAction.Operation.Precondition(storageR))
                {
                    button.onClick.AddListener(()=> OnClicked(cardAction));
                }
                // Если операция невозможна.
                else
                {
                    button.interactable = false;
                }
            }
            else
            {
                Debug.LogWarning($"Storage does not contain {resourceInfo.Name}");
                button.interactable = false;
            }
        }
    }
}