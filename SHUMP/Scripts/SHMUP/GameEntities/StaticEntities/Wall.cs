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

        private Action doAction = null;
        private Area2D body = null;
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
                body = pBody;
                ((Player)pBody).TakeDamage(DAMAGE);
                SetActionCollide();
            }

            if (pBody is Bullet)
                pBody.QueueFree();
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

        private void DoActionCollide()
        {
            body.GlobalPosition += Vector2.Left * size; 
        }

        protected override void Dispose(bool pDisposing)
        {
            body = null;
            base.Dispose(pDisposing);
        }

    }

}