using System;
using System.Collections.Generic;
using System.Linq;
using Egsp.Core;
using Game.Stories;
using Game.Ui.MainMenu;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game
{
    public interface IMainMenu
    {
        void BackToMenu();
        void ContinueGame();
        void StartNewGame();
        void OpenSettings();
        void Exit();
    }

    public class MainMenu : SerializedMonoBehaviour, IMainMenu, IEventContextEntity
    {
        [OdinSerialize] private List<IStoryInfo> _storyInfos;
        [OdinSerialize] private IEventContext _eventContext;
        
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

        public void BackToMenu()
        {
            _eventBus.Raise<IMenuActionsController>(x=>
                x.Enable());
        }

        public void ContinueGame()
        {
            throw new NotImplementedException();            
        }

        public void StartNewGame()
        {
            Debug.Log("Start new game.");
            _eventBus.Raise<IMenuActionsController>(x => 
                x.Disable());
            _eventBus.Raise<IStoryActionsController>(x =>
            {
                Debug.Log("story actions");
                x.Enable();
                x.PreviewStories(_storyInfos);
            });
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