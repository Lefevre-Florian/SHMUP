using Godot;
using System;
using Com.IsartDigital.Utils.Events;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.Structure {
	
    public class SoundManager : Node
    {
        private static SoundManager instance;

        [Export] private int nSoundEmitter = 60;
        [Export] private NodePath musicEmitterPath = default;

        private const string SECTION_NAME = "Settings";
        private const string KEY_VFX_NAME = "VFX";
        private const string KEY_MUSIC_NAME = "Music";

        private List<AudioStreamPlayer2D> audioPlayerPool = new List<AudioStreamPlayer2D>();
        private List<AudioStreamPlayer2D> activeAudioPlayer = new List<AudioStreamPlayer2D>();

        private AudioStreamPlayer2D musicEmitter = null;

        private static float soundEffectLevel = 0f;
        private static float musicLevel = 0f;

        private SoundManager():base() {}

        public override void _Ready()
        {
            if (instance != null){  
                QueueFree();
                return;
            }
            instance = this;

            Connect(EventNode.TREE_EXITING, this, nameof(Destructor));

            if(musicEmitterPath != null)
            {

                object lMusicData = SaveManager.LoadData(SECTION_NAME, KEY_MUSIC_NAME);
                if (lMusicData != null)
                    musicLevel = (float)lMusicData;

                musicEmitter = GetNode<AudioStreamPlayer2D>(musicEmitterPath);
                musicEmitter.VolumeDb = musicLevel;
            }

            object lVFXData = SaveManager.LoadData(SECTION_NAME, KEY_VFX_NAME);
            if (lVFXData != null)
                soundEffectLevel = (float)lVFXData;

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
            lAudio.VolumeDb = soundEffectLevel;

            activeAudioPlayer.Add(lAudio);

            lAudio.Connect(EventAudioStreamPlayer2D.FINISHED, this, nameof(CleanAudioPlayer), new Godot.Collections.Array(lAudio, pTarget));
            if(!pTarget.IsConnected(EventNode.TREE_EXITING, this, nameof(CleanAudioPlayer)))
                pTarget.Connect(EventNode.TREE_EXITING, this, nameof(CleanAudioPlayer), new Godot.Collections.Array(lAudio, pTarget));
            audioPlayerPool.Remove(lAudio);

            pTarget.AddChild(lAudio);
            
        }

        private void CleanAudioPlayer(AudioStreamPlayer2D pAudio, Node pTarget)
        {
            pAudio.Stop();
            if(pTarget.IsConnected(EventNode.TREE_EXITING, this, nameof(CleanAudioPlayer)))
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
            musicEmitter.StreamPaused = pState;
        }

        public void ChangeAudioPlayersVFXVolume(float pDBVolume)
        {
            soundEffectLevel = pDBVolume;
            int lLength = activeAudioPlayer.Count - 1;
            for (int i = lLength; i >= 0; i--)
            {
                activeAudioPlayer[i].VolumeDb = pDBVolume;
            }
        }

        public void ChangeAudioPlayerMusicVolume(float pDBVolume)
        {
            musicEmitter.VolumeDb = pDBVolume;
            musicLevel = pDBVolume; 
        }

        public void ChangeMainMusic(AudioStreamOGGVorbis pMusic)
        {   
            musicEmitter.Stop();
            musicEmitter.Stream = null;
            musicEmitter.Stream = pMusic;
            musicEmitter.Play();
        }

        private void Destructor()
        {
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