using Game.Scenes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game
{
    public class SceneTester : SerializedMonoBehaviour
    {

        [Button]
        private void LoadScene(string sceneName, bool setActive = true)
        {
            GameSceneManager.Instance.LoadSceneAdditive(sceneName, setActive);
        }

        [Button]
        private void UnloadScene(string sceneName)
        {
            GameSceneManager.Instance.UnloadScene(sceneName);
        }
    }
}