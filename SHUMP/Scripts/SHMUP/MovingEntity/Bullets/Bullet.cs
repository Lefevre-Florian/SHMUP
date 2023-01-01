using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.MovingEntities.Bullets {

	public abstract class Bullet : MovingEntity
	{
        public static List<Bullet> bullets = new List<Bullet>();

		[Export] protected int damage;

        public override void _Ready()
        {
            base._Ready();

            Connect(EventNode.TREE_EXITING, this, nameof(Destructor));

            SetActionMove();
        }

        protected override void SetActionMove()
        {
            bullets.Add(this);
            base.SetActionMove();
        }

        protected override void DoActionMove()
        {
            base.DoActionMove();
            if (GlobalPosition.x < 0 || GlobalPosition.x > screenSize.x
            || GlobalPosition.y < 0 || GlobalPosition.y > screenSize.y)
                QueueFree();
        }

        protected override void Destructor()
        {
            Disconnect(EventNode.TREE_EXITING, this, nameof(Destructor));
            bullets.Remove(this);
            base.Destructor();
        }

    }

}