using System.Collections;
using UnityEngine;

namespace Salems_Draw
{
    public class VineChoke : Weapon
    {
        [SerializeField] private float damageDelay = 1f;
        [SerializeField] private float attackRadius = 1f;
        [SerializeField] private int numHits = 1;

        private Health target;

        private void Start()
        {
            var collider = new Collider[1];
            Physics.OverlapSphereNonAlloc(transform.position, attackRadius, collider, targets);
            target = collider[0]?.GetComponent<Health>();
            StartCoroutine(AttackLoop());
        }

        private IEnumerator AttackLoop()
        {
            int counter = 0;
            do
            {
                Attack();
                yield return new WaitForSeconds(damageDelay);
            } while (++counter < numHits);
            Destroy(gameObject);
        }

        protected override void Attack()
        {
            target.TakeDamage(Damage);
        }
    }
}
