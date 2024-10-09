using Kickstarter.Bootstrapper;
using Kickstarter.DependencyInjection;
using UnityEngine;

namespace Salems_Draw
{
    public class EnemyCounter : MonoBehaviour
    {
        [Inject] private SceneLoader sceneLoader;

        private static int enemyCount = 0;

        private void OnEnable()
        {
            transform.root.GetComponentInChildren<Health>().OnDie += OnEnemyDeath;
            enemyCount++;
        }

        private void OnDisable()
        {
            transform.root.GetComponentInChildren<Health>().OnDie -= OnEnemyDeath;
            enemyCount--;
        }

        private void OnEnemyDeath()
        {
            StartCoroutine(DecrementEnemyCount());
            transform.root.GetComponentInChildren<Health>().OnDie -= OnEnemyDeath;
        }

        private System.Collections.IEnumerator DecrementEnemyCount()
        {
            enemyCount--;
            yield return new WaitForSeconds(3);
            if (enemyCount <= 0)
                sceneLoader.LoadSceneGroup("You Win");
        }
    }
}
