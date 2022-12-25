using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.Bullets {

	public class EnemyBoomerang : Bullet
	{

        [Export] public float throwDistance = 50f;

        private float internalTime = 0f;
        private float angle = 0;

        public override void _Ready()
        {
            base._Ready();

            internalTime = GlobalPosition.DistanceTo(new Vector2(GlobalPosition.x, GlobalPosition.y - throwDistance)) / speed;

            GetTree().CreateTimer(internalTime).Connect(EventTimer.TIMEOUT, this, nameof(Return));

            velocity = Vector2.Up * speed;
        }

        protected override void DoActionMove()
        {
            base.DoActionMove();
            angle += delta;
            Rotation = angle;
        }

        private void Return()
        {
            velocity = Vector2.Down * speed;
        }

        protected override void OnAreaEnter(Area2D pBody)
        {
            if(pBody is Player)
            {
                ((Player)pBody).TakeDamage(damage);
                Clean();
            }
        }

    }

}