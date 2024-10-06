using Kickstarter.GOAP;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Salems_Draw
{
    public class IdleStrategy : IActionStrategy
    {
        public bool CanPerform => true;
        public bool Complete { get; private set; }

        private CountdownTimer timer;
        private Action<string> onIdle;

        public IdleStrategy(float duration, Action<string> onIdle)
        {
            timer = new CountdownTimer(duration);
            timer.OnTimerStart += () => Complete = false;
            timer.OnTimerStop += () => Complete = true;
            this.onIdle = onIdle;
        }

        public void Start()
        {
            timer.Start();
            onIdle?.Invoke("Idle");
        }

        public void Update(float deltaTime)
            => timer.Tick(deltaTime);
    }

    public class WanderStrategy : IActionStrategy
    {
        readonly NavMeshAgent agent;
        readonly float wanderRadius;

        public bool CanPerform => !Complete;
        public bool Complete => agent.remainingDistance <= 2f && !agent.pathPending;

        private event Action<string> onWander;

        public WanderStrategy(NavMeshAgent agent, float wanderRadius, Action<string> onWander)
        {
            this.agent = agent;
            this.wanderRadius = wanderRadius;
        }

        public void Start()
        {
            onWander?.Invoke("Wandering");
            for (int i = 0; i < 5; i++)
            {
                Vector3 randomDirection = (UnityEngine.Random.insideUnitSphere * wanderRadius).With(y: 0);
                NavMeshHit hit;

                if (NavMesh.SamplePosition(agent.transform.position + randomDirection, out hit, wanderRadius, 1))
                {
                    agent.SetDestination(hit.position);
                    return;
                }
            }
        }
    }

    public class MoveStrategy : IActionStrategy
    {
        private readonly NavMeshAgent agent;
        private readonly System.Func<Vector3> destination;

        public bool CanPerform => !Complete;
        public bool Complete => agent.remainingDistance <= 2f && !agent.pathPending;

        private Action<string> onMove;

        public MoveStrategy(NavMeshAgent agent, Func<Vector3> destination, Action<string> onMove)
        {
            this.agent = agent;
            this.destination = destination;
            this.onMove = onMove;
        }

        public void Start()
        {
            agent.SetDestination(destination());
            onMove?.Invoke("Chasing");
        }

        public void Update(float deltaTime)
            => agent.SetDestination(destination());

        public void Stop()
            => agent.ResetPath();
    }

    public class SpellStrategy : IActionStrategy
    {
        public bool CanPerform => true;
        public bool Complete { get; private set; }

        private readonly Action<string> onCastSpell;

        readonly private Func<GameObject> target;
        readonly private float attackCooldown;
        readonly private Func<Coroutine> spell;
        readonly private Transform transform;

        public SpellStrategy(Func<GameObject> getTarget, float attackCooldown, Func<Coroutine> spell, Action<string> onCastSpell, Transform transform)
        {
            target = getTarget;
            this.attackCooldown = attackCooldown;
            this.spell = spell;
            this.onCastSpell = onCastSpell;
            this.transform = transform;
        }

        public void Start()
        {
            onCastSpell?.Invoke("Casting Spell");
            CoroutineHelper.Instance.StartCoroutine(ExecuteSpell());
        }

        public void Update(float deltaTime)
        {
            // Assuming you have a reference to the transform and the target GameObject
            transform.LookAt(target()?.transform ?? transform);

        }

        private IEnumerator ExecuteSpell()
        {
            Complete = false;
            AttackTarget();
            yield return spell();
            Complete = true;
        }

        private void AttackTarget()
        {
            Debug.Log("Attacking target");
        }
    }
}
