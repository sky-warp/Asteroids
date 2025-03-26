using _Project.Scripts.AudioSystems.AudioTypes;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.AudioSystems
{
    public class DefaultAudioManager : MonoBehaviour
    {
        private AudioSource _bulletSound;
        private AudioSource _laserSound;

        [Inject]
        private void Construct(BulletSound bulletSound, LaserSound laserSound)
        {
            _bulletSound =  Instantiate(bulletSound.BulletSoundSource, transform);
            _laserSound = Instantiate(laserSound.LaserSoundSource, transform);
        }

        public void PlayBulletSound()
        {
            _bulletSound.Play();
        }

        public void PlayLaserSound()
        {
            _laserSound.Play();
        }
    }
}