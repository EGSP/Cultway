using System.Collections.Generic;
using Egsp.Extensions.Linq;
using Sirenix.OdinInspector;

namespace Game.Cards
{
    public class CardSetScriptable : SerializedScriptableObject, ICardSet
    {
        public List<CardInfo> cardInfos;
        public CardInfo GetRandomCard()
        {
            return cardInfos.Random();
        }
    }
}