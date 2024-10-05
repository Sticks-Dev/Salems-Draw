using Kickstarter.GOAP;
using System.Collections.Generic;
using UnityEngine;

namespace Salems_Draw
{
    public class Witch : GoapAgent
    {
        [SerializeField] private float maxHealth;

        private float health;

        protected override void SetupBeliefs()
        {
            beliefs = new Dictionary<string, AgentBelief>();
            BeliefFactory factory = new BeliefFactory(this, beliefs);

            factory.AddBelief("Nothing", () => false);
            factory.AddBelief("Healthy", () => health >= maxHealth / 2);
        }

        protected override void SetupActions()
        {
            actions = new HashSet<AgentAction>();

            /*
                actions.Add(new AgentAction.Builder("Relax")
                    .WithStrategy(new IdleStrategy(5))
                    .AddEffect(beliefs["Nothing"])
                    .Build());
             */
        }

        protected override void SetupGoals()
        {
            goals = new HashSet<AgentGoal>();

            goals.Add(new AgentGoal.Builder("Chill Out")
                .WithPriority(1)
                .WithDesiredEffect(beliefs["Nothing"])
                .Build());
        }

        protected override void UpdateStats()
        {
            health += InRangeOf(transform.position, 0f) ? 20 : 0;
            health = Mathf.Clamp(health, 0, 100);
        }
    }
}
