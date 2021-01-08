using Game.Cards;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Stories
{
    public interface IStoryInfo
    {
        [CanBeNull] Sprite Sprite { get; }

        string Description { get; }

        /// <summary>
        /// Сет карт, который будет использоваться в игре.
        /// </summary>
        [CanBeNull]ICardSet CardSet { get; }
    }
}