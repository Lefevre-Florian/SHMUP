using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities {

	public class MovingEntity : Area2D
	{

		[Export] private float speed;

		protected Vector2 direction;

		public override void _Ready()
		{
		}

        public override void _Process(float pDelta)
        {
			GlobalPosition += direction * speed * pDelta;
		}

    }

}