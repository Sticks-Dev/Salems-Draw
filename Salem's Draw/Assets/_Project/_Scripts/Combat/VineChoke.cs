using System.Collections;
using UnityEngine;

namespace Salems_Draw
{
    public class VineChoke : Weapon
    {
        [SerializeField] private float damageDelay = 1f;
        [SerializeField] private float size = 1f;

        private Health target;

        private void Start()
        {
            var collider = new Collider[1];
            Physics.OverlapSphereNonAlloc(transform.position, size, collider, targets);
            target = collider[0]?.GetComponent<Health>();
            StartCoroutine(AttackLoop());
        }

        private IEnumerator AttackLoop()
        {
            while (target != null)
            {
                Attack();
                yield return new WaitForSeconds(damageDelay);
            }
        }

        protected override void Attack()
        {
            target.TakeDamage(Damage);
        }
    }
}
