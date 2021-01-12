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
    public class GameSceneManager : SingletonRaw<GameSceneManager>
    {
        private IEnumerator _sceneRoutine;

        [NotNull] private Dictionary<string, SceneParams> _sceneParamsCache; 
        
        [NotNull]
        public EventBus SceneEvents { get; private set; }
        
        [NotNull]
        public List<Tuple<Scene, LoadSceneMode>> LoadedScenes { get; private set; }

        [CanBeNull]
        public Tuple<Scene, LoadSceneMode> LastLoadedScene
        {
            get
            {
                if (LoadedScenes.Count == 0)
                    return null;

                return LoadedScenes[LoadedScenes.Count - 1];
            }
        }

        public GameSceneManager() : base()
        {
            _sceneParamsCache = new Dictionary<string, SceneParams>();
            SceneEvents = new EventBus();
            LoadedScenes = new List<Tuple<Scene, LoadSceneMode>>();

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            
            // Данный вызов не требуется, т.к. при подписке на событие обрабатывается новая сцена.
            // HandleRootScene();
        }

        public bool SetActiveScene(string sceneName)
        {
            return SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
        
        public bool SetActiveScene(Scene scene)
        {
            if (scene.IsValid())
            {
                SceneManager.SetActiveScene(scene);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Запускает загрузку сцены. 
        /// </summary>
        public void LoadSceneAdditive(string sceneName, bool autoSetActiveScene)
        {
            if (Application.CanStreamedLevelBeLoaded(sceneName) == false)
                return;

            _sceneRoutine = LoadSceneRoutine(sceneName, autoSetActiveScene);
            Coroutiner.StartRoutine(_sceneRoutine);
        }

        /// <summary>
        /// Запускает загрузку сцены с параметром.
        /// </summary>
        public void LoadSceneAdditive(string sceneName, bool autoSetActiveScene, SceneParams @params)
        {
            if (Application.CanStreamedLevelBeLoaded(sceneName) == false)
                return;
            
            // Кэшируем параметры.
            CacheParams(sceneName,@params);
            
            _sceneRoutine = LoadSceneRoutine(sceneName, autoSetActiveScene);
            Coroutiner.StartRoutine(_sceneRoutine);
        }

        /// <summary>
        /// Асинхронная загрузка сцены.
        /// </summary>
        private IEnumerator LoadSceneRoutine(string sceneName, bool autoSetActiveScene)
        {
            var loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!loadOperation.isDone)
            {
                yield return null;
            }
            
            // Ждем срабатывания Awake и Start у объектов.
            yield return new WaitForEndOfFrame();

            // Меняем активную сцену и вызываем событие смены оператора.
            if (LastLoadedScene != null)
            {
                if (autoSetActiveScene)
                {
                    SceneManager.SetActiveScene(LastLoadedScene.Item1);
                }
            }
        }

        public void UnloadScene(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            
            // Оповещаем всех операторов перед выгрузкой сцены.
            SceneEvents.Raise<ISceneOperator>(o=>
                o.BeforeSceneUnload(scene, GetParamsFromCache(sceneName, removeParams: true)));
            
            SceneManager.UnloadSceneAsync(scene.name);
        }
        
        /// <summary>
        /// Заносим новую сцену в список открытых.
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var coincidence= LoadedScenes
                .FirstOrDefault(x => x.Item1 == scene);

            // Если сцена уже была загружена.
            if (coincidence != null)
            {
                throw new SceneDuplicateException(scene);
            }
            
            LoadedScenes.Add(new Tuple<Scene, LoadSceneMode>(scene, mode));
            SceneEvents.Raise<ISceneOperator>(x=>
                x.AfterSceneLoaded(scene, GetParamsFromCache(scene.name)));
        }

        /// <summary>
        /// Убираем сцену из списка открытых сцен.
        /// </summary>
        private void OnSceneUnloaded(Scene scene)
        {
            var coincidence = LoadedScenes
                .FirstOrDefault(x => x.Item1 == scene);

            if (coincidence != null)
            {
                LoadedScenes.Remove(coincidence);
            }
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            SceneEvents.Raise<ISceneOperator>(o=>
                o.ActiveSceneChanged(newScene, GetParamsFromCache(newScene.name)));
        }

        /// <summary>
        /// Добавляет самую первую запущенную сцену в список сцен.
        /// </summary>
        private void HandleRootScene()
        {
            Coroutiner.StartRoutine(HandleRootSceneRoutine());
        }

        private IEnumerator HandleRootSceneRoutine()
        {
            // Ждем прогрузку объектов сцены.
            yield return new WaitForEndOfFrame();
            
            var scene = SceneManager.GetActiveScene();
            OnSceneLoaded(scene, LoadSceneMode.Single);
        }

        private void CacheParams(string sceneName, [CanBeNull] SceneParams @params)
        {
            if(@params == null)
                @params = new EmptyParams();
            
            if (_sceneParamsCache.ContainsKey(sceneName))
            {
                _sceneParamsCache[sceneName] = @params;
            }
            else
            {
                _sceneParamsCache.Add(sceneName, @params);
            }
        }

        [NotNull]
        private SceneParams GetParamsFromCache(string sceneName, bool removeParams = false)
        {
            if(!_sceneParamsCache.ContainsKey(sceneName))
                return new EmptyParams();

            var @params = _sceneParamsCache[sceneName];

            if (removeParams)
                _sceneParamsCache.Remove(sceneName);
            
            return @params;
        }
        

        public override void Dispose()
        {
            base.Dispose();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
    }
}