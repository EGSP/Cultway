using Game.Stories;

namespace Game.Scenes
{
    public class GameStartParams : SceneParams
    {
        public readonly IStoryInfo StoryInfo;

        public GameStartParams(IStoryInfo storyInfo)
        {
            StoryInfo = storyInfo;
        }
    }
}