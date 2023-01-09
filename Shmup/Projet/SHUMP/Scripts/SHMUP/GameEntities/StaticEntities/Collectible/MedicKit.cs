using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player;

namespace Com.IsartDigital.SHMUP.GameEntities.StaticEntities.Collectible {

	public class MedicKit : StaticEntity
	{

		[Export] private int lifePoint = 1;

        protected override void OnAreaEntered(Area2D pBody)
        {
            if(pBody is Player)
            {
                ((Player)pBody).RegainHealth(lifePoint);
                QueueFree();
            }
        }

    }

}