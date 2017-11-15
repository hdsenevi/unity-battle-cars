using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PluggableAI
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
    public class AttackAction : Action
    {
        public override void Act(StateController controller)
        {
            Attack(controller);
        }

        private void Attack(StateController controller)
        {
            // Is object infront of agent
            RaycastHit hit;

            Debug.DrawRay (controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.attackRange, Color.red);

            var sphereCastResult = Physics.SphereCast(
                controller.eyes.position,
                controller.enemyStats.lookSphereCastRadius,
                controller.eyes.forward,
                out hit,
                controller.enemyStats.attackRange
            );
            if (sphereCastResult && hit.collider.CompareTag("Player"))
            {
                if (controller.CheckIfCountDownElapsed(controller.enemyStats.attackRate)) {
					controller.tankShooting.Fire(controller.enemyStats.attackForce, controller.enemyStats.attackRate);
				}
            }
        }
    }
}