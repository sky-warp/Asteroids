using UnityEngine;
using R3;

namespace _Project.Scripts.InputService
{
    public class InputManager : MonoBehaviour
    {
        public Subject<Unit> OnLeftClick = new Subject<Unit>();
        public Subject<Unit> OnRightClick = new Subject<Unit>();
        
        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
                OnLeftClick.OnNext(Unit.Default);
            
            if(Input.GetMouseButtonDown(1))
                OnRightClick?.OnNext(Unit.Default);
                
        }
    }
}