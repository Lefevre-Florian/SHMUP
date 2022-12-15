using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.MovingEntities.Bullets {

	public abstract class Bullet : MovingEntity
	{
		[Export] protected int damage;

        public override void _Ready()
        {
            base._Ready();

            doAction = SetActionMove;
        }

        protected override void DoActionMove()
        {
            base.DoActionMove();
            if (GlobalPosition.x < 0 || GlobalPosition.x > screenSize.x
            || GlobalPosition.y < 0 || GlobalPosition.y > screenSize.y)
                QueueFree();
        }

    }

}