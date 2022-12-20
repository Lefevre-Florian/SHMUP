using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.GameEntities;
using Godot;
using System;
using Com.IsartDigital.SHMUP.MovingEntities.ShootingEntities.Player;

namespace Com.IsartDigital.SHMUP.MovingEntities {

	public class SpecialFeature : MovingEntity
	{

        [Export] private float duration = 5f;
        [Export] private int damage = 1;
        [Export (PropertyHint.ExpRange, "0.1,1,0.05")] private float spacingTime = 0.25f;

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

        public override void _Ready() { }

        public void Init(Player pPlayer)
        {
            owner = pPlayer;
            Position = owner.Position;

            forcedSpeed = BackgroundManager.GetInstance().forcedSpeed;

            strikeZone = GD.Load<PackedScene>(STRIKE_ZONE_PATH).Instance<SPStrikeZone>();
            GetParent().AddChild(strikeZone);
            strikeZone.Init(duration, damage, forcedSpeed);

            timer = new Timer();
            timer.WaitTime = spacingTime;
            timer.Autostart = true;

            timer.Connect(EventTimer.TIMEOUT, this, nameof(Draw));

            GetTree().CreateTimer(duration).Connect(EventTimer.TIMEOUT, this, nameof(Stop));

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
            strikeZone.DropPoint(Position);
        }

        private void Stop()
        {
            timer.Disconnect(EventTimer.TIMEOUT, this, nameof(Draw));
            QueueFree();
            owner.specialFeature = false;
        }

    }

}