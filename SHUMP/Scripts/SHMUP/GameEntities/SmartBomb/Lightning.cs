using Godot;
using System;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.GameEntities.SmartBombUtilities {

	public class Lightning : Line2D
	{

		[Export] private float size = 50f;
		[Export] private float squash = 25f;
		[Export(PropertyHint.Range, "3, 8, 1")] private int maxSegment = 5;

		private const int MIN_SEGMENT = 3;
		private const float MIN_SQUASH = 5f;

		private const string METHOD_ADD_POINT = "add_point";

		private List<Vector2> points = new List<Vector2>();
		private int pointsCount = 0;

		private float drawingDuration;

		public void DrawLightning(Vector2 pFromPoint, Vector2 pToPoint, float pDuration)
		{
			Vector2 lReference = pToPoint;

			points.Add(pToPoint);

			RandomNumberGenerator lRand = new RandomNumberGenerator();
			lRand.Randomize();

			int lNSegment = lRand.RandiRange(MIN_SEGMENT, maxSegment);

			size = pFromPoint.DistanceTo(pToPoint);

			float lSegmentSize = size / lNSegment;

			SceneTreeTween lTween = GetTree().CreateTween();
			lTween.Chain();

			for (int i = 0; i < lNSegment; i++)
			{
				if (i % 2 == 0)
					points.Add(new Vector2(lReference.x - lRand.RandfRange(MIN_SQUASH, squash)
										 , lReference.y - lSegmentSize * (i + 1)));
				else
					points.Add(new Vector2(lReference.x + lRand.RandfRange(MIN_SQUASH, squash)
										  , lReference.y - lSegmentSize * (i + 1)));
			}
			pointsCount = points.Count - 1;

			drawingDuration = pDuration / (pointsCount * 2);

			for (int i = 0; i < pointsCount; i++)
				lTween.TweenMethod(this, METHOD_ADD_POINT, points[i], points[i + 1], drawingDuration / 2);

			ClearPoints();

			for (int i = pointsCount; i > 0; i--)
				lTween.TweenMethod(this, METHOD_ADD_POINT, points[i], points[i - 1], drawingDuration);

			lTween.Play();
			lTween.TweenCallback(this, nameof(Destroy));

		}

		private void Destroy()
		{
			QueueFree();
		}

	}

}