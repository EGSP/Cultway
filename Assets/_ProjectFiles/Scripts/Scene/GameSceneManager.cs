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
            SceneEvents = new EventBus();
            LoadedScenes = new List<Tuple<Scene, LoadSceneMode>>();

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            
            HandleRootScene();
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
        

        public void LoadSceneAdditive(string sceneName)
        {
            if (Application.CanStreamedLevelBeLoaded(sceneName) == false)
                return;

            _sceneRoutine = LoadSceneRoutine(sceneName);
            Coroutiner.StartRoutine(_sceneRoutine);
        }

        /// <summary>
        /// Асинхронная загрузка сцены.
        /// </summary>
        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            var loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            loadOperation.allowSceneActivation = false;

            while (!loadOperation.isDone)
            {
                yield return null;
            }
            
            // Ждем срабатывания Awake и Start у объектов.
            yield return new WaitForEndOfFrame();

            // Меняем активную сцену и вызываем событие смены оператора.
            if (LastLoadedScene != null)
            {
                SceneManager.SetActiveScene(LastLoadedScene.Item1);
                SceneEvents.Raise<ISceneOperator>(o =>
                    o.ActiveSceneChanged(LastLoadedScene.Item1));
            }
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
        }
        
        
        public void UnloadScene(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            // Оповещаем всех операторов перед выгрузкой сцены.
            SceneEvents.Raise<ISceneOperator>(o=>o.BeforeSceneUnload(scene));
            SceneManager.UnloadSceneAsync(scene.name);
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
            SceneEvents.Raise<ISceneOperator>(o=>o.ActiveSceneChanged(newScene));
        }

        /// <summary>
        /// Добавляет самую первую запущенную сцену в список сцен.
        /// </summary>
        private void HandleRootScene()
        {
            var scene = SceneManager.GetActiveScene();
            OnSceneLoaded(scene, LoadSceneMode.Single);
        }

        public override void Dispose()
        {
            base.Dispose();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
    }
}