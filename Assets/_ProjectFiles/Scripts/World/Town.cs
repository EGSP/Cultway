using UnityEngine;

namespace Game.World
{
    public class Town : MonoBehaviour, ITown
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

    }
}