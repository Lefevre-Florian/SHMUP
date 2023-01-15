using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities {

	public class BattleDrone : Drone
	{
        [Export] private NodePath rendererPath = default;
        [Export] private float shootingSignalDelay = 0f;
        [Export] private int nBlipWarningSignal;
        [Export] private Tween.TransitionType shootingTransitionType = default;
        [Export] private Tween.EaseType shootingEase = default;
        [Export] private Color warningSignalColor = default;

        [Export] private AudioStreamOGGVorbis soundLaser = null;

        private const string PATH_LASER_SCENE = "res://Scenes/Prefab/Bullets/EnemyLaser.tscn";
        private const string PROPERTY_COLOR = "color";

        private float shootDelay;

        private Timer timer = null;
        private SceneTreeTween tween = null;
        private PackedScene laserScene = null;

        private Color initialColor;
        private Polygon2D renderer;

        private SoundManager soundManager = null;

        public override void _Ready()
        {
            base._Ready();
            laserScene = GD.Load<PackedScene>(PATH_LASER_SCENE);
            soundManager = SoundManager.GetInstance();
        }

        public void Init(float pRadius, float pSpeed ,float pShootDelay,  bool pRotating = false)
        {
            base.Init(pRadius, pSpeed, pRotating);

            renderer = GetNode<Polygon2D>(rendererPath);
            initialColor = renderer.Color;

            shootDelay = pShootDelay;

            timer = new Timer();
            timer.WaitTime = pShootDelay;
            AddChild(timer);
            timer.Start();

            timer.Connect(EventTimer.TIMEOUT, this, nameof(TriggerSignal));
        }

        private void TriggerSignal()
        {
            tween = GetTree().CreateTween();
            tween.Chain();
            for (int i = 0; i < nBlipWarningSignal; i++)
            {
                tween.TweenProperty(renderer, PROPERTY_COLOR, warningSignalColor, shootingSignalDelay / (nBlipWarningSignal))
                 .SetTrans(shootingTransitionType)
                 .SetEase(shootingEase);
                tween.TweenProperty(renderer, PROPERTY_COLOR, initialColor, shootingSignalDelay / (nBlipWarningSignal))
                     .SetTrans(shootingTransitionType)
                     .SetEase(shootingEase);
            }
            tween.TweenCallback(this, nameof(Shoot));
        }

        private void Shoot()
        {
            soundManager.GetAudioPlayer(soundLaser, this);
            EnemyLaser lLaser = laserScene.Instance<EnemyLaser>();
            AddChild(lLaser);
            timer.WaitTime = shootDelay + lLaser.drawingDuration + lLaser.attackDuration;
        }

        protected override void Destructor()
        {
            if(timer.IsConnected(EventTimer.TIMEOUT, this, nameof(Shoot)))
                timer.Disconnect(EventTimer.TIMEOUT, this, nameof(Shoot));
            base.Destructor();
        }

    }

}