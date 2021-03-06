﻿using System;
using System.Collections.Generic;
using System.Linq;
using Egsp.Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scenes
{
    public enum PreventLoadType
    {
        Ignore,
        Log,
        Warning,
        Error
    }
    
    
    public sealed class SceneLoadAssistant : IDisposable
    {
        [NotNull] private readonly ILogger _logger;
        
        [NotNull] private readonly EventBus _bus;
        [NotNull] private readonly GameSceneManager _gameSceneManager;

        /// <summary>
        /// Все запросы на загрузку сцен. Если сцен с одинаковым именем несколько,
        /// то не имеет значения кому принадлежит запрос с тем же именем,
        /// ведь запросы есть на все сцены. 
        /// </summary>
        [NotNull] private List<SceneLoadRequest> _sceneLoadRequests;
        
        [NotNull] public List<SceneWrap> Scenes { get; private set; }
        
        /// <summary>
        /// Запрещает загрузку сцен из вне (не используя данный менеджер). 
        /// </summary>
        public PreventLoadType PreventOutsideLoading { get; set; }
        
        [CanBeNull]
        public SceneWrap LastLoadedScene
        {
            get
            {
                if (Scenes.Count == 0)
                    return null;

                return Scenes.OrderByDescending(x=>x.CreationTime).First();
            }
        }

        public SceneLoadAssistant([NotNull] EventBus bus, [NotNull] GameSceneManager gameSceneManager,
            ILogger logger)
        {
            _bus = bus ?? throw new ArgumentNullException();
            _gameSceneManager = gameSceneManager ?? throw new ArgumentNullException();
            _logger = logger;

            _sceneLoadRequests = new List<SceneLoadRequest>();
            
            Scenes = new List<SceneWrap>();

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public void AcceptRequest(SceneLoadRequest request)
        {
            if (!_sceneLoadRequests.Contains(request))
                _sceneLoadRequests.Add(request);
        }

        /// <summary>
        /// Заносим новую сцену в список открытых.
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (SceneAlreadyLoaded(scene))
                return;
            
            Scenes.Add(new SceneWrap(scene,mode));
            
            // Получаем запрос на загрузку данной сцены.
            var request = GetLoadRequest(scene.name, true);
            if (request != null)
            {
                // Оповещаем о загрузке сцены.
                _bus.Raise<ISceneOperator>(x=>
                    x.AfterSceneLoaded(scene, request.Params));
            
                // Активация сцены.
                if (request.ActivateOnLoad)
                {
                    _gameSceneManager.SetActiveScene(scene, request.ActivateParams);
                }    
            }
            else
            {
                PreventLoadingHandler(scene, PreventOutsideLoading);
                
                // Оповещаем о загрузке сцены.
                _bus.Raise<ISceneOperator>(x=>
                    x.AfterSceneLoaded(scene, new EmptyParams()));
            }
            
        }
        
        [CanBeNull]
        private SceneLoadRequest GetLoadRequest(string sceneName, bool autoRemove = false)
        {
            var coincidence = _sceneLoadRequests.FirstOrDefault(x =>
                x.SceneName == sceneName);
            
            // TODO: обработать ситуацию когда сцена может быть загружена из вне.
            
            // Запрос всегда существует, если загружать сцену через данный менеджер.
            if (coincidence == null)
            {
                // Первая сцена всегда будет загружена из вне.
                if (Scenes.Count == 1 || Scenes.Count == 0)
                    return new SceneLoadRequest(sceneName,false);

                return null;
            }

            if (autoRemove)
                _sceneLoadRequests.Remove(coincidence);
            
            return coincidence;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            Scenes.RemoveAll(x=>x.SceneInstanceEquals(scene));
        }

        private bool SceneAlreadyLoaded(Scene scene)
        {
            var coincidence= Scenes
                .FirstOrDefault(x => x.SceneInstanceEquals(scene));

            return coincidence != null;
        }

        private void PreventLoadingHandler(Scene scene, PreventLoadType type)
        {
            switch (type)
            {
                case PreventLoadType.Ignore:
                    return;
                
                case PreventLoadType.Log:
                    _logger.Log($"Сцена была загружена из вне! {scene}");
                    return;
                
                case PreventLoadType.Warning:
                    _logger.Log($"Сцена была загружена из вне! {scene}");
                    return;
                
                case PreventLoadType.Error:
                    _logger.LogError("Загрузка сцены.",$"Сцена была загружена из вне! {scene}");
                    return;
            }
        }


        public void Dispose()
        {
            // TODO: Dispose from gamescenemanager.
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}