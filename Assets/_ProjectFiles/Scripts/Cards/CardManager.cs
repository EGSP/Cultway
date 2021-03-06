﻿using System;
using DG.Tweening;
using Egsp.RandomTools;
using Game.Resources;
using Game.Ui;
using JetBrains.Annotations;
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

        [SerializeField] private bool throwNotCriticalExceptions;

        public event Action AfterCardHide = delegate {  };

        private void Awake()
        {
            if (cardRootController == null || visualFactory == null)
                throw new NullReferenceException();
            
            if(throwNotCriticalExceptions == true)
                if (_cardSet == null)
                    throw new NullReferenceException();

            cardRootController.AfterAction += ListenCardAction;
        }

        public void SetCards([NotNull]ICardSet cardSet)
        {
            if(cardSet == null)
                throw new ArgumentNullException();

            _cardSet = cardSet;
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
        
        
        
        
        
        
        
        
        
        
        private void TestWeightedList(ICardSet cardSet)
        {
            var weightedList = WeightedList<CardInfo>.FromList(cardSet);
            weightedList.Step = 10f;
            weightedList.SetBalancer(new ThrowOverBalancer<CardInfo>(weightedList));

            for (int i = 0; i < 100; i++)
            {
                Debug.Log(weightedList.Pick().Value.Name);
            }
            
            Debug.Log("-------------------------------------------------");

            for (int i = 0; i < weightedList.Count; i++)
            {
                Debug.Log(weightedList[i].ToString());
            }
            
            Debug.Log(weightedList.WeightBalancer.Name);
        }
    }
}