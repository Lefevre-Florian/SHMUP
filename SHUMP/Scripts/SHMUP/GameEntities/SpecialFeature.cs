using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.GameEntities;
using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player;

namespace Com.IsartDigital.SHMUP.MovingEntities {

	public class SpecialFeature : MovingEntity
	{

        [Export] public float duration = 5f;
        [Export] private int damage = 1;
        [Export (PropertyHint.ExpRange, "0.1,1,0.05")] private float spacingTime = 0.25f;
        [Export (PropertyHint.Range, "0.1,1.0,0.1")] private float slowMotionRatio = 0.5f;

        #region Controls
        private const string MOVE_UP = "Up";
        private const string MOVE_DOWN = "Down";
        private const string MOVE_LEFT = "Left";
        private const string MOVE_RIGHT = "Right";
        private const string SPECIAL = "Special";
        #endregion

        private const string STRIKE_ZONE_PATH = "res://Scenes/Prefab/SpecialFeature/StrikeZone.tscn";

        private float forcedSpeed;

        private Timer timer = null;
        private SPStrikeZone strikeZone;

        private Player owner;

        [Signal]
        public delegate void Finished();

        public override void _Ready() { }

        public void Init(Player pPlayer)
        {
            owner = pPlayer;
            Position = owner.Position;

            forcedSpeed = BackgroundManager.GetInstance().forcedSpeed;

            strikeZone = GD.Load<PackedScene>(STRIKE_ZONE_PATH).Instance<SPStrikeZone>();
            GetParent().AddChild(strikeZone);
            strikeZone.Init(duration, damage, forcedSpeed, slowMotionRatio);

            Engine.TimeScale *= slowMotionRatio;

            timer = new Timer();
            timer.WaitTime = spacingTime / (slowMotionRatio * (1/slowMotionRatio)) ;
            timer.Autostart = true;

            forcedSpeed /= (slowMotionRatio * (1 / slowMotionRatio));

            timer.Connect(EventTimer.TIMEOUT, this, nameof(Draw));

            GD.Print(duration / (slowMotionRatio * (1 / slowMotionRatio)));

            GetTree().CreateTimer(duration / (slowMotionRatio * (1 / slowMotionRatio))).Connect(EventTimer.TIMEOUT, this, nameof(Stop));

            AddChild(timer);
            SetActionMove();
        }

        public override void _Input(InputEvent pEvent)
        {
            velocity = new Vector2(forcedSpeed, 0);
            if (Input.IsActionPressed(MOVE_UP))
                velocity = new Vector2(forcedSpeed, -1 * (forcedSpeed + speed));
            else if (Input.IsActionPressed(MOVE_DOWN))
                velocity = new Vector2(forcedSpeed, 1 * (forcedSpeed + speed));
            else if (Input.IsActionPressed(MOVE_LEFT))
                velocity = new Vector2(-1 * (forcedSpeed + speed), 0);
            else if (Input.IsActionPressed(MOVE_RIGHT))
                velocity = new Vector2(1 * (forcedSpeed + speed), 0);

            if (Input.IsActionJustPressed(SPECIAL))
            {
                strikeZone.EndDrawing();
                Stop();
            }

        }

        private void Draw()
        {
            GD.Print(timer.TimeLeft);
            strikeZone.DropPoint(Position);
        }

        private void Stop()
        {
            Engine.TimeScale = 1;
            timer.Disconnect(EventTimer.TIMEOUT, this, nameof(Draw));
            EmitSignal(nameof(Finished));
            QueueFree();
        }

    }

}