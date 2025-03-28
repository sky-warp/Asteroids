using System.Collections.Generic;
using _Project.Scripts.AudioSystems.AudioTypes;
using _Project.Scripts.Factories;
using _Project.Scripts.GameOverServices;
using R3;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.AudioSystems
{
    public class DefaultAudioManager : MonoBehaviour
    {
        private AudioSource _bulletSound;
        private AudioSource _laserSound;
        private AudioSource _scoreEarnSound;
        private AudioSource _backgroundMusic;
        
        private List<AudioSource> _audioEffects = new();

        [Inject]
        private void Construct(SoundSourceFactory<BulletSoundSource> bulletSoundSourceFactory,
            SoundSourceFactory<LaserSoundSource> laserSoundSourceFactory,
            SoundSourceFactory<ScoreEarnSoundSource> scoreSoundSourceFactory,
            SoundSourceFactory<BackgroundMusic> backgroundMusic,
            DefaultGameStateService gameStateService)
        {
            _audioEffects.Add(_bulletSound =  bulletSoundSourceFactory.CreateSoundSource(transform));
            _audioEffects.Add(_laserSound = laserSoundSourceFactory.CreateSoundSource(transform));
            _audioEffects.Add(_scoreEarnSound = scoreSoundSourceFactory.CreateSoundSource(transform));
            _audioEffects.Add(_backgroundMusic = backgroundMusic.CreateSoundSource(transform));
            
            gameStateService.OnGameStart
                .Subscribe(_ => _backgroundMusic.Play())
                .AddTo(this);
            gameStateService.OnGameOver
                .Subscribe(_ => StopAllSounds())
                .AddTo(this);
        }

        public void PlayBulletSound()
        {
            _bulletSound.Play();
        }

        public void PlayLaserSound()
        {
            _laserSound.Play();
        }

        public void PlayScoreEarnSound()
        {
            _scoreEarnSound.Play();
        }

        public void StopAllSounds()
        {
            for (int i = 0; i < _audioEffects.Count; i++)
            {
                _audioEffects[i].Stop();
            }
        }
    }
}