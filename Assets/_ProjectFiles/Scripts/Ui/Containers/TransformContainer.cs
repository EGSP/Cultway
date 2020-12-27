using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Egsp.Utils.GameObjectUtilities;
using UnityEngine;

namespace Game.Ui
{
    public class TransformContainer : MonoBehaviour, IContainer
    {
        public bool worldPositionStays;

        private List<object> _container = new List<object>();

        public TObject Put<TObject>(TObject prefab) where TObject : class
        {
            var inst = Instantiate(prefab as MonoBehaviour);
            inst.transform.SetParent(transform,worldPositionStays);
            
            _container.Add(inst);
            return inst as TObject;
        }
        

        public void Clear()
        {
            _container.Clear();
            transform.DestroyAllChildrens();
        }

        public IEnumerable<TObject> GetEnumerable<TObject>()
        {
            return _container.Cast<TObject>();
        }
    }
}