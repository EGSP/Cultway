using UnityEngine.SceneManagement;

namespace Game.Scenes
{
    public interface ISceneOperator
    {
        void ActiveSceneChanged(Scene newActiveScene);

        void BeforeSceneUnload(Scene scene);
    }
}