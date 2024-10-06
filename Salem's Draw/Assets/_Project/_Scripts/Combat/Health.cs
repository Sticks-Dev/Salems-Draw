using UnityEngine;

namespace Salems_Draw
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100;

        public float CurrentHealth { get; private set; }

        public event System.Action OnDie;

        private void Start()
        {
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                OnDie?.Invoke();
            }
        }
    }
}
