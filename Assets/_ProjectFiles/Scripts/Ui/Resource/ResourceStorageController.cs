using System;
using System.Runtime.InteropServices;
using DG.Tweening;
using Game.Resources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Ui
{
    public interface IResourceStorageController
    {
        void Accept(IResourcesStorage storage);

        void Activate();
        void Deactivate();
    }
    
    public class ResourceStorageController : SerializedMonoBehaviour, IResourceStorageController
    {
        [BoxGroup("Visual")]
        [SerializeField] private TransformContainer resourcesContainer;
        [BoxGroup("Visual")]
        [SerializeField] private IResourceVisual resourceVisualPrefab;
        
        [BoxGroup("Animation")]
        [SerializeField] private Vector2 defaultPosition;
        [BoxGroup("Animation")]
        [SerializeField] private Vector2 activePosition;
        [BoxGroup("Animation")]
        [SerializeField] private float animDuration;

        private void Awake()
        {
            Deactivate();
        }

        public void Accept(IResourcesStorage storage)
        {
            if(storage == null)
                throw new NullReferenceException();
            
            resourcesContainer.Clear();
            foreach (var resource in storage.Resources.Values)
            {
                var inst = resourcesContainer.Put<IResourceVisual>(resourceVisualPrefab);
                inst.Accept(resource);
            }
        }

        public void Activate()
        {
            resourcesContainer.transform.DOLocalMove(activePosition, animDuration);
        }

        public void Deactivate()
        {
            resourcesContainer.transform.DOLocalMove(defaultPosition, animDuration);
        }

        private void Clear()
        {
            resourcesContainer.Clear();
        }
    }
}