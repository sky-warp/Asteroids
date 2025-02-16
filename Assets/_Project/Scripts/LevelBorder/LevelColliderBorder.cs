using _Project.Scripts.Infrastructure;
using UnityEngine;
using R3;

namespace _Project.Scripts.LevelBorder
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelColliderBorder : MonoBehaviour
    {
        public Subject<Projectileable> OnProjectileEnter = new Subject<Projectileable>();
        
        private void OnTriggerExit2D(Collider2D other)
        {
            var bullet = other.GetComponent<Projectileable>();
            
            OnProjectileEnter?.OnNext(bullet);
        }
    }
}