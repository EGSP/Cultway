using System.Collections;
using System.Collections.Generic;
using Game.Cards;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public interface ICardVisual
{
    void Accept(CardInfo cardInfo);
}

public class CardVisual : SerializedMonoBehaviour, ICardVisual
{
    [OdinSerialize]
    public Image CardSprite { get; private set; }
    
    [OdinSerialize]
    public TMP_Text CardName { get; private set; }
    [OdinSerialize]
    public TMP_Text CardDescription { get; private set; }

    public void Accept(CardInfo cardInfo)
    {
        CardSprite.sprite = cardInfo.Sprite;
        CardName.text = cardInfo.Name;
        CardDescription.text = cardInfo.Description;
    }
}
