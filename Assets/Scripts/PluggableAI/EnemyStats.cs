using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PluggableAI
{
	[CreateAssetMenu (menuName = "PluggableAI/EnemyStats")]	
	public class EnemyStats : ScriptableObject
	{
		public float moveSpeed = 1;
		public float lookRange = 40f;
		public float lookSphereCastRadius = 1f;

		public float attackRange = 1f;
		public float attackRate = 1f;
		public float attackForce = 15f;
		public int attackDamage = 50;

		public float searchDuration = 4f;
		public float searcingTurnSpeed = 120f;
	}
}