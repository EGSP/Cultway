using System;
using UnityEngine;

namespace Game.World
{
    public class Town : MonoBehaviour, ITown
    {
        private bool _visited;
        public event Action<ITown> OnVisited = delegate(ITown town) {  };

        public bool Visited
        {
            get => _visited;
            set
            {
                _visited = value;
                OnVisited(this);
            } 
        }

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
    }
}