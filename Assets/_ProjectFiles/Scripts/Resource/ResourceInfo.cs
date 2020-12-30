using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game.Resources
{
    [InlineEditor()]
    public class ResourceInfo : SerializedScriptableObject
    {
        [OdinSerialize] [PreviewField]
        public Sprite Sprite { get; private set; }

        [OdinSerialize]
        public string Name { get; private set; }
    }
}