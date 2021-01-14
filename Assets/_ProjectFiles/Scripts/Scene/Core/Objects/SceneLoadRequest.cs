using System;
using JetBrains.Annotations;

namespace Game.Scenes
{
    /// <summary>
    /// Запрос на загрузку сцены, который ассоциирован с именем загружаемой сцены.
    /// Хранит в себе параметры.
    /// </summary>
    public sealed class SceneLoadRequest
    {
        public readonly string SceneName;
        /// <summary>
        /// Будет ли сцена активирована после загрузки.
        /// </summary>
        public readonly bool ActivateOnLoad;
            
        [NotNull]
        public SceneParams Params { get; private set; }
            
        /// <summary>
        /// Параметры, которые будут использованы при активации сцены после загрузки.
        /// </summary>
        [CanBeNull]
        public SceneParams ActivateParams { get; set; }
            
        /// <exception cref="Exception">Incorrect scene name.</exception>
        public SceneLoadRequest(string sceneName, bool activateOnLoad,
            [CanBeNull] SceneParams @params = null)
        {
            if(string.IsNullOrWhiteSpace(sceneName))
                throw new Exception($"Incorrect scene name:{sceneName}.");

            SceneName = sceneName;
            ActivateOnLoad = activateOnLoad;

            if (@params == null)
                @params = new EmptyParams();

            Params = @params;
        }
    }
}