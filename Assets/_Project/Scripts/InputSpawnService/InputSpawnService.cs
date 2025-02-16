using _Project.Scripts.CustomPool;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.LevelBorder;
using R3;
using UnityEngine;

namespace _Project.Scripts.InputSpawnService
{
    public class InputSpawnService : MonoBehaviour
    {
        [SerializeField] private Projectileable _bulletPrefab;
        [SerializeField] private Transform _shipTransform;
        [SerializeField] private LevelColliderBorder _levelBorder;

        private CustomPool<Projectileable> _bulletPool;
        private CompositeDisposable _disposable = new();

        private void Awake()
        {
            _levelBorder.OnProjectileEnter
                .Subscribe(DeleteSpawnedProjectile)
                .AddTo(_disposable);
            
            _bulletPool = new CustomPool<Projectileable>(_bulletPrefab, 3, _shipTransform);
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
                CreateBullet();
        }

        private void CreateBullet()
        {
            var bullet = _bulletPool.Get();
            bullet.MoveProjectile();
        }

        private void DeleteSpawnedProjectile(Projectileable projectile)
        {
            _bulletPool.Release(projectile);
            Destroy(projectile.gameObject);
        }
        
        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}