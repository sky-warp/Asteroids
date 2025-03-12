using System;
using _Project.Scripts.GameOverService;
using _Project.Scripts.SpawnService;
using R3;
using AmmoModel = _Project.Scripts.Projectiles.Ammo.Model.AmmoModel;

namespace _Project.Scripts.Projectiles.Ammo.ViewModel
{
    public class AmmoViewModel
    {
        public Subject<float> OnCooldownChanged { get; } = new();

        public readonly ReactiveProperty<int> LaserAmmoView = new();
        public readonly ReactiveProperty<float> LaserCooldownView = new();
        public readonly ReactiveProperty<bool> IsEnoughLaserView = new();
        public readonly ReactiveProperty<bool> IsGameOver = new();

        private AmmoModel _ammoModel;
        private readonly CompositeDisposable _disposable = new();

        public AmmoViewModel(AmmoModel ammoModel, ProjectileSpawnService projectileSpawnService,
            GameOverService.GameOverService gameOverService)
        {
            _ammoModel = ammoModel;
            _ammoModel.CurrentLaserAmmo
                .Subscribe(count => LaserAmmoView.Value = count)
                .AddTo(_disposable);
            _ammoModel.LaserCooldown
                .Subscribe(cooldown => LaserCooldownView.Value = cooldown)
                .AddTo(_disposable);
            _ammoModel.IsEnoughLaserAmmo
                .Subscribe(isEnough => IsEnoughLaserView.Value = isEnough)
                .AddTo(_disposable);

            gameOverService.OnGameOver
                .Subscribe(_ => IsGameOver.Value = true)
                .AddTo(_disposable);

            projectileSpawnService.OnLaserSpawned
                .Subscribe(_ => DecreaseLaserAmmo())
                .AddTo(_disposable);
            projectileSpawnService.OnLaserSpawned
                .Subscribe(_ => EvaluateCooldown(LaserCooldownView.Value))
                .AddTo(_disposable);

            IsEnoughLaserView
                .Subscribe(isReady => projectileSpawnService.IsReadyToShootLaser.Value = isReady);

            ResetAmmoStats();
        }

        public void EvaluateCooldown(float cooldown)
        {
            OnCooldownChanged?.OnNext(cooldown);

            Observable
                .Timer(TimeSpan.FromSeconds(cooldown))
                .Subscribe(_ => IncreaseLaserAmmo())
                .AddTo(_disposable);
        }

        public void DecreaseLaserAmmo()
        {
            if (LaserAmmoView.Value > 0)
            {
                LaserAmmoView.Value--;
                CheckLaserAmmo();
            }
        }

        public void ApplyAmmoStats()
        {
            _ammoModel.CurrentLaserAmmo.Value = LaserAmmoView.Value;
            _ammoModel.LaserCooldown.Value = LaserCooldownView.Value;
            _ammoModel.IsEnoughLaserAmmo.Value = IsEnoughLaserView.Value;
        }

        private void IncreaseLaserAmmo()
        {
            if (LaserAmmoView.Value + 1 <= _ammoModel.MaxAmmo)
            {
                LaserAmmoView.Value++;
                CheckLaserAmmo();
            }
        }

        private void ResetAmmoStats()
        {
            LaserAmmoView.Value = _ammoModel.CurrentLaserAmmo.Value;
            LaserCooldownView.Value = _ammoModel.LaserCooldown.Value;
            IsEnoughLaserView.Value = _ammoModel.IsEnoughLaserAmmo.Value;
        }

        private void CheckLaserAmmo()
        {
            if (_ammoModel.CurrentLaserAmmo.Value <= 0)
            {
                IsEnoughLaserView.Value = false;
            }
            else
            {
                IsEnoughLaserView.Value = true;
            }
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}