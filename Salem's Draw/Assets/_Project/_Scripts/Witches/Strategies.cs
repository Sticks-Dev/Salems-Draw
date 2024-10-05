using Kickstarter.GOAP;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Salems_Draw
{
    public class IdleStrategy : IActionStrategy
    {
        public bool CanPerform => true;
        public bool Complete { get; private set; }

        private CountdownTimer timer;

        public IdleStrategy(float duration)
        {
            timer = new CountdownTimer(duration);
            timer.OnTimerStart += () => Complete = false;
            timer.OnTimerStop += () => Complete = true;
        }

        public void Start()
            => timer.Start();

        public void Update(float deltaTime)
            => timer.Tick(deltaTime);
    }

    public class WanderStrategy : IActionStrategy
    {
        readonly NavMeshAgent agent;
        readonly float wanderRadius;

        public bool CanPerform => !Complete;
        public bool Complete => agent.remainingDistance <= 2f && !agent.pathPending;

        public WanderStrategy(NavMeshAgent agent, float wanderRadius)
        {
            this.agent = agent;
            this.wanderRadius = wanderRadius;
        }

        public void Start()
        {
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

        public MoveStrategy(NavMeshAgent agent, Func<Vector3> destination)
        {
            this.agent = agent;
            this.destination = destination;
        }

        public void Start()
            => agent.SetDestination(destination());

        public void Update(float deltaTime)
            => agent.SetDestination(destination());

        public void Stop()
            => agent.ResetPath();
    }

    public class AttackStrategy : IActionStrategy
    {
        public bool CanPerform => true;
        public bool Complete { get; private set; }

        private GameObject targetGameObject;

        readonly private Func<GameObject> target;
        readonly private float attackCooldown;

        private bool canAttack;

        public AttackStrategy(Func<GameObject> getTarget, float attackCooldown)
        {
            target = getTarget;
            this.attackCooldown = attackCooldown;
        }

        private CountdownTimer timer;

        public void Start()
        {
            timer = new CountdownTimer(attackCooldown);
            timer.OnTimerStart += () =>
            {
                Complete = false;
                AttackTarget();
            };
            timer.OnTimerStop += () => Complete = true;
            timer.Start();
        }

        public void Update(float deltaTime)
        {
            timer.Tick(deltaTime);
        }

        private void AttackTarget()
        {
            Debug.Log("Attacking target");
        }
    }
}
