using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.Particles {

	public class Explosion : Particles2D
	{

		public void Init(Vector2 pPosition)
        {
			Position = pPosition;

			GetTree().CreateTimer(Lifetime).Connect(EventTimer.TIMEOUT, this, nameof(Destroy));

			Emitting = true;
        }

		private void Destroy()
        {
			QueueFree();
        }

	}

}