﻿using System;
using System.Collections;
using DG.Tweening;
using Game.Resources;
using Game.Ui;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Cards
{
    public class CardManager : SerializedMonoBehaviour
    {
        [SerializeField] private CardRootController cardRootController;
        [SerializeField] private CardVisualFactory visualFactory;

        [SerializeField] private ICardSet _cardSet;

        [SerializeField] private float hideDelay;

        public event Action AfterCardHide = delegate {  };

        private void Awake()
        {
            if (cardRootController == null || visualFactory == null)
                throw new NullReferenceException();
            
            if(_cardSet == null)
                throw new NullReferenceException();

            cardRootController.AfterAction += ListenCardAction;
        }

        private void ListenCardAction()
        {
            Debug.Log("ListenCardAction");
            DOVirtual.DelayedCall(hideDelay, HideCard);
        }

        /// <summary>
        /// Отображает визуально случайную карточку из сета.
        /// </summary>
        public void ShowCardRandom(IResourcesStorage storage)
        {
            // Получение случайной карты.
            var card = _cardSet.GetRandomCard();
            // Визуальное отображение.
            cardRootController.AcceptCard(visualFactory, card, storage);
        }

        private void HideCard()
        {
            Debug.Log("HideCard");
            
            cardRootController.AbortCard();
            AfterCardHide();
        }
    }
}