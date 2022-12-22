using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities {

	public class BattleDrone : Drone
	{

        private Timer timer;

        public void Init(float pRadius, float pSpeed ,float shootDelay,  bool pRotating = false)
        {
            base.Init(pRadius, pSpeed, pRotating);

            timer = new Timer();
            timer.WaitTime = shootDelay;
            AddChild(timer);
            timer.Start();

            timer.Connect(EventTimer.TIMEOUT, this, nameof(Shoot));
        }

        private void Shoot()
        {
            GD.Print("Hit");
        }

        protected override void Destroy()
        {
            Disconnect(EventTimer.TIMEOUT, this, nameof(Shoot));
            base.Destroy();
        }

    }

}