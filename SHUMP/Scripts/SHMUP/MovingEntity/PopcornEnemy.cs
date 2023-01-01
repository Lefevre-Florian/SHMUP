using Com.IsartDigital.SHMUP.GameEntities;
using Com.IsartDigital.SHMUP.GameEntities.StaticEntities.Collectible;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player;
using Godot;
using System;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.MovingEntities {

	public class PopcornEnemy : MovingEntity
	{

        public static List<PopcornEnemy> popcornEnemies = new List<PopcornEnemy>();

        [Export] private float margin;
        [Export] private uint score = 100;
        [Export] private int damage = 1;
        [Export] private PackedScene drop = null;

        private const string TRIGGER_TAG = "Trigger";

        private const string PATH_COLLECTIBLE_CONTAINER = "../../CollecitbleContainer";

        private const string PATH_SCORE_POPUP = "res://Scenes/Prefab/Juiciness/FlyingScore.tscn";

        private float yMax;
        private float yMin;

        public override void _Ready()
        {
            base._Ready();
            velocity = new Vector2(-speed, -1 * speed);

            yMax = GlobalPosition.y + margin;
            yMin = GlobalPosition.y - margin;
        }

        protected override void SetActionMove()
        {
            popcornEnemies.Add(this);
            base.SetActionMove();
        }

        protected override void DoActionMove()
        {
            base.DoActionMove();
            if (GlobalPosition.y <= yMin)
                velocity = new Vector2(-speed, speed);
            if (GlobalPosition.y >= yMax)
                velocity = new Vector2(-speed, -speed);

            if (GlobalPosition.x < 0)
                Destructor();
        }

        protected override void OnAreaEnter(Area2D pBody)
        {
            if (pBody.IsInGroup(TRIGGER_TAG))
                SetActionMove();

            if (pBody is Player)
                ((Player)pBody).TakeDamage(damage);
        }

        public void Destroy()
        {
            FlyingScore lScore = GD.Load<PackedScene>(PATH_SCORE_POPUP).Instance<FlyingScore>();
            GetParent().AddChild(lScore);
            lScore.RectPosition = Position;
            lScore.SetScore(score);

            if (drop != null)
            {
                Area2D lCollectible = drop.Instance<Area2D>();
                GetNode<Node2D>(PATH_COLLECTIBLE_CONTAINER).AddChild(lCollectible);
                lCollectible.Position = Position;
            }

            Destructor();
        }

        protected override void Destructor()
        {
            popcornEnemies.Remove(this);
            base.Destructor();
        }

    }

}