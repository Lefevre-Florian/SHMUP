using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.MovingEntities {

	public abstract class MovingEntity : Area2D
	{

		[Export] protected float speed;

		protected Vector2 velocity;
		protected Vector2 screenSize;

		protected Action doAction;
		protected float delta;

		public override void _Ready()
		{
			screenSize = GetViewportRect().Size;

			Connect(EventArea2D.AREA_ENTERED, this, nameof(OnAreaEnter));
			Connect(EventNode.TREE_EXITING, this, nameof(Destructor));

			SetActionVoid();
		}

        public override void _Process(float pDelta)
        {

			if (doAction != null) doAction();
			delta = pDelta;
		}

		protected virtual void DoActionVoid() { }

		public virtual void SetActionVoid() 
		{
			doAction = DoActionVoid;
		}

		protected virtual void DoActionMove() 
		{
			GlobalPosition += velocity * delta;
		}

		protected virtual void SetActionMove() 
		{
			doAction = DoActionMove;
		}

		protected virtual void OnAreaEnter(Area2D pBody) { }

		protected virtual void Destructor()
        {
			Disconnect(EventNode.TREE_EXITING, this, nameof(Destructor));
			Disconnect(EventArea2D.AREA_ENTERED, this, nameof(OnAreaEnter));
        }

    }

}