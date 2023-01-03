using Godot;
using System;
using Com.IsartDigital.Utils.Events;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.Structure {
	
    public class SoundManager : Node
    {
        private static SoundManager instance;

        [Export] private int nSoundEmitter = 60;

        private List<AudioStreamPlayer2D> audioPlayerPool = new List<AudioStreamPlayer2D>();
        private List<AudioStreamPlayer2D> activeAudioPlayer = new List<AudioStreamPlayer2D>();

        private SoundManager():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                return;
            }
            instance = this;

            Connect(EventNode.TREE_EXITING, this, nameof(Destructor));

            for (int i = 0; i < nSoundEmitter; i++)
                audioPlayerPool.Add(new AudioStreamPlayer2D());
        }

        public static SoundManager GetInstance()
        {
            if (instance == null) instance = new SoundManager();
            return instance;

        }

        public void GetAudioPlayer(AudioStreamOGGVorbis pStream, Node pTarget)
        {
            AudioStreamPlayer2D lAudio = audioPlayerPool[0];
            if(lAudio == null)
                lAudio = new AudioStreamPlayer2D();
            pStream.Loop = false;
            lAudio.Stream = pStream;
            lAudio.Autoplay = true;

            activeAudioPlayer.Add(lAudio);

            lAudio.Connect(EventAudioStreamPlayer2D.FINISHED, this, nameof(CleanAudioPlayer), new Godot.Collections.Array(lAudio, pTarget));
            audioPlayerPool.Remove(lAudio);

            pTarget.AddChild(lAudio);
            pTarget.Connect(EventNode.TREE_EXITING, this, nameof(CleanAudioPlayer), new Godot.Collections.Array(lAudio, pTarget));
        }

        private void CleanAudioPlayer(AudioStreamPlayer2D pAudio, Node pTarget)
        {
            pAudio.Stop();
            pTarget.Disconnect(EventNode.TREE_EXITING, this, nameof(CleanAudioPlayer));
            pAudio.Disconnect(EventAudioStreamPlayer2D.FINISHED, this, nameof(CleanAudioPlayer));
            pAudio.Stream = null;

            activeAudioPlayer.Remove(pAudio);

            pTarget.RemoveChild(pAudio);

            audioPlayerPool.Add(pAudio);
        }

        public void PauseAudioPlayers(bool pState)
        {
            int lLength = activeAudioPlayer.Count - 1;
            for (int i = lLength; i >= 0; i--)
            {
                activeAudioPlayer[i].StreamPaused = pState;
            }
        }

        public void ChangeAudioPlayersVolume(float pDBVolume)
        {
            int lLength = activeAudioPlayer.Count - 1;
            for (int i = lLength; i >= 0; i--)
            {
                activeAudioPlayer[i].VolumeDb = pDBVolume;
            }
        }

        private void Destructor()
        {
            int lLength = activeAudioPlayer.Count - 1;
            for (int i = lLength; i >= 0; i++)
                activeAudioPlayer[i].QueueFree();

            lLength = audioPlayerPool.Count - 1;
            for (int i = lLength; i >= 0; i++)
                audioPlayerPool[i].QueueFree();

            Disconnect(EventNode.TREE_EXITING, this, nameof(Destructor));
            QueueFree();
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}