using Godot;
using System;
using Com.IsartDigital.SHMUP.UI;

namespace Com.IsartDigital.SHMUP.Structure {
	
    public class UIManager : Node
    {
        private static UIManager instance;

        [Export] private NodePath popupContainerPath = default;

        private const string PATH_PAUSE_POPUP = "res://Scenes/UIPrefab/Popup/Pause.tscn";
        private const string PATH_GAMEOVER_POPUP = "res://Scenes/UIPrefab/Popup/GameOver.tscn";
        
        private Pause pause;
        private UI.Popup gameOver;

        private Node popupContainer;

        private UIManager ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                return;
            }
            instance = this;

            popupContainer = GetNode<Node>(popupContainerPath);

            pause = GD.Load<PackedScene>(PATH_PAUSE_POPUP).Instance<Pause>();
            popupContainer.AddChild(pause);

            pause.CloseScreen();
        }

        public static UIManager GetInstance()
        {
            if (instance == null) instance = new UIManager();
            return instance;

        }

        public void CallPopup()
        {
            pause.OpenScreen();
        }

        public void TriggerGameOver()
        {
            gameOver = GD.Load<PackedScene>(PATH_GAMEOVER_POPUP).Instance<UI.Popup>();
            popupContainer.AddChild(gameOver);
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}