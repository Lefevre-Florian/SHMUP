using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public abstract class ChargingEnemy : Enemy
	{

		[Export] private float moveDelay = 1f;

		private Timer chargeTimer = new Timer();

		private const float ENDMARGIN = 50f;

		private float internalTime;

		private bool isCharging = false;

		public override void _Ready()
		{
			base._Ready();

			internalTime = GlobalPosition.DistanceTo(new Vector2(ENDMARGIN, GlobalPosition.y)) / (speed + forcedSpeed);
			chargeTimer.WaitTime = moveDelay;
			chargeTimer.Connect(EventTimer.TIMEOUT, this, nameof(Charging));
			AddChild(chargeTimer);
		}

		protected virtual void Charging()
        {
			chargeTimer.Stop();
			velocity = Vector2.Left * speed;
			GetTree().CreateTimer(internalTime).Connect(EventTimer.TIMEOUT, this, nameof(Returning));
		}

		protected virtual void Returning()
        {
			velocity = Vector2.Right * (speed + forcedSpeed * 2);
			GetTree().CreateTimer(internalTime).Connect(EventTimer.TIMEOUT, this, nameof(ChargeProcess));
		}

		protected virtual void ChargeProcess()
        {
			chargeTimer.Start();
        }

        protected override void Destroy()
        {
			if (chargeTimer != null)
				chargeTimer.QueueFree();
            base.Destroy();
        }

    }

}