using UnityEngine;

namespace Salems_Draw
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;

        public int CurrentHealth { get; private set; }

        public event System.Action OnDie;

        private void Start()
        {
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                OnDie?.Invoke();
            }
        }
    }
}
