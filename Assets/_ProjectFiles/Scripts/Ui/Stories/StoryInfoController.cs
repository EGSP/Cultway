using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Ui.Stories
{
    [RequireComponent(typeof(IStoryInfoVisual))]
    public class StoryInfoController : MonoBehaviour, IPointerDownHandler
    {
        private IStoryInfoVisual _storyInfoVisual;
        private bool _toogleState;
        
        private void Awake()
        {
            _storyInfoVisual = GetComponent<IStoryInfoVisual>();
            if(_storyInfoVisual == null)
                throw new NullReferenceException();

            _toogleState = false;
            _storyInfoVisual.ToogleDescription(_toogleState);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if(_storyInfoVisual == null)
                throw new NullReferenceException();

            if (_storyInfoVisual.InAnimation)
                return;
            
            _toogleState = !_toogleState;
            _storyInfoVisual.ToogleDescription(_toogleState);
            _storyInfoVisual.Click();
        }
    }
}