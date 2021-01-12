using System;
using System.Collections;
using System.Collections.Generic;
using Egsp.Core.Ui;
using Game.Stories;
using Game.Ui.Stories;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui.MainMenu
{
    public interface IStoryActionsController : IVisual<IStoryActionsController>
    {
        void PreviewStories([NotNull]IEnumerable<IStoryInfo> stories);
    }
    
    public class StoryActionsController : MainMenuController<IStoryActionsController>, IStoryActionsController
    {
        [SerializeField] private TransformContainer container;
        [SerializeField] private IStoryInfoVisual _storyInfoPrefab;
        [SerializeField] private Button startNewGameButton;

        [CanBeNull] private IStoryInfo _selectedStory;

        protected override void Awake()
        {
            base.Awake();
            CanGameBeStarted();
        }

        private void CanGameBeStarted()
        {
            if (_selectedStory == null)
            {
                startNewGameButton.interactable = false;
            }
            else
            {
                startNewGameButton.interactable = true;
            }
        }
        
        public void Back()
        {
            _selectedStory = null;
            CanGameBeStarted();
            
            ContextBus?.Raise<IMainMenu>(x=>{
                Disable();
                x.ShowMenu();
            });
        }

        public void StartNewGame()
        {
            if(_selectedStory == null)
                Debug.LogWarning("Попытка начать игру без выбранной истории.");
            
            ContextBus?.Raise<IMainMenu>(x=>x.StartNewGame(_selectedStory));
        }

        public void PreviewStories(IEnumerable<IStoryInfo> stories)
        {
            if(stories == null)
                throw new NullReferenceException();
            
            if(_storyInfoPrefab == null)
                throw new NullReferenceException();
            
            container.Clear();
            
            foreach (var info in stories)
            {
                var inst = container.Put(_storyInfoPrefab);
                inst.Accept(info);
                ListenStory(inst);
            }
        }

        private void ListenStory( IStoryInfoVisual storyInfoVisual)
        {
            storyInfoVisual.OnClick += OnStoryClicked;
        }

        private void OnStoryClicked(IStoryInfoVisual storyInfoVisual)
        {
            _selectedStory = storyInfoVisual.StoryInfo;
            CanGameBeStarted();
        }
    }
}