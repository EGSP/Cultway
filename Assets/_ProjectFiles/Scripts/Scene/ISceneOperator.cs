
using UnityEngine.SceneManagement;

namespace Game.Scenes
{
    public interface ISceneOperator
    {
        Scene ParentScene { get; }

        void AfterSceneLoaded(Scene loadedScene);
        
        void ActiveSceneChanged(Scene newActiveScene);

        void BeforeSceneUnload(Scene scene);
    }

    public static class SceneOperatorExtensions
    {
        public static bool IsParentScene(this ISceneOperator sceneOperator,Scene scene)
        {
            return sceneOperator.ParentScene == scene;
        }
    }
}