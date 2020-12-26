using Sirenix.Serialization;
using UnityEngine;

namespace Game.Resources
{
    // TODO: Operation group for card actions.
    public interface IResourceOperation
    {
        /// <summary>
        /// Получение символов описывающих действие.
        /// </summary>
        string GetDescriptionSymbols();
        /// <summary>
        /// Получение цвета обозначения операции.
        /// </summary>
        Color GetColor();
        
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

        public Color Color
        {
            get 
            {
                if (color == null)
                {
                    color = Color.green;        
                }

                return color;
            }
            private set => color = value;
        }
        [SerializeField]
        private Color color;

        public string GetDescriptionSymbols()
        {
            return $"+{AddCount}";
        }

        public Color GetColor()
        {
            return Color;
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
        
        public Color Color
        {
            get 
            {
                if (color == null)
                {
                    color = Color.red;        
                }

                return color;
            }
            private set => color = value;
        }
        [SerializeField]
        private Color color;

        public string GetDescriptionSymbols()
        {
            return $"-{RemoveCount}";
        }

        public Color GetColor()
        {
            return Color;
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