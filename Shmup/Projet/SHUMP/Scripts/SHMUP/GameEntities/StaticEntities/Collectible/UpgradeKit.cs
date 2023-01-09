using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player;

namespace Com.IsartDigital.SHMUP.GameEntities.StaticEntities.Collectible {

	public class UpgradeKit : StaticEntity
	{

        protected override void OnAreaEntered(Area2D pBody)
        {
            if(pBody is Player)
            {
                ((Player)pBody).canon.UpgradeWeapon();
                QueueFree();
            }
        }

    }

}