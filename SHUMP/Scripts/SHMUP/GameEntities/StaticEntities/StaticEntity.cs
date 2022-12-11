using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.GameEntities.StaticEntities {

	public class StaticEntity : Area2D
	{
		
		public override void _Ready()
		{
			Connect(EventArea2D.AREA_ENTERED, this, nameof(OnAreaEntered));
		}

		protected virtual void OnAreaEntered(Area2D pBody) { }

	}

}