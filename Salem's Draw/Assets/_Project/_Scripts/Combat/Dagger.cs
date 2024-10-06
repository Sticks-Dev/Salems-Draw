using Kickstarter.Inputs;
using System.Collections;
using UnityEngine;

namespace Salems_Draw
{
    public class Dagger : Weapon, IInputReceiver
    {
        #region Input Handler
        [SerializeField] private FloatInput attackInput;

        public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            attackInput.RegisterInput(OnAttackInputChange, playerIdentifier);
        }

        public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            attackInput.DeregisterInput(OnAttackInputChange, playerIdentifier);
        }

        private void OnAttackInputChange(float input)
        {
            if (input == 0)
                return;
            Attack();
        }
        #endregion

        #region Attacking
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private float attackCooldown = 0.5f;

        public event System.Action<float> CooldownProgressChanged;

        private bool canAttack = true;

        protected override void Attack()
        {
            var targetHealth = GetTargetHealth();
            StartCoroutine(Attack(targetHealth));
        }

        private Health GetTargetHealth()
        {
            var cam = Camera.main.transform;
            if (!Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, attackRange, targets))
                return null;
            return hitInfo.collider.GetComponent<Health>();
        }

        private IEnumerator Attack(Health targetHealth)
        {
            if (!canAttack)
                yield break;
            canAttack = false;
            if (targetHealth != null)
                targetHealth.TakeDamage(Damage);

            float elapsedTime = 0f;
            while (elapsedTime < attackCooldown)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / attackCooldown;
                CooldownProgressChanged?.Invoke(progress);
                yield return null;
            }

            canAttack = true;
        }
        #endregion
    }
}
