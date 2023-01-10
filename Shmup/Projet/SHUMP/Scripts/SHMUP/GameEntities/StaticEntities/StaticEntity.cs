using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.GameEntities.StaticEntities {

	public abstract class StaticEntity : Area2D
	{
		
		public override void _Ready()
		{
			Connect(EventNode.TREE_EXITING, this, nameof(Destructor));
			Connect(EventArea2D.AREA_ENTERED, this, nameof(OnAreaEntered));
		}

		protected virtual void OnAreaEntered(Area2D pBody) { }

		private void Destructor()
        {
			Disconnect(EventNode.TREE_EXITING, this, nameof(Destructor));
			Disconnect(EventArea2D.AREA_ENTERED, this, nameof(OnAreaEntered));
        }

	}

}