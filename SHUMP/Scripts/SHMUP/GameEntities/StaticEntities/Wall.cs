using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player;

namespace Com.IsartDigital.SHMUP.GameEntities.StaticEntities {

	public class Wall : StaticEntity
	{

		private const int DAMAGE = 1;

        protected override void OnAreaEntered(Area2D pBody)
        {
            if(pBody is Player)
            {
                ((Player)pBody).TakeDamage(DAMAGE);
            }
        }

    }

}