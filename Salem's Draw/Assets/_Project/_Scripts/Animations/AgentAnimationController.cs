using UnityEngine;

namespace Salems_Draw
{
    [RequireComponent(typeof(Animator))]
    public class AgentAnimationController : MonoBehaviour
    {
        private Witch agentController;
        private Animator animator;

        private void Awake()
        {
            agentController = transform.root.GetComponent<Witch>();
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            agentController.OnAction += ChangeAnimationState;
        }

        private void OnDisable()
        {
            agentController.OnAction -= ChangeAnimationState;
        }

        private void ChangeAnimationState(string action)
        {
            animator.Play(action, 0, 0.125f);
        }
    }
}
