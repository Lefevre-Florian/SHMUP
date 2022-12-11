using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities {

	public class ShootingEntity : MovingEntity
	{
        [Export] protected int healthpoint = 1;

        public virtual void TakeDamage(int pDamage)
        {
            healthpoint -= pDamage;
            if (healthpoint <= 0)
                QueueFree();
        }

    }

}