using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities {

	public class ShootingEntity : MovingEntity
	{

		[Export] private NodePath bulletPoolPath = default;


		private Node bulletPool;
		public override void _Ready()
		{
			base._Ready();

			bulletPool = GetNode<Node>(bulletPoolPath);
		}

        protected virtual void Shoot()
        {
			// replace by bullet
			bulletPool.GetChild<Area2D>(0);
        }

    }

}