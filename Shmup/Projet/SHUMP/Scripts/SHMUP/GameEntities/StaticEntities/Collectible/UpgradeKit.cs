using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player;
using Com.IsartDigital.SHMUP.Structure;

namespace Com.IsartDigital.SHMUP.GameEntities.StaticEntities.Collectible {

	public class UpgradeKit : StaticEntity
	{

        [Export] private AudioStreamOGGVorbis sound = null;

        protected override void OnAreaEntered(Area2D pBody)
        {
            if(pBody is Player)
            {
                ((Player)pBody).canon.UpgradeWeapon();

                if (sound != null)
                    SoundManager.GetInstance().GetAudioPlayer(sound, pBody);

                QueueFree();
            }
        }

    }

}