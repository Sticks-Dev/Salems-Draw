using System.Collections;
using UnityEngine;

namespace Salems_Draw
{
    public class HazardGas : Weapon
    {
        [SerializeField] private float damageDelay = 1f;

        private ParticleSystem particles;
        private Health target;

        private IEnumerator Start()
        {
            particles = GetComponent<ParticleSystem>();
            yield return new WaitUntil(() => particles.isStopped);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((targets.value & (1 << other.gameObject.layer)) == 0)
                return;
            target = other.GetComponent<Health>();
            StartCoroutine(AttackLoop());
        }

        private void OnTriggerExit(Collider other)
        {
            if (target != null && other.gameObject == target.gameObject)
                target = null;
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
