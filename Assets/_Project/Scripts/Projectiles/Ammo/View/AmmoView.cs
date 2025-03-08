using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Projectiles.Ammo.ViewModel;
using _Project.Scripts.SpawnService;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Projectiles.Ammo.View
{
    public class AmmoView : MonoBehaviour
    {
        [SerializeField] private Transform _ammoParent;
        [SerializeField] private Image _laserImage;
        [SerializeField] private Image _cooldownImage;
        [SerializeField] private Sprite _laserSprite;

        private AmmoViewModel _ammoViewModel;
        private List<Image> _cooldownImages;

        public void Init(AmmoViewModel ammoViewModel, ProjectileSpawnService projectileSpawnService,
            PauseGameService.PauseGameService pauseGameService)
        {
            pauseGameService.OnPause
                .Subscribe(_ => GameOver())
                .AddTo(this);
            
            _cooldownImages = new List<Image>();

            _ammoViewModel = ammoViewModel;

            CreateLaserCount(_ammoViewModel.LaserAmmoView.Value);

            projectileSpawnService.OnLaserSpawned
                .Subscribe(_ => _ammoViewModel.DecreaseLaserAmmo())
                .AddTo(this);
            projectileSpawnService.OnLaserSpawned
                .Subscribe(_ => _ammoViewModel.EvaluateCooldown(_ammoViewModel.LaserCooldownView.Value))
                .AddTo(this);

            _ammoViewModel.OnCooldownChanged
                .Subscribe(ShowCooldownImage)
                .AddTo(this);
            _ammoViewModel.IsEnoughLaserView
                .Subscribe(isReady => projectileSpawnService.IsReadyToShootLaser.Value = isReady);

            _ammoViewModel.LaserAmmoView
                .Subscribe(_ => _ammoViewModel.ApplyAmmoStats())
                .AddTo(this);
            _ammoViewModel.LaserCooldownView
                .Subscribe(_ => _ammoViewModel.ApplyAmmoStats())
                .AddTo(this);
            _ammoViewModel.IsEnoughLaserView
                .Subscribe(_ => _ammoViewModel.ApplyAmmoStats())
                .AddTo(this);
        }

        private void GameOver()
        {
            _ammoParent.gameObject.SetActive(false);
        }

        private void CreateLaserCount(int count)
        {
            _laserImage.sprite = _laserSprite;

            for (int i = 0; i < count; i++)
            {
                var currentImage = Instantiate(_laserImage, _ammoParent);

                var currentCooldown = Instantiate(_cooldownImage, currentImage.transform);
                currentCooldown.fillAmount = 0f;
                _cooldownImages.Add(currentCooldown);
            }
        }

        private async void ShowCooldownImage(float cooldown)
        {
            try
            {
                var firstOrDefault = _cooldownImages.FirstOrDefault(image => image.fillAmount == 0f);

                if (firstOrDefault != null && firstOrDefault.gameObject != null &&
                    firstOrDefault.gameObject.activeInHierarchy)
                {
                    firstOrDefault.fillAmount = 1f;

                    var startTime = Time.time;

                    await Observable.EveryUpdate()
                        .TakeWhile(_ => Time.time - startTime < cooldown && firstOrDefault != null)
                        .ForEachAsync(_ =>
                        {
                            if (firstOrDefault == null)
                                return;

                            float elapsed = Time.time - startTime;
                            firstOrDefault.fillAmount = 1f - (elapsed / cooldown);
                        });

                    if (firstOrDefault != null)
                    {
                        firstOrDefault.fillAmount = 0f;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Couldn't show cooldown image", e);
            }
        }
    }
}