using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.World
{
    public class TownManager : SerializedMonoBehaviour, IMapListener
    {
        
        [BoxGroup("Map")][Tooltip("Минимальное расстояние между городами.")]
        [SerializeField] private float minimumDistance;
        [BoxGroup("Map")][Tooltip("Основное положение активного города.")]
        [SerializeField] public Vector3 mainPosition;

        [SerializeField] private int townCount;
        [Tooltip("Количество посещенных городов, которые остаются в игровом мире.")]
        [SerializeField] private int visitedTownsCount;
        [SerializeField] private ITown townPrefab;
        
        private LinkedList<ITown> _townList = new LinkedList<ITown>();
        

        /// <summary>
        /// Возвращает следующий город после активного.
        /// </summary>
        public ITown NextTown
        {
            get
            {
                if (ActiveTown == null)
                    return null;

                var currentTownNode = _townList.Find(ActiveTown);
                // Если данный город не в списке.
                if (currentTownNode == null)
                    throw new NullReferenceException();

                if (currentTownNode.Next == null)
                    return null;

                return currentTownNode.Next.Value;
            }
        }
        
        public ITown ActiveTown { get=> _activeTown; private set => _activeTown = value; }
        // Город, к которому движемся или находимся в нем.
        private ITown _activeTown;
        
        // Направление спавна городов.
        private Vector3 SpawnDirection => Vector3.up;

        [Button("Spawn towns")]
        public void SpawnTowns(int count)
        {
            _townList.Clear();
            for (var i = 0;  i< count; i++)
            {
                SpawnTown();
            }
        }

        /// <summary>
        /// Спавн города. Город заносится в список.
        /// </summary>
        private ITown SpawnTown()
        {
            var townLastNode = _townList.Last;
            
            var townInst = Instantiate(townPrefab as MonoBehaviour);
            townInst.transform.SetParent(transform,true);
            
            var newTown = (ITown) townInst;

            if (townLastNode != null)
            {
                // Если по какой-то причине значение null.
                if (townLastNode.Value == null)
                {
                    townLastNode.Value = newTown;
                    newTown.Position = mainPosition;

                    return newTown;
                }
                else // Нормальное поведение.
                {
                    newTown.Position = townLastNode.Value.Position + SpawnDirection * minimumDistance;
                }
            }
            else // Если самый первый город.
            {
                newTown.Position = mainPosition;
            }
            
            _townList.AddLast(newTown);

            return newTown;
        }

        /// <summary>
        /// Устнавливает новый активный город и возвращает старый или null.
        /// </summary>
        private ITown SetActiveTown(ITown newActiveTown)
        {

            if (ActiveTown == null)
            {
                ActiveTown = newActiveTown;
                return null;
            }
            else
            {
                var oldActiveTown = ActiveTown;
                ActiveTown = newActiveTown;
                return oldActiveTown;
            }
        }

        /// <summary>
        /// Меняет активный город на следующий.
        /// </summary>
        public bool Next()
        {
            // Если нет активного города н аданный момент,
            // то сначала нужно получить его.
            if (ActiveTown == null)
            {
                if (_townList != null)
                {
                    if (_townList.Count > 0)
                    {
                        SetActiveTown(_townList.First.Value);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            
            // Следующий город после активного.
            if (NextTown != null)
            {
                SetActiveTown(NextTown);
                return true;
            }

            return false;
        }
        
        public void MapUpdate(Vector3 addOffset)
        {
            foreach (var town in _townList)
            {
                town.Position += addOffset;
            }
        }
    }
}