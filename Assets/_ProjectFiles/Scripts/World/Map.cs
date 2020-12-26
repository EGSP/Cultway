using System.Collections.Generic;
using DG.Tweening;
using Egsp.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.World
{
    public interface IMapListener
    {
        void MapUpdate(Vector3 addOffset);
    }
    
    public class Map : SerializedMonoBehaviour
    {
        [SerializeField] private List<IMapListener> _mapListeners;

        /// <summary>
        /// Смещение происходит относительно направления движения кого-либо объекта.
        /// Скорость умножается на deltaTime.
        /// </summary>
        [Button("Move map")]
        public void MoveRelated(Vector3 relatedDirection, float speed)
        {
            Move(-relatedDirection, speed);
        }

        /// <summary>
        /// Мир сдвигается по направлению.
        /// Скорость умножается на deltaTime.
        /// </summary>
        public void Move(Vector3 direction, float speed)
        {
            var tempGameobject = new GameObject("TempGameobject");
            tempGameobject.transform.SetParent(transform);
            tempGameobject.transform.position = Vector3.zero;

            // Передвигаем виртуальный объект и обновляем позиции слушателей.
            var tween = tempGameobject.transform.DOLocalMove(direction, 1/speed)
                .OnUpdate(()=>UpdateMapListeneres(tempGameobject.transform.localPosition))
                .OnComplete(()=> DestroyImmediate(tempGameobject));

            _lastPosition = tempGameobject.transform.localPosition;
        }

        private Vector3 _lastPosition;
        private void UpdateMapListeneres(Vector3 newPosition)
        {
            if (_mapListeners == null)
                return;

            foreach (var listener in _mapListeners)
            {
                listener.MapUpdate(newPosition - _lastPosition);
            }

            _lastPosition = newPosition;
        }
    }
}