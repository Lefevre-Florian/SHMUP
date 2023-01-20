using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.GameEntities {
	
    public class Camera : Camera2D
    {
        private static Camera instance;

        private Camera ():base() {}

        [Export] private float shakeAmount = 1f;
        [Export] private float shakeDuration = 1f;
        [Export] private float offsetStabilisationDelay = 0.5f;
        [Export] private Tween.TransitionType offsetTransition = default;
        [Export] private Tween.EaseType offsetEase = default;

        [Export] private NodePath shadowPath = default;
        [Export] private float shadowDuration = 1f;

        private const float MIN_OFFSET = -1f;
        private const float MAX_OFFSET = 1f;

        private const string PROPERTY_OFFSET = "offset";
        private const string PROPERTY_MODULATE = "modulate";

        private Vector2 initialOffset;

        private float delta;

        private Timer timer = new Timer();
        private RandomNumberGenerator rnd = new RandomNumberGenerator();

        private ColorRect shadow;

        private Color initColor;
        private Color alteredColor;

        Action myAction = null;

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                return;
            }
            instance = this;

            rnd.Randomize();

            initialOffset = Offset;

            shadow = GetNode<ColorRect>(shadowPath);
            initColor = shadow.Modulate;
            alteredColor = new Color(shadow.Modulate.r, shadow.Modulate.g, shadow.Modulate.b, 1f);

            timer.Connect(EventTimer.TIMEOUT, this, nameof(StopShake));
            SetActionVoid();
        }

        public override void _Process(float pDelta)
        {
            if (myAction != null) myAction();
            delta = pDelta;
        }

        public static Camera GetInstance()
        {
            if (instance == null) instance = new Camera();
            return instance;
        }

        private void SetActionVoid()
        {
            myAction = DoActionVoid;
        }

        private void DoActionVoid() { }

        public void SetActionShake()
        {
            timer.WaitTime = shakeDuration;
            timer.OneShot = true;
            AddChild(timer);
            timer.Start();

            myAction = DoActionShake;
        }

        private void DoActionShake()
        {
            Offset = new Vector2(rnd.RandfRange(MIN_OFFSET, MAX_OFFSET), rnd.RandfRange(MIN_OFFSET, MAX_OFFSET)) * shakeAmount;
        }

        private void StopShake()
        {
            SetActionVoid();
            SceneTreeTween lTween = GetTree().CreateTween();
            lTween.Parallel();

            lTween.TweenProperty(this, PROPERTY_OFFSET, initialOffset, offsetStabilisationDelay)
                                .SetTrans(offsetTransition).SetEase(offsetEase);

            lTween.TweenProperty(shadow, PROPERTY_MODULATE, initColor, offsetStabilisationDelay);

            lTween.Play();
        }

        public void TriggerShadow()
        {
            SceneTreeTween lTween = GetTree().CreateTween();

            lTween.TweenProperty(shadow, PROPERTY_MODULATE, alteredColor, shadowDuration);
            lTween.Play();
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}