using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.SHMUP.MovingEntities.Bullets;

namespace Com.IsartDigital.SHMUP.Structure {
	
    public class BulletPool : Node
    {
        private static BulletPool instance;

        [Export] private int nBullet = 200;
        [Export] private List<PackedScene> bulletScene = new List<PackedScene>();
		
		public static BulletPool GetInstance () {
			if (instance == null) instance = new BulletPool();
		    return instance;

		}

        private BulletPool ():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                return;
            }
            
            instance = this;

            foreach (PackedScene lEntry in bulletScene)
            {
                for (int i = 0; i < nBullet; i++)
                    AddChild(lEntry.Instance<Bullet>());
            }

        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}