using Com.IsartDigital.SHMUP.UI.UIElements;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.Environment {

	public class NewsTitle : TranslableLabel
	{

		[Export] private float speed = 50f;

		private Action myAction = null;

		private float delta;

		private Vector2 screenSize;

		public override void _Ready()
		{
			base._Ready();
			screenSize = GetViewportRect().Size;

			SetActionMove();
		}

        public override void _Process(float pDelta)
        {
			if (myAction != null) myAction();
			delta = pDelta;
        }

        private void SetActionMove()
        {
			myAction = DoActionMove;
        }

		private void SetActionVoid() 
		{
			myAction = DoActionVoid;
		}

		private void DoActionVoid() { }

		private void DoActionMove()
        {
			RectPosition += Vector2.Right * speed * delta;
			if (RectPosition.x >= screenSize.x)
				RectPosition = new Vector2(0f, RectPosition.y);
        }

	}

}