using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities {

	public abstract class ShootingEntity : MovingEntity
	{
        [Export] protected int healthpoint = 1;

        protected virtual void SetActionMoveAndShoot()
        {
            GD.Print("Move start ! : " + GetType());
            doAction = DoActionMoveAndShoot; 
        }

        protected virtual void DoActionMoveAndShoot()
        {
            DoActionMove();
        }

        public virtual void TakeDamage(int pDamage)
        {
            healthpoint -= pDamage;
            if (healthpoint <= 0)
                Destroy();
        }

        protected virtual void Destroy()
        {
            QueueFree();
        }

    }

}