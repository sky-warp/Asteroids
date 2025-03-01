using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Projectile.Ammo.ViewModel;
using _Project.Scripts.SpawnService;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Projectile.Ammo.View
{
    public class AmmoView : MonoBehaviour
    {
        [SerializeField] private ProjectileSpawnService _projectileSpawnService;

        [SerializeField] private Transform _ammoParent;
        [SerializeField] private Image _laserImage;
        [SerializeField] private Image _cooldownImage;
        [SerializeField] private Sprite _laserSprite;

        private AmmoViewModel _ammoViewModel;
        private List<Image> _cooldownImages = new();

        public void Init(AmmoViewModel ammoViewModel)
        {
            _ammoViewModel = ammoViewModel;

            CreateLaserCount(_ammoViewModel.LaserAmmoView.Value);


            _projectileSpawnService.OnLaserSpawned
                .Subscribe(_ => _ammoViewModel.DecreaseLaserAmmo())
                .AddTo(this);
            _projectileSpawnService.OnLaserSpawned
                .Subscribe(_ => _ammoViewModel.EvaluateCooldown(_ammoViewModel.LaserCooldownView.Value))
                .AddTo(this);

            _ammoViewModel.OnCooldownChanged
                .Subscribe(ShowCooldownImage)
                .AddTo(this);
            _ammoViewModel.IsEnoughLaserView
                .Subscribe(isReady => _projectileSpawnService.IsReadyToShootLaser.Value = isReady);

            //Apply stats
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

                if (firstOrDefault != null)
                {
                    firstOrDefault.fillAmount = 1f;

                    var startTime = Time.time;

                    await Observable.EveryUpdate()
                        .TakeWhile(_ => Time.time - startTime < cooldown)
                        .ForEachAsync(_ =>
                        {
                            float elapsed = Time.time - startTime;
                            firstOrDefault.fillAmount = 1f - (elapsed / cooldown);
                        });

                    firstOrDefault.fillAmount = 0f;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Couldn't show cooldown image", e);
            }
        }
    }
}