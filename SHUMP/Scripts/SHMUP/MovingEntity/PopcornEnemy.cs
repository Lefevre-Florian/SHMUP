using Godot;
using System;

namespace Com.IsartDigital.SHMUP.MovingEntities {

	public class PopcornEnemy : MovingEntity
	{

        [Export] private float margin;

        private float yMax;
        private float yMin;

        public override void _Ready()
        {
            base._Ready();
            velocity = new Vector2(-speed, -1 * speed);

            yMax = GlobalPosition.y + margin;
            yMin = GlobalPosition.y - margin;

            doAction = SetActionMove;
        }

        protected override void DoActionMove()
        {
            base.DoActionMove();
            if (GlobalPosition.y <= yMin)
                velocity = new Vector2(-speed, speed);
            if (GlobalPosition.y >= yMax)
                velocity = new Vector2(-speed, -speed);
        }



    }

}