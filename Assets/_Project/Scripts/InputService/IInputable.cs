using System;
using R3;

namespace _Project.Scripts.InputService
{
    public interface IInputable
    {
        ReactiveProperty<bool> IsAvailable { get; }

        event Action OnBulletRelease;
        event Action OnLaserRelease;

        float GetAxisVertical();
        float GetAxisHorizontal();
    }
}