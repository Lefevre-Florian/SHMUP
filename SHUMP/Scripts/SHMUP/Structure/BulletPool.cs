using Godot;
using System;

namespace Com.IsartDigital.SHMUP.Structure {
	
    public class BulletPool : Node
    {
        static private BulletPool instance;
		
		static public BulletPool GetInstance () {
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
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}