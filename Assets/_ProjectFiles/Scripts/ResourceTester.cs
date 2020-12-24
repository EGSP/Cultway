using System;
using Game.Resources;
using Game.Ui;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class ResourceTester : SerializedMonoBehaviour
    {
        [SerializeField] private ResourceStorageController storageController;
        [SerializeField] private IResourcesStorage _resourcesStorage;

        private void Start()
        {
            if (_resourcesStorage != null)
                storageController.Accept(_resourcesStorage);
        }
    }
}