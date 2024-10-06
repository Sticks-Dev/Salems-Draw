using UnityEngine;

namespace Salems_Draw
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] private float damage = 1f;
        [SerializeField] protected LayerMask targets;

        public float Damage => damage;

        protected abstract void Attack();
    }
}
