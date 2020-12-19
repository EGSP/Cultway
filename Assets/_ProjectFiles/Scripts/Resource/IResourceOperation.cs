using Sirenix.Serialization;
using UnityEngine;

namespace Game.Resources
{
    public interface IResourceOperation
    {
        /// <summary>
        /// Предусловие по которому можно понять возможность совершения операции.
        /// </summary>
        bool Precondition(Resource source);
        void Invoke(Resource source);
    }
    
    [System.Serializable]
    public class AddOperation : IResourceOperation
    {
        [OdinSerialize]
        public int AddCount { get; set; }

        public bool Precondition(Resource source)
        {
            return true;
        }

        public void Invoke(Resource source)
        {
            if (source == null)
                Debug.LogWarning("Resource is null! Operation failed");

            source.Count += AddCount;
        }
    }
    
    public class RemoveOperation : IResourceOperation
    {
        [OdinSerialize]
        public int RemoveCount { get; set; }

        public bool Precondition(Resource source)
        {
            return source.Count >= RemoveCount;
        }

        public void Invoke(Resource source)
        {
            if (source == null)
                Debug.LogWarning("Resource is null! Operation failed");

            source.Count -= RemoveCount;
        }
    }
}