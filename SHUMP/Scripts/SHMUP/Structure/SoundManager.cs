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

        public void GetAudioPlayer(AudioStreamOGGVorbis pStream, Node pTarget)
        {
            AudioStreamPlayer2D lAudio = soundPool.GetChildOrNull<AudioStreamPlayer2D>(0);
            if(lAudio == null)
                lAudio = new AudioStreamPlayer2D();
            pStream.Loop = false;
            lAudio.Stream = pStream;
            lAudio.Autoplay = true;

            lAudio.Connect(EventAudioStreamPlayer2D.FINISHED, this, nameof(CleanAudioPlayer), new Godot.Collections.Array(lAudio, pTarget));
            soundPool.RemoveChild(lAudio);

            pTarget.AddChild(lAudio);
            pTarget.Connect("tree_exiting", this, nameof(CleanAudioPlayer), new Godot.Collections.Array(lAudio, pTarget));
        }

        private void CleanAudioPlayer(AudioStreamPlayer2D pAudio, Node pTarget)
        {
            pTarget.Disconnect("tree_exiting", this, nameof(CleanAudioPlayer));
            pAudio.Disconnect(EventAudioStreamPlayer2D.FINISHED, this, nameof(CleanAudioPlayer));
            pAudio.Stop();
            pAudio.Stream = null;

            pTarget.RemoveChild(pAudio);
            soundPool.AddChild(pAudio);
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}