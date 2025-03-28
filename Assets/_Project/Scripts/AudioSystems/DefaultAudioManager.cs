using _Project.Scripts.AudioSystems.AudioTypes;
using _Project.Scripts.Factories;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.AudioSystems
{
    public class DefaultAudioManager : MonoBehaviour
    {
        private AudioSource _bulletSound;
        private AudioSource _laserSound;
        private AudioSource _scoreEarnSound;

        [Inject]
        private void Construct(SoundSourceFactory<BulletSoundSource> bulletSoundSourceFactory,
            SoundSourceFactory<LaserSoundSource> laserSoundSourceFactory,
            SoundSourceFactory<ScoreEarnSoundSource> scoreSoundSourceFactory)
        {
            _bulletSound =  bulletSoundSourceFactory.CreateSoundSource(transform);
            _laserSound = laserSoundSourceFactory.CreateSoundSource(transform);
            _scoreEarnSound = scoreSoundSourceFactory.CreateSoundSource(transform);
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
    }
}