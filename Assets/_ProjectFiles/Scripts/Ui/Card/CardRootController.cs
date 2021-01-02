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
        
        public event Action AfterAction = delegate {  };
        
        private void Awake()
        {
            actionsContainer?.Clear();
        }

        private void AcceptStorage(IResourcesStorage storage)
        {
            if (_storage == null)
            {
                _storage = storage;
                _resourceStorageController.Accept(storage);
            }
        }

        public void AcceptCard(CardVisualFactory visualFactory, CardInfo cardInfo, IResourcesStorage storage)
        {
            // Создает визуальное отображение ресурсов, если до этого оно ни разу не создавалось.
            AcceptStorage(storage);
            
            _currentCardVisual = visualFactory.CreateCardVisual(cardRoot, cardInfo);
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
                DestroyImmediate(mono.gameObject);
            }

            _currentCardVisual = null;
            
            // Очищаем контейнер и скрываем его и ресурсы.
            actionsContainer.Clear();
            Deactivate();
        }

        private void CreateActions(CardInfo cardInfo, IResourcesStorage storage)
        {
            var actions = cardInfo.GetActions();
            if (actions == null)
                return;
            
            foreach (var cardAction in actions)
            {
                var inst = actionsContainer.Put(actionPrefab);
                inst.Accept(cardAction,storage);
                ListenCardAction(inst, cardInfo);
            }
        }

        // Ожидание нажатия на операцию.
        private void ListenCardAction(ICardActionVisual cardActionVisual, CardInfo owner)
        {
            cardActionVisual.OnClicked += (a)=>ProcessCardAction(a,owner);
        }

        // Обработка операции карточки.
        private void ProcessCardAction(ICardAction cardAction, CardInfo owner)
        {
            LockActions();
            
            cardAction.Invoke(_storage);
            owner.RuntimeBehaviour = cardAction.NewCardBehaviour;
            
            AfterAction();
        }

        // Блокирование всех действий. Пользователь не должен иметь возможности их активировать снова.
        private void LockActions()
        {
            foreach (var actionVisual in actionsContainer.GetEnumerable<ICardActionVisual>())
            {
                actionVisual.Lock();
            }
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