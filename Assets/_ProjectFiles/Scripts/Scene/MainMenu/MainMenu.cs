using System;
using System.Collections.Generic;
using System.Linq;
using Egsp.Core;
using Game.Scenes;
using Game.Stories;
using Game.Ui.MainMenu;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game
{
    public interface IMainMenu
    {
        void ShowMenu();
        void ContinueGame();
        void SetupNewGame();
        void StartNewGame(IStoryInfo storyInfo);
        void OpenSettings();
        void Exit();
    }

    public class MainMenu : SerializedMonoBehaviour, IMainMenu, IEventContextEntity
    {
        [OdinSerialize] private List<IStoryInfo> _storyInfos;
        [OdinSerialize] private IEventContext _eventContext;
        [SerializeField] private string gameplayScene;
        
        private EventBus _eventBus;
        
        private void Awake()
        {
            // Т.к. мы не в контексте, то сами подпишемся на все.
            SetEventBus(_eventContext.Bus);
            _eventBus.Subscribe<IMainMenu>(this);
            _eventContext.SetupContextToEntities();
        }

        private void Start()
        {
            CanGameBeContinued();
        }

        private void CanGameBeContinued()
        {
            if (!GameProgress.IsAnyStoryStarted)
            {
                _eventBus.Raise<IMenuActionsController>(x=>
                    x.ContinueInteractable = false);
            }
        }

        public void ContinueGame()
        {
            throw new NotImplementedException();            
        }

        public void SetupNewGame()
        {
            _eventBus.Raise<IMenuActionsController>(x => 
                x.Disable());
            _eventBus.Raise<IStoryActionsController>(x =>
            {
                x.Enable();
                x.PreviewStories(_storyInfos);
            });
        }

        public void StartNewGame(IStoryInfo storyInfo)
        {
            if (string.IsNullOrWhiteSpace(gameplayScene))
            {
                Debug.LogWarning("Incorrect scene name in start new game!");
                return;
            }
            
            var @params = new GameStartParams(storyInfo);

            GameSceneManager.Instance.LoadSceneAdditive(gameplayScene, true, @params);
            HideMenu();
        }

        public void ShowMenu()
        {
            _eventBus.Raise<IMenuActionsController>(x=>
                x.Enable());
        }
        
        public void HideMenu()
        {
            _eventBus.Raise<IMenuActionsController>(x=>
                x.Disable());

            _eventBus.Raise<IStoryActionsController>(x =>
                x.Disable());
        }

        public void OpenSettings()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void SetEventBus(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void AfterContextSetup()
        {
        }
    }
}