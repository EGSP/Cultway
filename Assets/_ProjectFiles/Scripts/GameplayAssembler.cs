using System;
using Game.Cards;
using Game.Resources;
using Game.World;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class GameplayAssembler : SerializedMonoBehaviour
    {
        [SerializeField] private TravelManager travelManager;
        [SerializeField] private CardManager cardManager;

        [SerializeField] private IResourcesStorage _storage;

        private void Awake()
        {
            if(travelManager == null)
                throw new NullReferenceException();
            
            if(cardManager == null)
                throw new NullReferenceException();

            travelManager.OnArrival += OnTownArrival;
            cardManager.AfterCardHide += OnAfterCardHide;
        }

        private void Start()
        {
            Travel();
        }

        private void OnTownArrival(ITown town)
        {
            ShowCard();
        }

        private void ShowCard()
        {
            cardManager.ShowCardRandom(_storage);
        }

        private void OnAfterCardHide()
        {
            Travel();
        }

        private void Travel()
        {
            travelManager.TravelToNextTown();
        }
    }
}