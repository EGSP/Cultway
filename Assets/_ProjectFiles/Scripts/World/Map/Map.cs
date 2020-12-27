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

        public Vector3 MainOffset { get; set; }
        
        /// <summary>
        /// Мир сдвигается относительно конечной точки в обратном направлении.
        /// </summary>
        public CallBack Move(Vector3 endPoint, float time)
        {
            var tempGameobject = new GameObject("TempGameobject");
            tempGameobject.transform.SetParent(transform);
            tempGameobject.transform.localPosition = Vector3.zero-MainOffset;
            
            var callback = new CallBack();
            callback.With(() => DestroyImmediate(tempGameobject));
            
            // Передвигаем виртуальный объект и обновляем позиции слушателей.
            var tween = tempGameobject.transform.DOLocalMove(-endPoint, time)
                .OnUpdate(()=>UpdateMapListeneres(tempGameobject.transform.localPosition))
                .OnComplete(callback.On);

            _lastPosition = tempGameobject.transform.localPosition;

            return callback;
        }

        private Vector3 _lastPosition;
        private void UpdateMapListeneres(Vector3 newPosition)
        {
            if (_mapListeners == null)
                return;

            // Сдвигаем всех подписчиков по координатам.
            foreach (var listener in _mapListeners)
            {
                listener.MapUpdate(newPosition - _lastPosition);
            }

            _lastPosition = newPosition;
        }
    }
}