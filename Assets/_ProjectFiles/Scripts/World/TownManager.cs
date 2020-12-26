using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.World
{
    public class TownManager : SerializedMonoBehaviour, IMapListener
    {
        [Tooltip("Минимальное расстояние между городами.")]
        [SerializeField] private float minimumDistance;
        [Tooltip("Основное положение активного города.")]
        [SerializeField] private Vector3 mainPosition;

        [SerializeField] private int townCount;
        [SerializeField] private ITown townPrefab;
        
        // Направление спавна городов.
        private Vector3 SpawnDirection => Vector3.up;

        private LinkedList<ITown> _townList = new LinkedList<ITown>();

        private void SpawnTown()
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
                    return;
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
        }

        [Button("Spawn towns")]
        private void SpawnTowns(int count)
        {
            _townList.Clear();
            for (var i = 0;  i< count; i++)
            {
                SpawnTown();
            }
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