using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.World
{
    public class TravelManager : SerializedMonoBehaviour
    {
        [BoxGroup("Systems")]
        [SerializeField] private TownManager townManager;
        [BoxGroup("Systems")]
        [SerializeField] private Map map;

        [BoxGroup("Travel")]
        [SerializeField] private float travelTime;

        /// <summary>
        /// Вызывается по прибытию в город.
        /// </summary>
        public event Action<ITown> OnArrival = delegate(ITown town) {  }; 
        
        /// <summary>
        /// Вызывается по отбытию из города.
        /// </summary>
        public event Action OnDeparture = delegate {  };

        private void Awake()
        {
            if(townManager == null)
                throw new NullReferenceException();
            
            if(map == null)
                throw new NullReferenceException();
            
            townManager.SpawnTowns(4);
            SetupMap();
        }

        private void SetupMap()
        {
            map.MainOffset = townManager.mainPosition;
        }

        public void TravelToNextTown()
        {
            if (townManager.Next() == false)
            {
                Debug.LogWarning("Города закончились!");
                return;
            }

            var nextTown = townManager.CurrentTown;
            map.Move(nextTown.Position, travelTime).With(() => OnTravelComplete(nextTown));

            OnDeparture();
        }

        private void OnTravelComplete(ITown town)
        {
            Debug.Log("Прибытие в город.");

            town.Visited = true;
            OnArrival(town);
        }
    }
}






