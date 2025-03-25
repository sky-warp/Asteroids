using _Project.Scripts.Bootstrap;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    [CreateAssetMenu(fileName = "BootstrapSceneInstaller", menuName = "Installers/BootstrapSceneInstaller")]
    public class BootstrapSceneInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesTo<Bootstrapper>()
                .AsSingle()
                .NonLazy();
        }
    }
}