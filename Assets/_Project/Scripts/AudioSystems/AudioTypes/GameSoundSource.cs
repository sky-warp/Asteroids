using UnityEngine;

namespace _Project.Scripts.AudioSystems.AudioTypes
{
    public abstract class GameSoundSource
    {
        public readonly AudioSource SoundSource;

        protected GameSoundSource(AudioSource soundSource)
        {
            SoundSource = soundSource;
        }
    }
}