using System;
using System.Collections;
using System.Collections.Generic;
using Egsp.Core.Ui;
using Game.Cards;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui
{
    public interface ICardVisual : IVisual<ICardVisual>
    {
        event Action OnShowActions;
        event Action OnHideActions;

        void Accept(CardInfo cardInfo);
        void CallShowActionsEvent();
        void CallHideActionsEvent();
    }

    public class CardVisual : SerializedVisual<ICardVisual>, ICardVisual
    {
        [OdinSerialize] public Image CardSprite { get; private set; }

        [OdinSerialize] public TMP_Text CardName { get; private set; }
        [OdinSerialize] public TMP_Text CardDescription { get; private set; }

        /// <summary>
        /// Карточка связанная с визуальным элементом.
        /// </summary>
        public CardInfo CardInfo { get; private set; }

        public event Action OnShowActions = delegate { };
        public event Action OnHideActions = delegate { };

        public void Accept(CardInfo cardInfo)
        {
            CardSprite.sprite = cardInfo.Sprite;
            CardName.text = cardInfo.Name;
            CardDescription.text = cardInfo.Description;
        }

        public void CallShowActionsEvent()
        {
            OnShowActions();
        }

        public void CallHideActionsEvent()
        {
            OnHideActions();
        }
    }
}