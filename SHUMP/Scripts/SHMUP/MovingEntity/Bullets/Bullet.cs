using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.MovingEntities.Bullets {

	public class Bullet : MovingEntity
	{
		[Export] protected int damage;

        public override void _Process(float pDelta)
        {
            base._Process(pDelta);
			if (GlobalPosition.x < 0 || GlobalPosition.x > screenSize.x
			|| GlobalPosition.y < 0 || GlobalPosition.y > screenSize.y)
				QueueFree();
		}

    }

}