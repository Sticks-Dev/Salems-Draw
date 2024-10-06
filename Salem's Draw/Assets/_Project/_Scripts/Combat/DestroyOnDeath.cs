using UnityEngine;

namespace Salems_Draw
{
    public class DestroyOnDeath : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Health>().OnDie += Die;
        }

        private void Die()
        {
            Destroy(transform.root.gameObject);
        }
    }
}
