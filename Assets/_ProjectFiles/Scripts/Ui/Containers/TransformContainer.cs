using Egsp.Utils.GameObjectUtilities;
using UnityEngine;

namespace Game.Ui
{
    public class TransformContainer : MonoBehaviour, IContainer
    {
        public bool worldPositionStays;

        private void Awake()
        {
            // Parent = transform;
        }

        public TObject Put<TObject>(TObject prefab) where TObject : class
        {
            var inst = Instantiate(prefab as MonoBehaviour);
            inst.transform.SetParent(transform,worldPositionStays);
            return inst as TObject;
        }
        //
        // public TObject Put<TObject>(TObject prefab) where TObject : class
        // {
        //     
        // }
        

        public void Clear()
        {
            transform.DestroyAllChildrens();
        }
    }
}