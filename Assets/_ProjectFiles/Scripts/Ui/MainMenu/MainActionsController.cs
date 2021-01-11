
using Egsp.Core;
using Egsp.Core.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui.MainMenu
{
    public interface IMenuActionsController : IVisual<IMenuActionsController>
    {
        bool ContinueInteractable { get; set; }
        void DisableContinue();
    }
    
    public class MainActionsController : MainMenuController<IMenuActionsController>, IMenuActionsController
    {
        [SerializeField] private Button continueButton;

        public bool ContinueInteractable
        {
            get => continueButton.interactable;
            set => continueButton.interactable = value;
        }
        
        protected override void Awake()
        {
            base.Awake();
        }

        public void DisableContinue()
        {
            continueButton.interactable = false;
        }

        public void Continue()
        {
            ContextBus?.Raise<IMainMenu>(x=>x.ContinueGame());
        }

        public void NewGame()
        {
            ContextBus?.Raise<IMainMenu>(x=>x.StartNewGame());
        }

        public void Settings()
        {
            ContextBus?.Raise<IMainMenu>(x=>x.OpenSettings());
        }

        public void Exit()
        {
            ContextBus?.Raise<IMainMenu>(x=>x.Exit());
        }
    }
    
    
}