using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Game.Scenes
{
    // Для лучшего упраления лучше настроить очередь вызова скриптов. Оператор должен быть первее.
    public class CallbackSceneOperator : MonoBehaviour, ISceneOperator
    {
        [SerializeField] private UnityEvent onActiveSceneChanged;
        [SerializeField] private UnityEvent onBeforeSceneUnload;
        
        private void Awake()
        {
            GameSceneManager.Instance.SceneEvents.Subscribe<ISceneOperator>(this);
        }

        public Scene ParentScene => gameObject.scene;

        public void AfterSceneLoaded(Scene loadedScene)
        {
            Debug.Log($"Is loaded my (Parent:{ParentScene.name}) : {this.IsParentScene(loadedScene)}");
        }

        public void ActiveSceneChanged(Scene newActiveScene)
        {
            Debug.Log("ACTIVE SCENE CHANGED");
            Debug.Log($"Is active my (Parent:{ParentScene.name}) : {this.IsParentScene(newActiveScene)}");
            onActiveSceneChanged.Invoke();
        }

        public void BeforeSceneUnload(Scene scene)
        {
            Debug.Log("BEFORE SCENE UNLOAD");
            onBeforeSceneUnload.Invoke();
        }
    }
}