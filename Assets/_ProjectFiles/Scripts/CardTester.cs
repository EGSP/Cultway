using System;
using System.Collections.Generic;
using System.Linq;
using Egsp.Core;
using Game.Cards;
using Game.Player;
using Game.Resources;
using Game.Services;
using Game.Ui;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game
{
    public class CardTester : SerializedSingleton<CardTester>
    {
        [SerializeField] private CardInfo cardInfo;
        [SerializeField] private CardRootController cardRootController;
        [SerializeField] private ResourceStorageController resourceStorageController;

        [OdinSerialize] private IResourcesStorage _storage;

        protected void Start()
        {
            _storage.InitResourcesByInfo(cardInfo.CardActions.Select(x=>x.resourceInfo));
            
            cardRootController.AcceptCard(CardFactory.Instance, cardInfo, _storage);
            resourceStorageController.Accept(_storage);
        }
    }
}