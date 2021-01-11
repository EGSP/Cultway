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

        protected override void Awake()
        {
            base.Awake();
        }

        public void Back()
        {
            ContextBus?.Raise<IMainMenu>(x=>{
                Disable();
                x.BackToMenu();
            });
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
            }
        }
    }
}