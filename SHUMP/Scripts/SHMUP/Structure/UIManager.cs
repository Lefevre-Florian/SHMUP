using Godot;
using System;
using Com.IsartDigital.SHMUP.UI;

namespace Com.IsartDigital.SHMUP.Structure {
	
    public class UIManager : Node
    {
        private static UIManager instance;

        private const string PATH_PAUSE_POPUP = "res://Scenes/UIPrefab/Popup.tscn";
        private UI.Popup pause;

        private UIManager ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                return;
            }
            
            instance = this;

            pause = GD.Load<PackedScene>(PATH_PAUSE_POPUP).Instance<UI.Popup>();
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

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}