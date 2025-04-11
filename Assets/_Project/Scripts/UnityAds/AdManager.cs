using Zenject;

namespace _Project.Scripts.UnityAds
{
    public class AdManager : IInitializable
    {
        private AdsInitializer _initializer;
        
        public AdManager(AdsInitializer adsInitializer) //wrapper to show loaded ads when they needed
        {
            _initializer = adsInitializer;
        }

        public void Initialize()
        {
            _initializer.InitializeAds();
        }
    }
}