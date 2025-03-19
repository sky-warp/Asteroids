using UnityEngine;

namespace _Project.Scripts.Infrastructure
{
    public class DummyForSpawnTimer : MonoBehaviour
    {
        private float _elapsedTime;
        
        public void Update()
        {
            if(gameObject.transform.childCount == 0)
                Destroy(gameObject);
        }
    }
}