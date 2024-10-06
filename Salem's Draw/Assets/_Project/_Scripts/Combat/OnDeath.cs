using Kickstarter.Bootstrapper;
using Kickstarter.DependencyInjection;
using UnityEngine;

namespace Salems_Draw
{
    public class OnDeath : MonoBehaviour
    {
        [Inject] private SceneLoader sceneLoader;

        private void Awake()
        {
            var health = GetComponent<Health>();
            health.OnDie += OnDeathHandler;
        }

        private void OnDeathHandler()
        {
            sceneLoader.LoadSceneGroup("Game Over");
        }
    }
}
