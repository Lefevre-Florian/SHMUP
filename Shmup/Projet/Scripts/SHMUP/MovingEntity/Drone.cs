using Com.IsartDigital.SHMUP.GameEntities;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities {

	public class Drone : MovingEntity
	{
        [Signal]
        public delegate void Destroyed();

        protected const float BASE_ANGLE = 90f;

        private bool isRotating = false;

		private float angle = 0f;
        private float radius = 0f;

        private Area2D pParent = null;

		public void Init(float pRadius, float pSpeed, bool pRotating = false, float pStartAngle = BASE_ANGLE)
        {
            pParent = GetParent<Area2D>();

            speed = pSpeed;
            radius = pRadius;

            GlobalPosition = pParent.GlobalPosition + new Vector2(Mathf.Cos(pStartAngle), Mathf.Sin(pStartAngle)) * radius;

            if (pRotating)
                SetActionMove();
            else
                SetActionVoid();
        }

        protected override void OnAreaEnter(Area2D pBody)
        {
            if (pBody is SPStrikeZone)
                QueueFree();
        }

        protected override void DoActionMove()
        {
            angle += delta;
            GlobalPosition = pParent.GlobalPosition + new Vector2(Mathf.Cos(angle * speed), Mathf.Sin(angle * speed)) * radius;
        }

        protected override void Destructor()
        {
            EmitSignal(nameof(Destroyed));
            base.Destructor();
        }

    }

}