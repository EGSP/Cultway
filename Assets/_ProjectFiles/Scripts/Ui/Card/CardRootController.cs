using System;
using System.Collections.Generic;
using DG.Tweening;
using Egsp.Utils.GameObjectUtilities;
using Game.Cards;
using Game.Resources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Ui
{
    public class CardRootController : SerializedMonoBehaviour
    {
        [BoxGroup("Card")]
        public Transform cardRoot;
        
        [BoxGroup("Card actions")]
        [SerializeField] private TransformContainer actionsContainer;
        [BoxGroup("Card actions")]
        [SerializeField] private ICardActionVisual actionPrefab;
        
        [BoxGroup("Card actions/Animation")]
        [SerializeField] private Vector2 defaultPosition;
        [BoxGroup("Card actions/Animation")]
        [SerializeField] private Vector2 activePosition;
        [BoxGroup("Card actions/Animation")]
        [SerializeField] private float animDuration;

        [BoxGroup("Resources")]
        [SerializeField] private IResourceStorageController _resourceStorageController;

        private ICardVisual _currentCardVisual;
        private IResourcesStorage _storage;
        
        private void Awake()
        {
            actionsContainer?.Clear();
        }

        public void AcceptCard(CardFactory factory, CardInfo cardInfo, IResourcesStorage storage)
        {
            _storage = storage;
            
            _currentCardVisual = factory.CreateCardVisual(cardRoot, cardInfo);
            _currentCardVisual.OnShowActions += Activate;
            _currentCardVisual.OnHideActions += Deactivate;
            
            CreateActions(cardInfo, storage);
        }

        public void AbortCard()
        {
            _currentCardVisual.OnShowActions -= Activate;
            _currentCardVisual.OnHideActions -= Deactivate;
            
            var mono = _currentCardVisual as MonoBehaviour;
            if (mono != null)
            {
                DestroyImmediate(mono);
            }

            _currentCardVisual = null;
            
            actionsContainer.Clear();
        }

        private void CreateActions(CardInfo cardInfo, IResourcesStorage storage)
        {
            foreach (var cardAction in cardInfo.CardActions)
            {
                var inst = actionsContainer.Put<ICardActionVisual>(actionPrefab);
                inst.Accept(cardAction,storage);
                ListenCardAction(inst);
            }
        }

        private void ListenCardAction(ICardActionVisual cardActionVisual)
        {
            cardActionVisual.OnClicked += ProcessCardAction;
        }

        private void ProcessCardAction(CardAction cardAction)
        {
            cardAction.Invoke(_storage);
        }

        private void Activate()
        {
            actionsContainer.transform.DOLocalMove(activePosition, animDuration);
            
            _resourceStorageController?.Activate();
        }

        private void Deactivate()
        {
            actionsContainer.transform.DOLocalMove(defaultPosition, animDuration);
            
            _resourceStorageController?.Deactivate();
        }
    }
}