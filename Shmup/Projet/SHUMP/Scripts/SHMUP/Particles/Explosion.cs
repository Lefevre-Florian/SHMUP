using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.Particles {

	public class Explosion : Particles2D
	{

        public override void _Ready()
        {
			Emitting = true;
			GetTree().CreateTimer(Lifetime).Connect(EventTimer.TIMEOUT, this, nameof(Destroy));
		}

		private void Destroy()
        {
			QueueFree();
        }

	}

}