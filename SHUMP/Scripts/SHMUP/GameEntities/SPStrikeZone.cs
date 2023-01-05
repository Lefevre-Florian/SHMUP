using Godot;
using System;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Enemy;
using Com.IsartDigital.SHMUP.MovingEntities;
using Com.IsartDigital.SHMUP.Structure;

namespace Com.IsartDigital.SHMUP.GameEntities {

	public class SPStrikeZone : Area2D
	{
		[Export] private NodePath linePath = default;

		private const float REMANING_TIME = .5f;

		private Line2D line;

		private SceneTreeTimer timer;

		private int damage;
		private Vector2 internalVelocity;

		public void Init(float pDuration, int pDamage, float pForcedSpeed, float pSlowMotion)
        {
			line = GetNode<Line2D>(linePath);
			line.ZIndex = -1;

			internalVelocity = new Vector2(pForcedSpeed * (pSlowMotion * (1 / pSlowMotion)), 0);

			timer = GetTree().CreateTimer(pDuration / (pSlowMotion * (1 / pSlowMotion)));
			timer.Connect(EventTimer.TIMEOUT, this, nameof(EndDrawing));
        }

        public override void _Process(float pDelta)
        {
			GlobalPosition += internalVelocity * pDelta;
        }

        public void DropPoint(Vector2 pPosition)
        {
			line.AddPoint(new Vector2(pPosition.x - Position.x, pPosition.y));
        }

		public void EndDrawing()
        {

			if(timer.IsConnected(EventTimer.TIMEOUT, this, nameof(EndDrawing)))
				timer.Disconnect(EventTimer.TIMEOUT, this, nameof(EndDrawing));

			if (line.Points.Length <= 0)
            {
				Destroy();
				return;
			}

			CollisionPolygon2D lCollision;
            foreach (Vector2[] lPolygon in Geometry.OffsetPolyline2d(line.Points, line.Width/2))
            {
				lCollision = new CollisionPolygon2D();
				lCollision.Polygon = lPolygon;
				AddChild(lCollision);
            }
			if(!IsConnected(EventArea2D.AREA_ENTERED, this, nameof(Destroy)))
				Connect(EventArea2D.AREA_ENTERED, this, nameof(OnAreaEntered));
			GetTree().CreateTimer(REMANING_TIME).Connect(EventTimer.TIMEOUT, this, nameof(Destroy));
        }

		private void OnAreaEntered(Area2D pBody)
        {
			if (pBody is Enemy)
				((Enemy)pBody).TakeDamage(damage);
			if (pBody is PopcornEnemy)
				pBody.QueueFree();
        }

		private void Destroy()
        {
			if (IsConnected(EventArea2D.AREA_ENTERED, this, nameof(OnAreaEntered)))
				Disconnect(EventArea2D.AREA_ENTERED, this, nameof(OnAreaEntered));

			QueueFree();
		}

	}

}