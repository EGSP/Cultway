using System.Collections.Generic;

namespace Game.Cards
{
    public interface ICardSet : IList<CardInfo>
    {
        int Count { get; }
        
        CardInfo GetRandomCard();
    }
}