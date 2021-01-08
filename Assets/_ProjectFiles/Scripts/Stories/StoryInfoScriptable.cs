using Game.Cards;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game.Stories
{
    public class StoryInfoScriptable : SerializedScriptableObject, IStoryInfo
    {
        [OdinSerialize]
        public Sprite Sprite { get; private set; }
        [OdinSerialize][MultiLineProperty(2)]
        public string Description { get; private set; }

        [OdinSerialize]
        public ICardSet CardSet { get; private set; }
    }
}