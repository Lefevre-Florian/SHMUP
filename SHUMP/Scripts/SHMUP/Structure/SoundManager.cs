using Godot;
using System;
using Com.IsartDigital.Utils.Events;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.Structure {
	
    public class SoundManager : Node
    {
        private static SoundManager instance;

        [Export] private int nSoundEmitter = 60;
        [Export] private NodePath soundPoolPath = default;

        private Node soundPool = null;

        private SoundManager():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                return;
            }
            instance = this;

            soundPool = GetNode<Node>(soundPoolPath);

            for (int i = 0; i < nSoundEmitter; i++)
                soundPool.AddChild(new AudioStreamPlayer2D());
        }

        public static SoundManager GetInstance()
        {
            if (instance == null) instance = new SoundManager();
            return instance;

        }

        public AudioStreamPlayer2D GetAudioPlayer(AudioStreamOGGVorbis pStream)
        {
            AudioStreamPlayer2D lAudio = soundPool.GetChildOrNull<AudioStreamPlayer2D>(0);
            if(lAudio == null)
                lAudio = new AudioStreamPlayer2D();
            pStream.Loop = false;
            lAudio.Stream = pStream;

            lAudio.Connect(EventAudioStreamPlayer2D.FINISHED, this, nameof(CleanAudioPlayer), new Godot.Collections.Array(lAudio));
            soundPool.RemoveChild(lAudio);
            
            return lAudio;
        }

        private void CleanAudioPlayer(AudioStreamPlayer2D pAudio)
        {
            pAudio.Disconnect(EventAudioStreamPlayer2D.FINISHED, this, nameof(CleanAudioPlayer));
            pAudio.Stop();
            pAudio.Stream = null;

            pAudio.GetParent().RemoveChild(pAudio);
            soundPool.AddChild(pAudio);
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}