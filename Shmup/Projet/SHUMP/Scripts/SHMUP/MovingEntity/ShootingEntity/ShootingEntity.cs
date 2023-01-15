using Com.IsartDigital.SHMUP.Structure;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities {

	public abstract class ShootingEntity : MovingEntity
	{
        [Export] protected int healthpoint = 1;

        protected SoundManager soundManager = null;

        public override void _Ready()
        {
            base._Ready();
            soundManager = SoundManager.GetInstance();
        }

        protected virtual void SetActionMoveAndShoot()
        {
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

        public virtual void Destroy(){ }

    }

}