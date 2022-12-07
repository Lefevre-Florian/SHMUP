using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.MovingEntities {

	public class MovingEntity : Area2D
	{

		[Export] protected float speed;

		protected Vector2 velocity;

		public override void _Ready()
		{
			Connect(EventArea2D.AREA_ENTERED, this, nameof(OnAreaEnter));
		}

        public override void _Process(float pDelta)
        {
			GlobalPosition += velocity * pDelta;
		}

		protected virtual void OnAreaEnter(Area2D pBody) { }

    }

}