using Game.Stories;
using Game.Ui.Stories;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Game
{
    public class StoryVisualTester : SerializedMonoBehaviour
    {
        [OdinSerialize] private IStoryInfoVisual _storyInfoVisual;
        [OdinSerialize] private IStoryInfo _storyInfo;

        private void Awake()
        {
            if (_storyInfoVisual != null)
            {
                if (_storyInfo != null)
                {
                    _storyInfoVisual.Accept(_storyInfo);
                }
            }
        }
    }
}