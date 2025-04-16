using _Project.Scripts.Environment;
using UnityEngine;

namespace _Project.Scripts.Factories
{
    public class EnvironmentUnitFactory<T> : BaseMonoFactory<T> where T : EnvironmentObject
    {
        private T _unitPrefab;
        private float _unitSpeed;
        private int _unitScore;

        public EnvironmentUnitFactory(T monoPrefab, float unitSpeed, int unitScore) : base(monoPrefab)
        {
            _unitPrefab = monoPrefab;
            _unitSpeed = unitSpeed;
            _unitScore = unitScore;
        }
        
        public override T Create(Transform parent)
        {
            var instance = GameObject.Instantiate(_unitPrefab, parent);
            instance.SetSpeed(_unitSpeed);
            instance.SetScore(_unitScore);
            return instance;
        }
    }
}