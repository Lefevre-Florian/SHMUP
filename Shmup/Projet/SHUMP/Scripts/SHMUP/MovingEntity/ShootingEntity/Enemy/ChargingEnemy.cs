using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy {

	public abstract class ChargingEnemy : Enemy
	{

		[Export] private float moveDelay = 1f;
		[Export] private float chargeSpeed;

		private Timer chargeTimer = new Timer();

		private const float ENDMARGIN = 50f;

		private float internalTime;

		private bool isCharging = false;

		protected virtual void InitChargeProcess()
        {
			internalTime = GlobalPosition.DistanceTo(new Vector2(ENDMARGIN, GlobalPosition.y)) / (chargeSpeed + forcedSpeed);
			chargeTimer.WaitTime = moveDelay;
			chargeTimer.Connect(EventTimer.TIMEOUT, this, nameof(Charging));
			AddChild(chargeTimer);

			StartChargeProcess();
		}

		protected virtual void Charging()
        {
			chargeTimer.Stop();
			velocity = Vector2.Left * chargeSpeed;
			GetTree().CreateTimer(internalTime).Connect(EventTimer.TIMEOUT, this, nameof(Returning));
		}

		protected virtual void Returning()
        {
			velocity = Vector2.Right * (chargeSpeed + forcedSpeed * 2);
			GetTree().CreateTimer(internalTime).Connect(EventTimer.TIMEOUT, this, nameof(StartChargeProcess));
		}

		protected virtual void StartChargeProcess()
        {
			chargeTimer.Start();
        }

		protected void StopChargeProcess()
        {
			chargeTimer.Stop();
			chargeTimer.Disconnect(EventTimer.TIMEOUT, this, nameof(Charging));
			chargeTimer.QueueFree();

			chargeTimer = null;
        }

        public override void Destroy()
        {
			if (chargeTimer != null)
				chargeTimer.QueueFree();
            base.Destroy();
        }

    }

}