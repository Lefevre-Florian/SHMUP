using Godot;
using System;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.MovingEntities.Bullets;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player;

namespace Com.IsartDigital.SHMUP.GameEntities.StaticEntities {

	public class Wall : StaticEntity
	{
        [Export] private NodePath rendererPath = default;

		private const int DAMAGE = 1;

        private float delta;
        private float size = 5f;

        private Vector2 direction;

        private Action doAction = null;
        private Player body = null;
        private Polygon2D renderer = null;

        public override void _Ready()
        {
            base._Ready();
            Connect(EventArea2D.AREA_EXITED, this, nameof(OnAreaExited));

            renderer = GetNode<Polygon2D>(rendererPath);

            SetActionVoid();
        }

        public override void _Process(float pDelta)
        {
            if (doAction != null) doAction();
            delta = pDelta;
        }

        protected override void OnAreaEntered(Area2D pBody)
        {
            if(pBody is Player)
            {
                body = (Player)pBody;
                ((Player)pBody).TakeDamage(DAMAGE);
                SetActionCollide();
            }

            if (pBody is Bullet)
                ((Bullet)pBody).QueueFree();
        }

        private void OnAreaExited(Area2D pBody)
        {
            if(pBody is Player)
            {
                body = null;
                SetActionVoid();
            }
        }

        private void SetActionVoid()
        {
            doAction = DoActionVoid;
        }

        private void DoActionVoid() { }

        private void SetActionCollide()
        {
            doAction = DoActionCollide;
        }

        protected virtual void DoActionCollide()
        {

            if (body.GlobalPosition.y > GlobalPosition.y + size)
                direction = Vector2.Down;
            else if (body.GlobalPosition.y <= GlobalPosition.y - size)
                direction = Vector2.Up;

            if (body.GlobalPosition.x > GlobalPosition.x)
                direction = Vector2.Right;
            else if (body.GlobalPosition.x <= GlobalPosition.x)
                direction = Vector2.Left;

            body.GlobalPosition += direction * (size + body.GetSpeed()) * delta;

            //body.GlobalPosition += -1.1f * body.GetVelocity() * delta;
        }

        protected override void Dispose(bool pDisposing)
        {
            body = null;
            base.Dispose(pDisposing);
        }

    }

}