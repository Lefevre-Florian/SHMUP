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

			int lLength = line.Points.Length;

			float lDistance;

			CollisionShape2D lCollider;
			RectangleShape2D lRectangleShape;
			for (int i = 1; i <= lLength - 1; i++)
			{
				lCollider = new CollisionShape2D();
				lRectangleShape = new RectangleShape2D();

				lDistance = line.Points[i - 1].DistanceTo(line.Points[i]);

				lCollider.Position = new Vector2((line.Points[i - 1].x + line.Points[i].x) / 2, (line.Points[i - 1].y + line.Points[i].y) / 2);
				lCollider.Rotation = line.Points[i - 1].AngleToPoint(line.Points[i]);

				lRectangleShape.Extents = new Vector2(lDistance / 2, line.Width / 2);
				lCollider.Shape = lRectangleShape;

				AddChild(lCollider);
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