using Egsp.Core;
using Game.Cards;
using Game.Resources;
using Game.Ui;
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
            _storage.InitResourcesByInfo(CardAction.UniqueResourceInfos(cardInfo.GetActions()));
            
            cardRootController.AcceptCard(CardVisualFactory.Instance, cardInfo, _storage);
            resourceStorageController.Accept(_storage);
        }
    }
}