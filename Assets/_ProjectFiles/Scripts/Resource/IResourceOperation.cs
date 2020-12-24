using Sirenix.Serialization;
using UnityEngine;

namespace Game.Resources
{
    // TODO: Operation group for card actions.
    public interface IResourceOperation
    {
        /// <summary>
        /// Получение символов описывающих действие. "(-1) (+2)"
        /// </summary>
        string GetDescriptionSymbols();
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

        public string GetDescriptionSymbols()
        {
            return $"(+{AddCount})";
        }

        public bool Precondition(Resource source)
        {
            return true;
        }

        public void Invoke(Resource source)
        {
            if (source == null)
                Debug.LogWarning("Resource is null! Operation failed");

            source.Count += AddCount;
            Debug.Log("Added resources");
        }
    }
    
    public class RemoveOperation : IResourceOperation
    {
        [OdinSerialize]
        public int RemoveCount { get; set; }

        public string GetDescriptionSymbols()
        {
            return $"(-{RemoveCount})";
        }

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