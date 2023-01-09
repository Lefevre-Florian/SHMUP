using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.GameEntities.StaticEntities {

	public class BreakableWall : Wall
	{

		protected override void DoActionCollide()
        {
			base.DoActionCollide();
			QueueFree();
        }

        protected override void OnAreaEntered(Area2D pBody)
        {
            base.OnAreaEntered(pBody);

            if (pBody is PlayerBullet)
                QueueFree();

        }

    }

}