using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PluggableAI
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
    public class LookDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            bool targetVisible = Look(controller);
            return targetVisible;
        }

        private bool Look(StateController controller)
        {
            // Is object infront of agent
            RaycastHit hit;

			Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.lookRange, Color.green);

            var sphereCastResult = Physics.SphereCast(
                controller.eyes.position,
                controller.enemyStats.lookSphereCastRadius,
                controller.eyes.forward,
                out hit,
                controller.enemyStats.lookRange
            );

            if (sphereCastResult && hit.collider.CompareTag("Player"))
            {
                controller.chaseTarget = hit.transform;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
