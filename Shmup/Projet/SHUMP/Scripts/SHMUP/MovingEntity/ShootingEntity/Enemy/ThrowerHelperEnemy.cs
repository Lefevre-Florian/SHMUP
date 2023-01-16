using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public class ThrowerHelperEnemy : Enemy
	{

        [Export] private NodePath weaponRendererPath = default;
        [Export] private float retractionPositionY;
        [Export] private float weaponRetractionDuration;
        [Export] private Tween.TransitionType retractionTransitionType = default;
        [Export] private Tween.EaseType retractationEaseType = default;

        private const string PROPERTY_POSITION = "position";

		private const string BOOMERANG_PATH = "res://Scenes/Prefab/Bullets/EnemyBoomerang.tscn";
		private PackedScene boomerangScene;

        private const float XMARGIN = 80f;

        private int nEntity = 0;
        private int entityThrowed = 0;

        private Timer spacingTimer;
        private Timer weaponRetractionTimer;

        private Polygon2D weaponRenderer = null;
        private Vector2 initialPosition;
        private float throwingDelay;

        public void Init(float pSpeed, float pThrowingDelay, int pNEntity)
        {
            boomerangScene = GD.Load<PackedScene>(BOOMERANG_PATH);
            weaponRenderer = GetNode<Polygon2D>(weaponRendererPath);

            speed = pSpeed;
            nEntity = pNEntity;

            throwingDelay = pThrowingDelay;
            velocity = Vector2.Right * (speed + forcedSpeed);

            spacingTimer = new Timer();
            AddChild(spacingTimer);
            spacingTimer.WaitTime = pThrowingDelay;
            spacingTimer.Connect(EventTimer.TIMEOUT, this, nameof(Shoot));

            weaponRetractionTimer = new Timer();
            AddChild(weaponRetractionTimer);
            weaponRetractionTimer.WaitTime = shootDelay - weaponRetractionDuration;
            weaponRetractionTimer.Connect(EventTimer.TIMEOUT, this, nameof(StartAnimation));
            weaponRetractionTimer.Start();
            initialPosition = Position;

            SetActionMoveAndShoot();
        }

        public override void TakeDamage(int pDamage){ }

        protected override void DoActionMove()
        {
            if (GlobalPosition.x < XMARGIN)
            {
                velocity = Vector2.Right * (speed + forcedSpeed);
            }
            if (GlobalPosition.x > screenSize.x - XMARGIN)
                velocity = Vector2.Left * Mathf.Abs(speed - forcedSpeed);
            base.DoActionMove();
        }

        protected override void Shoot()
        {
            base.Shoot();
            if(entityThrowed++ == nEntity)
            {
                spacingTimer.Stop();
                entityThrowed = 0;
                return;
            }
            spacingTimer.Start();
            EnemyBoomerang lBoomerang = boomerangScene.Instance<EnemyBoomerang>();

            bulletContainer.AddChild(lBoomerang);
            lBoomerang.Position = canon.GlobalPosition;
        }

        private void StartAnimation()
        {
            SceneTreeTween lTween = GetTree().CreateTween();
            lTween.Play();
            lTween.Chain();
            lTween.TweenProperty(weaponRenderer, PROPERTY_POSITION, initialPosition, weaponRetractionDuration)
                  .SetTrans(retractionTransitionType)
                  .SetEase(retractationEaseType);
            lTween.TweenProperty(weaponRenderer, PROPERTY_POSITION, new Vector2(initialPosition.x, initialPosition.y + retractionPositionY), weaponRetractionDuration)
                  .SetTrans(retractionTransitionType)
                  .SetEase(retractationEaseType)
                  .SetDelay(throwingDelay * (nEntity+1));
        }

        protected override void Destructor()
        {
            spacingTimer.Disconnect(EventTimer.TIMEOUT, this, nameof(Shoot));
            weaponRetractionTimer.Disconnect(EventTimer.TIMEOUT, this, nameof(StartAnimation));
            base.Destructor();
        }

    }

}