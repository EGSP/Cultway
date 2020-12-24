using System;
using System.Collections;
using System.Collections.Generic;
using Egsp.Core.Ui;
using Game.Resources;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui
{
    public interface IResourceVisual : IVisual<IResourceVisual>
    {
        void Accept(Resource resource);
    }

    public class ResourceVisual : SerializedVisual<IResourceVisual>, IResourceVisual
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text count;

        private Resource _resource;
        
        public void Accept(Resource resource)
        {
            AcceptResource(resource);
            ListenResource(resource);
        }

        [Button("Accept random. (Not a main method).")]
        private void Accept(ResourceInfo resourceInfo)
        {
            Accept(Resource.CreateRandom(resourceInfo));
        }

        private void AcceptResource(Resource resource)
        {
            icon.sprite = resource.Icon;
            count.text = resource.Count.ToString();

            _resource = resource;
        }

        private void ListenResource(Resource resource)
        {
            resource.NotifyChanged += AcceptResource;
        }

        private void OnDestroy()
        {
            if (_resource != null)
                _resource.NotifyChanged -= AcceptResource;
        }
    }
}