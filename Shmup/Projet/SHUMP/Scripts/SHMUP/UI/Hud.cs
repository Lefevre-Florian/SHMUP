using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player;

namespace Com.IsartDigital.SHMUP.UI {
	
    public class Hud : Screen
    {
        private static Hud instance;

        [Export] private NodePath lifePath = default;
        [Export] private NodePath scorePath = default;
        [Export] private NodePath smartBombPath = default;

        [Export] private float scoreTweenDuration;
        [Export] private float scoreTweenRotation;
        [Export] private Vector2 scoreTweenScale;
        [Export] private Tween.TransitionType scoreTweenTransition = default;
        [Export] private Tween.EaseType scoreTweenEase = default;

        private const string PROPERTY_LABEL_SCALE = "rect_scale";
        private const string PROPERTY_LABEL_ROTATION = "rect_rotation";

        private Label score;
        private Label smartBomb;

        private ProgressBar lifeBar;

        private uint nScore = 0;
        private Vector2 scoreLabelPosition;
        
        private Tween scoreTween = new Tween();
        private Vector2 initialScoreScale;
        private float initialScoreRotation;

        private int nSmartBomb = 0;
        private int maxSmartBomb = 0;

        private string scoreExtension;

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
            scoreExtension = score.Text;
            score.RectPivotOffset = new Vector2(score.RectSize / 2);
            score.Text = $"{scoreExtension}:\n{nScore}";
            score.AddChild(scoreTween);
            scoreLabelPosition = score.RectGlobalPosition;

            initialScoreRotation = score.RectRotation;
            initialScoreScale = score.RectScale;

            smartBomb = GetNode<Label>(smartBombPath);

            Player lPlayer = Player.GetInstance();

            lifeBar.Value = lPlayer.GetHealthPoint();
            lifeBar.MaxValue = lPlayer.maxHealthPoint;
            lPlayer.Connect(nameof(Player.LifeState), this, nameof(UpdateLifeHUD));

            maxSmartBomb = lPlayer.maxSmartBomb;
            nSmartBomb = lPlayer.nSmartBomb;
            smartBomb.Text = $"{nSmartBomb}/{maxSmartBomb}";

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
            if (!scoreTween.IsActive())
            {
                GD.Print(scoreTween.IsActive());
                scoreTween.InterpolateProperty(score, PROPERTY_LABEL_SCALE, initialScoreScale, scoreTweenScale, scoreTweenDuration , scoreTweenTransition, scoreTweenEase);
                scoreTween.InterpolateProperty(score, PROPERTY_LABEL_ROTATION, initialScoreRotation, scoreTweenRotation, scoreTweenDuration, scoreTweenTransition, scoreTweenEase);
                float lInternalDuration = scoreTween.GetRuntime();
                scoreTween.InterpolateProperty(score, PROPERTY_LABEL_SCALE, scoreTweenScale, initialScoreScale, scoreTweenDuration,
                                               scoreTweenTransition, 
                                               scoreTweenEase, 
                                               delay:lInternalDuration);
                scoreTween.InterpolateProperty(score, PROPERTY_LABEL_ROTATION, scoreTweenRotation, initialScoreRotation, scoreTweenDuration, 
                                               scoreTweenTransition, 
                                               scoreTweenEase, 
                                               delay:lInternalDuration);
                scoreTween.Start();
            }
            
            nScore += pScore;
            score.Text = $"{scoreExtension}:\n{nScore}";
        }

        public Vector2 GetScorePosition()
        {
            return new Vector2(GetViewportRect().Size.x / 2, scoreLabelPosition.y);
        }

        public uint GetScoreValue()
        {
            return nScore;
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance != null) instance = null;
            base.Dispose(pDisposing);
        }

    }
}