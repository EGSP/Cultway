using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game.World
{
    public class TownManager : SerializedMonoBehaviour, IMapListener
    {
        [BoxGroup("Map")][Tooltip("Минимальное расстояние между городами.")]
        [SerializeField] private float minimumDistance;
        [BoxGroup("Map")][Tooltip("Основное положение активного города.")]
        [SerializeField] public Vector3 mainPosition;

        [BoxGroup("Spawn")]
        [SerializeField] private int townCount;
        [BoxGroup("Spawn")][Tooltip("Количество посещенных городов, которые остаются в игровом мире.")]
        [SerializeField] private int visitedTownsCount;
        [BoxGroup("Spawn")]
        [SerializeField] private ITown townPrefab;
        
        private LinkedList<ITown> _townList = new LinkedList<ITown>();

        [BoxGroup("Spawn")]
        [OdinSerialize] public bool AutoSpawn { get; private set; } = true;
        [BoxGroup("Spawn")]
        [OdinSerialize] public bool AutoDestroy { get; private set; } = true;
        
        
        /// <summary>
        /// Текущий город.
        /// </summary>
        public ITown CurrentTown { get; private set; }
        
        // Направление спавна городов.
        private Vector3 SpawnDirection => Vector3.up;

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
        /// Меняет активный город на следующий.
        /// </summary>
        public bool Next()
        {
            var haveNext = NextCurrent();

            if (haveNext == false)
                return false;

            if(AutoDestroy)
                DestroyWasteTowns();

            if (AutoSpawn)
                SpawnTown();

            return haveNext;
        }

        private bool NextCurrent()
        {
            if (_townList == null || _townList.Count == 0)
            {
                return false;
            }

            // Переход на самый первый город.
            if (CurrentTown == null)
            {
                CurrentTown = _townList.First.Value;
                return true;
            }
            else
            {
                var curNode = _townList.Find(CurrentTown);
                // Город не находится в списке.
                if (curNode == null)
                {
                    CurrentTown = _townList.First.Value;
                    return true;
                }
                else
                {
                    var nextNode = curNode.Next;
                    // Больше городов нет.
                    if (nextNode == null)
                        return false;

                    CurrentTown = nextNode.Value;
                    return true;
                }
            }
        }

        private void DestroyWasteTowns()
        {
            var visited = GetVisitedTowns(_townList);
            var wasted = GetWasteTowns(visited);
            
            foreach (var wastedTown in wasted)
            {
                _townList.Remove(wastedTown);

                var mono = wastedTown as MonoBehaviour;
                if(mono == null)
                    continue;
                
                DestroyImmediate(mono.gameObject);
            }
        }

        /// <summary>
        /// Возвращает очередь посещенных городов.
        /// Первый город - самый давний.
        /// </summary>
        private List<ITown> GetVisitedTowns(IEnumerable<ITown> towns)
        {
            var list = new List<ITown>();
            if (towns == null)
                return list;

            foreach (var town in towns)
            {
                if(town.Visited)
                    list.Add(town);
            }

            return list;
        }

        /// <summary>
        /// Предполагается, что коллекция состоит из посещенных городов.
        /// Лишние города будут возвращены отдельной очередью.
        /// </summary>
        private Queue<ITown> GetWasteTowns(ICollection<ITown> towns)
        {
            var queue = new Queue<ITown>();
            if (towns == null)
                return queue;

            // Если нет лишних городов.
            if (towns.Count <= visitedTownsCount)
                return queue;

            var wasteCount = towns.Count - visitedTownsCount;

            foreach (var town in towns)
            {
                if (wasteCount == 0)
                    return queue;
                    
                if (town.Visited)
                {
                    queue.Enqueue(town);
                    wasteCount--;
                }
            }

            return queue;
        }

        private void RemoveTownFromList(IList<ITown> list,ITown town)
        {
            if (_townList != null)
            {
                _townList.Remove(town);
            }
        }
        
        public void MapUpdate(Vector3 addOffset)
        {
            foreach (var town in _townList)
            {
                town.Position += addOffset;
            }
        }
        
        // HELPER FUNCTIONS -----------------------------------------
        
        [Button("Spawn towns")]
        public void SpawnTowns(int count)
        {
            for (var i = 0;  i< count; i++)
            {
                SpawnTown();
            }
        }
    }
}