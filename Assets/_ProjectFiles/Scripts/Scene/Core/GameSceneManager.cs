using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Egsp.Core;
using Egsp.Other;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scenes
{
    // TODO: Добавить класс кеша парамтеров сцены с двумя параметрами load и active.
    
    /// <summary>
    /// Подписываться на данный менеджер нужно в Awake.
    /// Это необходимо для своевременного подхвата событий SceneManager!
    /// </summary>
    public sealed class GameSceneManager : SingletonRaw<GameSceneManager>
    {
        [NotNull] private ILogger _logger;
        
        [NotNull] private SceneLoadAssistant _sceneLoadAssistant;
        [NotNull] private SceneActivateAssistant _sceneActivateAssistant;
        
        [NotNull] public EventBus Bus { get; private set; }

        public GameSceneManager() : base()
        {
            _logger = Debug.unityLogger;
            
            Bus = new EventBus();
            
            _sceneLoadAssistant = new SceneLoadAssistant(Bus, this, _logger)
                {PreventOutsideLoading = PreventLoadType.Error};
            _sceneActivateAssistant = new SceneActivateAssistant(Bus, this, _logger);
        }

        /// <summary>
        /// Запускает загрузку сцены в additive режиме. 
        /// </summary>
        public void LoadSceneAdditive(string sceneName, bool activateOnLoad)
        {
            LoadSceneAdditive(sceneName, activateOnLoad, null, null);
        }

        /// <summary>
        /// Запускает загрузку сцены в additive режиме и с параметрами.
        /// </summary>
        public void LoadSceneAdditive(string sceneName, bool activateOnLoad,[CanBeNull] SceneParams loadParams,
            [CanBeNull] SceneParams activateParams = null)
        {
            if (Application.CanStreamedLevelBeLoaded(sceneName) == false)
                return;
            
            // Формируем запрос. (Сохраняем данные нужные для распределения сцен после загрузки).
            var request = 
                FormLoadRequest(sceneName, activateOnLoad ,loadParams, activateParams);
            
            // Уведомляем обработчик загрузки о новом запросе.
            _sceneLoadAssistant.AcceptRequest(request);
            
            // Запускаем загрузку.
            var sceneRoutine = LoadSceneRoutine(sceneName);
            Coroutiner.StartRoutine(sceneRoutine);
        }
        
        /// <summary>
        /// Формируем новый запрос на загрузку сцены. Запрос будет помещен в список запросов.
        /// </summary>
        private SceneLoadRequest FormLoadRequest(string sceneName,bool activateOnLoad,
            [CanBeNull] SceneParams @params, [CanBeNull] SceneParams activateParams)
        {
            var loadRequest = new SceneLoadRequest(sceneName, activateOnLoad, @params);
            loadRequest.ActivateParams = activateParams;

            return loadRequest;
        }

        /// <summary>
        /// Корутина загрузки сцены.
        /// </summary>
        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            var loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!loadOperation.isDone)
            {
                // TODO: Возможно стоит добавить лог сообщений о прогрессе загрузки.
                yield return null;
            }
        }

        /// <summary>
        /// Выгрузка сцены по названию.
        /// Не рекомендуется если загружено несколько сцен с одинаковым названием.
        /// Вместо этого используйте перегрузку с аргументом типа Scene (gameObject.scene).
        /// </summary>
        public void UnloadScene(string sceneName, SceneParams @params = null)
        {
            UnloadScene(SceneManager.GetSceneByName(sceneName), @params);
        }

        public void UnloadScene(Scene scene, SceneParams @params = null)
        {
            // Оповещаем всех операторов перед выгрузкой сцены.
            Bus.Raise<ISceneOperator>(o =>
                o.BeforeSceneUnload(scene, @params));

            SceneManager.UnloadSceneAsync(scene);
        }
        
        /// <summary>
        /// Устанавливает активную сцену.
        /// </summary>
        public bool SetActiveScene(string sceneName, SceneParams activateParams = null)
        {
            return SetActiveScene(SceneManager.GetSceneByName(sceneName), activateParams);
        }

        public bool SetActiveScene(Scene scene, SceneParams activateParams = null)
        {
            if (scene.IsValid())
            {
                // Кешируем параметры.
                if (activateParams != null)
                    _sceneActivateAssistant.CacheActivateParams(scene, activateParams);
                
                SceneManager.SetActiveScene(scene);
                return true;
            }

            return false;
        }

        public void PreventOutsideLoading(PreventLoadType type)
        {
            _sceneLoadAssistant.PreventOutsideLoading = type;
        }
        
        
        public override void Dispose()
        {
            base.Dispose();
            
            _sceneLoadAssistant.Dispose();
            _sceneActivateAssistant.Dispose();
        }
    }
}