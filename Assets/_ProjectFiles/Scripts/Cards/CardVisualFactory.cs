using System;
using System.Collections.Generic;
using Egsp.Core;
using Game.Ui;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Game.Cards
{
    public class CardVisualFactory : SerializedSingleton<CardVisualFactory>
    {
        [OdinSerialize] public ICardVisual CardVisualPrefab;

        private void Awake()
        {
            CheckVisual();
        }

        [Button("CreateCardVisual")]
        public ICardVisual CreateCardVisual(Transform parent, CardInfo cardInfo)
        {
            if (!CheckVisual())
            {
                throw new NullReferenceException();
            }

            // Instancing
            var inst = Instantiate((MonoBehaviour) CardVisualPrefab);
            inst.transform.SetParent(parent, false);
            inst.transform.localPosition = Vector3.zero;

            // Logic
            var visual = (ICardVisual) inst;
            visual.Enable();
            visual.Accept(cardInfo);

            return visual;
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