using Godot;
using System;

namespace Com.IsartDigital.SHMUP.Environment {

	public class Wheel : Polygon2D
	{
		[Export] private float rotationSpeed = 0f;

		private Action myAction = null;
		private float delta;

		public override void _Ready()
		{
			SetActionSpin();
		}

        public override void _Process(float pDelta)
        {
			if (myAction != null) myAction();
			delta = pDelta;
        }

        public void SetActionSpin()
        {
			myAction = DoActionSpin;
        }

		private void DoActionSpin()
        {
			Rotation += rotationSpeed * delta;
        }

		public void SetActionVoid()
        {
			myAction = DoActionVoid;
        }

		private void DoActionVoid() { }

		public void ReverseRotation()
        {
			rotationSpeed *= -1;
        }

	}

}