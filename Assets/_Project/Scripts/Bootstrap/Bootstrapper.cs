using UnityEngine;

namespace _Project.Scripts.Bootstrap
{
    public class Bootstrapper : MonoBehaviour
    {
        private  void Start()
        {
            Initialize();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
        }
    }
}