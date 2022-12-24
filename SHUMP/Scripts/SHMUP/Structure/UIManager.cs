using Godot;
using System;
using Com.IsartDigital.SHMUP.UI;

namespace Com.IsartDigital.SHMUP.Structure {
	
    public class UIManager : Node
    {
        private static UIManager instance;

        private const string PATH_PAUSE_POPUP = "res://Scenes/UIPrefab/Popup/Pause.tscn";
        private const string PATH_GAMEOVER_POPUP = "res://Scenes/UIPrefab/Popup/GameOver.tscn";
        
        private Pause pause;
        private UI.Popup gameOver;


        private UIManager ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                return;
            }
            instance = this;

            pause = GD.Load<PackedScene>(PATH_PAUSE_POPUP).Instance<Pause>();
            AddChild(pause);

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

            GetTree().Paused = true;

            AddChild(gameOver);
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}