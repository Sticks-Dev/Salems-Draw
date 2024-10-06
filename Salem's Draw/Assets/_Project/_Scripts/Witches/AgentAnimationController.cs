using UnityEngine;

namespace Salems_Draw
{
    [RequireComponent(typeof(Animator))]
    public class AgentAnimationController : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            var agentController = transform.root.GetComponent<Witch>();
            agentController.OnAction += ChangeAnimationState;
            var health = transform.root.GetComponentInChildren<Health>();
            health.OnDie += () =>
            {
                ChangeAnimationState("Death");
                agentController.enabled = false;
            };
        }

        private void OnDisable()
        {
            var agentController = transform.root.GetComponent<Witch>();
            agentController.OnAction -= ChangeAnimationState;

            var health = transform.root.GetComponentInChildren<Health>();
            health.OnDie -= () =>
            {
                ChangeAnimationState("Death");
                agentController.enabled = false;
            };
        }

        private void ChangeAnimationState(string action)
        {
            animator.Play(action, 0, 0.125f);
        }
    }
}
