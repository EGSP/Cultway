using System;
using System.Collections.Generic;
using Egsp.Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scenes
{
    public class SceneActivateAssistant : IDisposable
    {
        [NotNull] private readonly ILogger _logger;
        
        [NotNull] private readonly EventBus _bus;
        [NotNull] private readonly GameSceneManager _gameSceneManager;

        /// <summary>
        /// Кеш параметров активируемых сцен. Очищается при выгрузке сцены с соответствующим названием.
        /// </summary>
        [NotNull] private Dictionary<Scene, SceneParams> _sceneActivateParamsCache;

        public SceneActivateAssistant([NotNull] EventBus bus, [NotNull] GameSceneManager gameSceneManager,
            ILogger logger)
        {
            _bus = bus ?? throw new ArgumentNullException();
            _gameSceneManager = gameSceneManager ?? throw new ArgumentNullException();
            _logger = logger;

            _sceneActivateParamsCache = new Dictionary<Scene, SceneParams>();

            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        
        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            _bus.Raise<ISceneOperator>(o=>
                o.ActiveSceneChanged(newScene, GetActivateParams(newScene)));
            
            // Очищаем старые данные.
            RemoveActivateParams(newScene);
        }
        
        public void CacheActivateParams(Scene scene, [NotNull] SceneParams activateParams)
        {
            if(activateParams == null)
                throw new ArgumentNullException();
            
            if (_sceneActivateParamsCache.ContainsKey(scene))
            {
                _sceneActivateParamsCache[scene] = activateParams;
            }
            else
            {
                _sceneActivateParamsCache.Add(scene, activateParams);
            }
        }

        [CanBeNull]
        private SceneParams GetActivateParams(Scene scene, bool autoRemove = false)
        {
            if (_sceneActivateParamsCache.ContainsKey(scene))
            {
                var @params = _sceneActivateParamsCache[scene];
                if (autoRemove)
                    _sceneActivateParamsCache.Remove(scene);

                return @params;
            }

            return null;
        }

        private void RemoveActivateParams(Scene scene)
        {
            _sceneActivateParamsCache.Remove(scene);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            RemoveActivateParams(scene);
        }
        
        public void Dispose()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}