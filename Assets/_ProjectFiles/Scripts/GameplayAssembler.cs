using System;
using Game.Cards;
using Game.Resources;
using Game.Scenes;
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

        [SerializeField] private bool autoStart;

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
            if (autoStart)
                Travel();
        }

        public void StartGame()
        {
            Travel();
        }

        public void StartGame(SceneParams @params)
        {
            var gp = @params as GameStartParams;

            if (gp != null)
            {
                var story = gp.StoryInfo ??  throw new NullReferenceException();
                var cardset = story.CardSet ?? throw new NullReferenceException();
                
                cardManager.SetCards(cardset);

                StartGame();
            }
            else
            {
                 Debug.LogWarning("Scene loaded without GameStartParams!");
            }
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