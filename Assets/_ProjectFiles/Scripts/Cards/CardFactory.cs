using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Cards
{
    public class CardFactory : SerializedMonoBehaviour
    {
        public List<CardInfo> cards;

        public ICardVisual CardVisualPrefab;

        private void Awake()
        {
            CheckVisual();
        }

        [Button("CreateCardVisual")]
        public void CreateCardVisual(Transform parent, CardInfo cardInfo)
        {
            if (!CheckVisual())
            {
                return;
            }

            var inst = Instantiate((MonoBehaviour) CardVisualPrefab);
            inst.transform.SetParent(parent, false);
            inst.transform.localPosition = Vector3.zero;

            ((ICardVisual) inst).Accept(cardInfo);
        }

        private bool CheckVisual()
        {
            if(CardVisualPrefab == null)
            {
                Debug.Log("Card visual prefab is null!");
                return false;
            }

            if(CardVisualPrefab is MonoBehaviour == false)
            {
                Debug.LogWarning("Card visual prefab isn't a Monobehaviour!");
                return false;
            }

            return true;
        }
    }
}