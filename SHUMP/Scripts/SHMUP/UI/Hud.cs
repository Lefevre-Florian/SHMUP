using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player;

namespace Com.IsartDigital.SHMUP.UI {
	
    public class Hud : Screen
    {
        private static Hud instance;

        [Export] private NodePath lifePath = default;

        [Export] private string scoreExtension;
        [Export] private NodePath scorePath = default;

        [Export] private NodePath smartBombPath = default;

        private Label score;
        private Label smartBomb;

        private ProgressBar lifeBar;

        private uint nScore = 0;

        private int nSmartBomb = 0;
        private int maxSmartBomb = 0;

        private Hud() : base() { }

        public override void _Ready()
        {
            if(instance != null)
            {
                QueueFree();
                return;
            }
            instance = this;

            lifeBar = GetNode<ProgressBar>(lifePath);

            score = GetNode<Label>(scorePath);
            smartBomb = GetNode<Label>(smartBombPath);

            Player lPlayer = Player.GetInstance();

            lifeBar.Value = lPlayer.GetHealthPoint();
            lifeBar.MaxValue = lPlayer.maxHealthPoint;
            lPlayer.Connect(nameof(Player.LifeState), this, nameof(UpdateLifeHUD));

            maxSmartBomb = lPlayer.maxSmartBomb;
            nSmartBomb = lPlayer.nSmartBomb;
            smartBomb.Text = $"{++nSmartBomb}/{maxSmartBomb}";

            lPlayer.Connect(nameof(Player.SmarbombState), this, nameof(UpdateSmartBombHUD));

        }

        public static Hud GetInstance()
        {
            if (instance == null) instance = new Hud();
            return instance;
        }

        public void UpdateLifeHUD(int pHealthPoint)
        {
            lifeBar.Value = pHealthPoint;
        }

        public void UpdateSmartBombHUD(bool pState)
        {
            if (pState)
                smartBomb.Text = $"{--nSmartBomb}/{maxSmartBomb}";
            else
                smartBomb.Text = $"{++nSmartBomb}/{maxSmartBomb}";
        }

        public void UpdateScoreHUD(uint pScore)
        {
            nScore += pScore;
            score.Text = $"{scoreExtension}:\n{nScore}";
        }

        public Vector2 GetScorePosition()
        {
            return score.RectPosition;
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance != null) instance = null;
            base.Dispose(pDisposing);
        }

    }
}