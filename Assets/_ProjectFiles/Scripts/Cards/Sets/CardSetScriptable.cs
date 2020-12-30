using System.Collections;
using System.Collections.Generic;
using Egsp.Extensions.Linq;
using Egsp.RandomTools;
using Sirenix.OdinInspector;

namespace Game.Cards
{
    public class CardSetScriptable : SerializedScriptableObject, ICardSet
    {
        public List<CardInfo> cardInfos;

        private WeightedList<CardInfo> _runtimeWeightedList;
        public CardInfo GetRandomCard()
        {
            if(_runtimeWeightedList == null)
                InitWeightedList();
            
            return _runtimeWeightedList.Pick().Value;
        }

        private void InitWeightedList()
        {
            _runtimeWeightedList = WeightedList<CardInfo>.FromList(this);
            _runtimeWeightedList.Step = 10f;
            _runtimeWeightedList.SetBalancer(new ThrowOverBalancer<CardInfo>(_runtimeWeightedList));
        }
        
        // INTERFACE --------------------------------------
        
        public void Add(CardInfo item)
        {
            cardInfos.Add(item);
        }

        public void Clear()
        {
            cardInfos.Clear();
        }

        public bool Contains(CardInfo item)
        {
            return cardInfos.Contains(item);
        }

        public void CopyTo(CardInfo[] array, int arrayIndex)
        {
            cardInfos.CopyTo(array,arrayIndex);
        }

        public bool Remove(CardInfo item)
        {
            return cardInfos.Remove(item);
        }

        public int Count => cardInfos.Count;
        public bool IsReadOnly => true;

        public IEnumerator<CardInfo> GetEnumerator()
        {
            return cardInfos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(CardInfo item)
        {
            return cardInfos.IndexOf(item);
        }

        public void Insert(int index, CardInfo item)
        {
            cardInfos.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            cardInfos.RemoveAt(index);
        }

        public CardInfo this[int index]
        {
            get => cardInfos[index];
            set => cardInfos[index] = value;
        }
    }
}