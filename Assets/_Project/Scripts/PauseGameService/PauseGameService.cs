using R3;
using UnityEngine;

namespace _Project.Scripts.PauseGameService
{
    public class PauseGameService : MonoBehaviour
    {
        public readonly Subject<Unit> OnPause = new();
        
        public void PauseService(MonoBehaviour service)
        {
            service.enabled = false;
        }
    }
}