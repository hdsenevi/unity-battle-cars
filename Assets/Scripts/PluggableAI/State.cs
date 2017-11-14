﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PluggableAI
{
	[CreateAssetMenu (menuName = "PluggableAI/State")]
	public class State : ScriptableObject
	{
		public Action[] actions;
		public Color sceneGizmoColor = Color.gray;

		public void UpdateState (StateController controller)
		{

		}

		private void DoActions (StateController controller)
		{
			for (int i = 0; i < actions.Length; i++) {
				actions [i].Act (controller);
			}
		}
	}
}