using UnityEngine;

namespace _Project.Scripts.Configs.AudioConfigs
{
    [CreateAssetMenu(fileName = "AudioSystemInstaller", menuName = "Installers/Audio System Installer")]
    public class AudioSystemConfig : ScriptableObject
    {
        [field: SerializeField] public AudioSource BulletSound { get; private set; }
        [field: SerializeField] public AudioSource LaserSound { get; private set; }
    }
}