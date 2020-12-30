using System;
using System.Linq;
using Egsp.Utils.MeshUtilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.World
{
    public class RoadDrawer : SerializedMonoBehaviour
    {
        [SerializeField] private Material roadMaterial;
        [SerializeField] private float width;
        [SerializeField] private ITownProvider _townProvider;

        private void LateUpdate()
        {
            if (_townProvider != null)
                DrawRoad(_townProvider);
        }

        private void DrawRoad(ITownProvider _townProvider)
        {
            var towns = _townProvider.Towns;

            if (towns == null)
                return;

            // Нечего рисовать.
            if (towns.Count == 0 || towns.Count == 1)
                return;

            // Получаем итератор и сдвигаем его на первый элемент.
            var iterator = towns.GetEnumerator();
            iterator.MoveNext();
            ITown lastTown = iterator.Current;

            // Пока не закончатся города.
            while (iterator.MoveNext())
            {
                DrawLine(lastTown.Position, iterator.Current.Position);
                lastTown = iterator.Current;
            }
        }

        private void DrawLine(Vector3 last, Vector3 next)
        {
            MeshUtils.DrawLine(last, next, width, roadMaterial);
        }
    }
}