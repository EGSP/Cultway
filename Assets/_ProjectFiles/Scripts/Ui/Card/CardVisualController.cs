using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Ui
{
    public class CardVisualController : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Vector2 defaultPoistion;
        [SerializeField] private Vector2 selectedPosition;
        [SerializeField] private Vector2 defaultScale;
        [SerializeField] private Vector2 selectedScale;
        [SerializeField] private float animDuration;

        private ICardVisual _cardVisual;

        private bool _clicked;
        private bool _isAnimating;

        private void Awake()
        {
            _cardVisual = GetComponent<ICardVisual>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Clicked on card!");

            var cardVisual = _cardVisual as MonoBehaviour;

            if (cardVisual == null)
                return;

            if (_clicked == false)
            {
                if (_isAnimating)
                    return;

                _isAnimating = true;
                _clicked = true;
                cardVisual.transform.DOLocalMove(selectedPosition, animDuration)
                    .OnComplete(() => _isAnimating = false);
                cardVisual.transform.DOScale(selectedScale, animDuration);

                ShowCardActions();
            }
            else
            {
                if (_isAnimating)
                    return;

                _isAnimating = true;
                _clicked = false;
                cardVisual.transform.DOLocalMove(defaultPoistion, animDuration)
                    .OnComplete(() => _isAnimating = false);
                ;
                cardVisual.transform.DOScale(defaultScale, animDuration);

                HideCardActions();
            }
        }

        private void ShowCardActions()
        {
            _cardVisual.CallShowActionsEvent();
        }

        private void HideCardActions()
        {
            _cardVisual.CallHideActionsEvent();
        }
    }
}