using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game.Cards
{
    [InlineEditor()]
    public class CardInfo : SerializedScriptableObject
    {
        [OdinSerialize]
        public Sprite Sprite { get; private set; }
        
        [OdinSerialize]
        public string Name { get; private set; }
        
        [OdinSerialize][MultiLineProperty(2)]
        public string Description { get; private set; }
        
        /// <summary>
        /// Действия которые может совершить карта.
        /// </summary>
        [OdinSerialize][TableList(AlwaysExpanded = true)]
        public List<CardAction> CardActions { get; private set; }
    }
}