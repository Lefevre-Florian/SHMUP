using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player;

namespace Com.IsartDigital.SHMUP.MovingEntities.Bullets {

	public class EnemyLaser : Area2D
	{

		[Export] public float drawingDuration;
		[Export] public float attackDuration;
		[Export] private int damage = 1;
		[Export(PropertyHint.Range, "1, 10, 1")] private int precision = 1;
		[Export] private NodePath nodePath = default;

		private const string AREA_ENTERED = "area_entered";
		private const string TIMEOUT = "timeout";

		private float spacing;
		private float delta;

		private int nPoint;
		private int interval = 0;

		private Line2D laser;
		private Timer timer;

		private Player target = null;

		private Vector2 initialPosition;

		private Action myAction = null;

		public override void _Ready()
		{
			laser = GetNode<Line2D>(nodePath);

			spacing = GlobalPosition.DistanceTo(new Vector2(0, GlobalPosition.y)) / (drawingDuration * precision);

			initialPosition = new Vector2(GlobalPosition);
			laser.AddPoint(new Vector2(GlobalPosition.x - initialPosition.x, 0f));

			nPoint = Mathf.CeilToInt(initialPosition.DistanceTo(new Vector2(0, initialPosition.y)) / spacing);
			nPoint *= precision;

			timer = new Timer();
			AddChild(timer);
			timer.WaitTime = drawingDuration / nPoint;
			timer.Connect(TIMEOUT, this, nameof(DrawLaser));
			timer.Start();

			Connect(AREA_ENTERED, this, nameof(OnAreaEntered));
		}

        public override void _Process(float pDelta)
        {
			if (myAction != null) myAction();
			delta = pDelta;
        }

		private void DrawLaser()
		{
			if (++interval > nPoint)
			{
				timer.Disconnect(TIMEOUT, this, nameof(DrawLaser));
				timer.WaitTime = attackDuration;
				timer.Connect(TIMEOUT, this, nameof(Destroy));
			}
			else
			{
				laser.AddPoint(new Vector2(GlobalPosition.x - initialPosition.x - (spacing * interval), 0));
				CollisionPolygon2D lCollider = new CollisionPolygon2D();

				Vector2 lPrevious = laser.Points[laser.Points.Length - 2];
				Vector2 lActual = laser.Points[laser.Points.Length - 1];

				Vector2[] lVectors = new Vector2[]
				{
					new Vector2(lPrevious.x, lPrevious.y + laser.Width/2),
					new Vector2(lActual.x, lActual.y + laser.Width/2),
					new Vector2(lActual.x, lActual.y - laser.Width/2),
					new Vector2(lPrevious.x, lPrevious.y - laser.Width/2)
				};

				lCollider.Polygon = lVectors;
				AddChild(lCollider);
			}

		}

		private void OnAreaEntered(Area2D pBody)
		{
			if (pBody is Player)
            {
				target = (Player)pBody;
				SetActionCollideEntity();
			}
		}

		private void OnAreaExited(Area2D pBody)
        {
			if (pBody is Player)
            {
				target = null;
				SetActionVoid();
			}
        }

		private void SetActionVoid()
        {
			myAction = DoActionVoid;
        }

		private void DoActionVoid() { }

		private void SetActionCollideEntity()
        {
			myAction = DoActionCollideEntity;
        }

		private void DoActionCollideEntity()
        {
			target.TakeDamage(damage);
        }

		private void Destroy()
		{
			timer.Stop();
			timer.Disconnect(TIMEOUT, this, nameof(Destroy));
			timer.QueueFree();

			QueueFree();
		}

	}

}