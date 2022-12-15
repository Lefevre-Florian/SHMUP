using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities {

	public abstract class ShootingEntity : MovingEntity
	{
        [Export] protected int healthpoint = 1;

        protected virtual void SetActionMoveAndShoot()
        {
            doAction = DoActionMoveAndShoot; 
        }

        protected virtual void DoActionMoveAndShoot()
        {
            DoActionMove();
            DoActionShoot();
        }

        protected virtual void SetActionShoot() 
        {
            doAction = DoActionShoot;
        }

        protected virtual void DoActionShoot() { }

        public virtual void TakeDamage(int pDamage)
        {
            healthpoint -= pDamage;
            if (healthpoint <= 0)
                QueueFree();
        }

    }

}