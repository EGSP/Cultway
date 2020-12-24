using System;
using Egsp.Core.Ui;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Resources
{
    public class Resource : INotifyChanged<Resource>
    {
        public readonly ResourceInfo Info;

        public Sprite Icon => Info.Sprite;
        
        /// <summary>
        /// Название ресурса.
        /// </summary>
        public string Name => Info.Name;

        /// <summary>
        /// Количество данного ресурса.
        /// </summary>
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                NotifyChanged(this);
            }
        }
        private int _count;

        public Resource(ResourceInfo info)
        {
            Info = info;
            Count = 0;
        }

        public Resource(ResourceInfo info, int count):this(info)
        {
            Count = count;
        }

        public static Resource CreateRandom(ResourceInfo resourceInfo, int max = 10)
        {
            return new Resource(resourceInfo, Random.Range(0,max+1));
        }

        public event Action<Resource> NotifyChanged = delegate(Resource resource) {  };
    }
}