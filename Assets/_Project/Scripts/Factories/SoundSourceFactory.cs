using _Project.Scripts.AudioSystems.AudioTypes;
using UnityEngine;

namespace _Project.Scripts.Factories
{
    public class SoundSourceFactory<T> where T : GameSoundSource
    {
        private T _soundSourcePrefab;

        public SoundSourceFactory(T soundSourcePrefab)
        {
            _soundSourcePrefab = soundSourcePrefab;
        }

        public AudioSource CreateSoundSource(Transform parent)
        {
            var instance = GameObject.Instantiate(_soundSourcePrefab.SoundSource, parent);
            return instance;
        }
    }
}