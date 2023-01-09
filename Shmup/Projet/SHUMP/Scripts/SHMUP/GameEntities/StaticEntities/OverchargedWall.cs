using Godot;
using System;
using Com.IsartDigital.SHMUP.GameEntities;

namespace Com.IsartDigital.SHMUP.GameEntities.StaticEntities {

	public class OverchargedWall : Wall
	{

        protected override void OnAreaEntered(Area2D pBody)
        {
            base.OnAreaEntered(pBody);

            if(pBody is SPStrikeZone)
            {
                QueueFree();
            }
        }

    }

}