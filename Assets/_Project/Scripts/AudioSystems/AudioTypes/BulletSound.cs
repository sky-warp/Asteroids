using UnityEngine;

namespace _Project.Scripts.AudioSystems.AudioTypes
{
    public class BulletSound
    {
        public AudioSource BulletSoundSource { get; private set; }

        public BulletSound(AudioSource bulletSoundSource)
        {
            BulletSoundSource = bulletSoundSource;
        }
    }
}