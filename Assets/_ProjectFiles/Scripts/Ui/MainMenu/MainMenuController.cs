using Egsp.Core;
using Egsp.Core.Ui;
using UnityEngine;

namespace Game.Ui.MainMenu
{
    public abstract class MainMenuController<TController> : SerializedContextVisual<TController>
        where TController : class
    {
        protected virtual void Awake()
        {
            
        }
    }
}