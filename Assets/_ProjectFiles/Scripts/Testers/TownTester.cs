﻿using System;
using DG.Tweening;
using Game.World;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class TownTester : SerializedMonoBehaviour
    {
        [BoxGroup("Systems")]
        [SerializeField] private TownManager townManager;
        [BoxGroup("Systems")]
        [SerializeField] private Map map;

        [BoxGroup("Travel")]
        [SerializeField] private float travelTime;

        private void Awake()
        {
            townManager.SpawnTowns(5);
            map.MainOffset = townManager.mainPosition;
        }

        /// <summary>
        /// Перемещение к следующему городу.
        /// Данный метод только влияет на карту, но не лагику городов.
        /// </summary>
        public void TravelToNextTown()
        {
            if (townManager.Next() == false)
            {
                Debug.LogWarning("Города закончились.");
                return;
            }

            var town = townManager.CurrentTown;
            
            // Передвигаем карту и в конце посещаем город.
            map.Move(town.Position, travelTime)
                .With(()=>VisitTargetTown(town));
        }

        /// <summary>
        /// Город, который считается посещенным.
        /// </summary>
        private void VisitTargetTown(ITown target)
        {
            target.Visited = true;
            Debug.Log("Прибытие в город.");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.grey;

            if (townManager != null)
            {
                if (townManager.CurrentTown != null)
                {
                    Gizmos.DrawSphere(townManager.CurrentTown.Position,0.3f);
                }
            }
        }
    }
}