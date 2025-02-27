using _Project.Scripts.Entities;
using UnityEngine;
using R3;

namespace _Project.Scripts.LevelBorder
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelColliderBorder : MonoBehaviour
    {
        public readonly Subject<Projectile> OnProjectileExit = new();
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Projectile>())
            {
                var projectile = other.GetComponent<Projectile>();
            
                OnProjectileExit?.OnNext(projectile);
            }
        }
    }
}