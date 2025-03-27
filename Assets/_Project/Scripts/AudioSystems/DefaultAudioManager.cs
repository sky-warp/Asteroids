using _Project.Scripts.AudioSystems.AudioTypes;
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
        private void Construct(BulletSoundSource bulletSoundSource, 
            LaserSoundSource laserSoundSource,
            ScoreEarnSoundSource scoreEarnSoundSource)
        {
            _bulletSound =  Instantiate(bulletSoundSource.SoundSource, transform);
            _laserSound = Instantiate(laserSoundSource.SoundSource, transform);
            _scoreEarnSound = Instantiate(scoreEarnSoundSource.SoundSource, transform);
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