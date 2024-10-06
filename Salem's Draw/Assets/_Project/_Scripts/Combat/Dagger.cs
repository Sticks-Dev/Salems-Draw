using Kickstarter.Inputs;
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

        protected override void Attack()
        {
            var targetHealth = GetTargetHealth();
            targetHealth?.TakeDamage(Damage);
        }

        private Health GetTargetHealth()
        {
            var cam = Camera.main.transform;
            if (!Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, attackRange, targets))
                return null;
            return hitInfo.collider.GetComponent<Health>();
        }
        #endregion
    }
}
