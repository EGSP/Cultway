﻿using System;
using DG.Tweening;
using Egsp.Core.Ui;
using Game.Stories;
using JetBrains.Annotations;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui.Stories
{
    public interface IStoryInfoVisual : IVisual<IStoryInfoVisual>
    {
        [CanBeNull] IStoryInfo StoryInfo { get; }
        event Action<IStoryInfoVisual> OnClick;

        void Click();
        
        void Accept([CanBeNull]IStoryInfo storyInfo);

        void ToogleDescription(bool toogleOn);
    }
    
    public class StoryInfoVisual : SerializedVisual<StoryInfoVisual>, IStoryInfoVisual
    {
        [SerializeField] private Image storyIcon;
        [SerializeField] private TMP_Text storyDescription;
        
        [SerializeField] private Vector2 defaultDescriptionPosition;
        [SerializeField] private Vector2 activeDescriptionPosition;
        [SerializeField] private float animDuration;
        
        public IStoryInfo StoryInfo { get; private set; }
        public event Action<IStoryInfoVisual> OnClick = delegate(IStoryInfoVisual visual) {  };

        public void Accept(IStoryInfo storyInfo)
        {
            if (storyInfo == null)
                return;

            StoryInfo = storyInfo;
            
            if (storyInfo.Sprite != null)
                storyIcon.sprite = storyInfo.Sprite;

            if (storyInfo.Description.IsNullOrWhitespace())
            {
                storyDescription.text = "No description!";
            }
            else
            {
                storyDescription.text = storyInfo.Description;
            }

            ToogleDescription(false);
        }

        public void ToogleDescription(bool toogleOn)
        {
            if (toogleOn)
            {
                EnterAnimation();
                storyDescription.gameObject.SetActive(true);
                storyDescription.transform.DOLocalMove(activeDescriptionPosition, animDuration)
                    .OnComplete(ExitAnimation);
            }
            else
            {
                EnterAnimation();
                storyDescription.transform.DOLocalMove(defaultDescriptionPosition, animDuration)
                    .OnComplete(()=>
                    {
                        ExitAnimation();
                        storyDescription.gameObject.SetActive(false);
                    });
            }
        }

        public void Click()
        {
            OnClick(this);
        }
    }
}