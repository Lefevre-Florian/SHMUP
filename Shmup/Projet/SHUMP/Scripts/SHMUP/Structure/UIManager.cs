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

        private const string PAUSE = "Pause";

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
        }

        public override void _Input(InputEvent pEvent)
        {
            if (Input.IsActionJustPressed(PAUSE))
                CallPopup();
        }

        public static UIManager GetInstance()
        {
            if (instance == null) instance = new UIManager();
            return instance;
        }

        public void CallPopup()
        {
            if (!pause.IsInsideTree())
            {
                popupContainer.AddChild(pause);
                pause.OpenScreen();
            }
            else
            {
                pause.CloseScreen();
                popupContainer.RemoveChild(pause);
            }
        }

        public void TriggerGameOver()
        {
            gameOver = GD.Load<PackedScene>(PATH_GAMEOVER_POPUP).Instance<UI.Popup>();
            popupContainer.AddChild(gameOver);
            gameOver.OpenScreen();
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}