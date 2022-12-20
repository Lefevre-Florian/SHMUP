using Godot;
using System;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.Structure {

	public class BackgroundManager : Node
	{

        private static BackgroundManager instance;

		[Export] private float speed = 1000f;
        [Export] private float slowMotionSpeed = 500f;
        
        [Export] private List<float> ratio;

        private List<ParallaxLayer> layers = new List<ParallaxLayer>();

        private const int gameplayLayerIndex = 2;

        private const string PATH_BACKGROUND_PARALLAX = "../ParallaxBackground";

        private int index = 0;
        private int length; 

        public float forcedSpeed;
        private float internalSpeed;

        private BackgroundManager() : base(){ }

        public override void _Ready()
        {
            if (instance != null)
            {
                QueueFree();
                return;
            }
            instance = this;

            Vector2 lScreenSize = GetViewport().Size;

            foreach (ParallaxLayer lLayer in GetNode<ParallaxBackground>(PATH_BACKGROUND_PARALLAX).GetChildren())
            {
                layers.Add(lLayer);
                
                if(gameplayLayerIndex != index)
                    lLayer.MotionMirroring = new Vector2(lScreenSize.x , 0);
                index++;
            }

            length = layers.Count;

            internalSpeed = speed;
            forcedSpeed = internalSpeed * ratio[gameplayLayerIndex];
        }

        public override void _Process(float pDelta)
        {
            for (index = 0; index< length; ++index)
                layers[index].MotionOffset += new Vector2(-internalSpeed * ratio[index] * pDelta, 0);
        }

        public static BackgroundManager GetInstance()
        {
            if (instance == null) 
                instance = new BackgroundManager();
            return instance;
        }

        public void SlowMotion(bool pState = true)
        {
            if (pState)
                internalSpeed = slowMotionSpeed;
            else
                internalSpeed = speed;
            forcedSpeed = internalSpeed * ratio[gameplayLayerIndex];
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance != null) 
                instance = this;
            base.Dispose(pDisposing);
        }

    }

}