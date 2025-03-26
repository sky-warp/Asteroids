using UnityEngine;

namespace _Project.Scripts.AudioSystems.AudioTypes
{
    public class LaserSound
    {
        public AudioSource LaserSoundSource { get; private set; }

        public LaserSound(AudioSource laserSoundSource)
        {
            LaserSoundSource = laserSoundSource;
        }
    }
}