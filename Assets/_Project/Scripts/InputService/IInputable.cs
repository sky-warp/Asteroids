using System;
using R3;

namespace _Project.Scripts.InputService
{
    public interface IInputable
    {
        ReactiveProperty<bool> IsAvailable { get; }

        event Action OnLeftClick;
        event Action OnRightClick;

        float GetAxisVertical();
        float GetAxisHorizontal();
    }
}