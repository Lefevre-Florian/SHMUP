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

	}

}