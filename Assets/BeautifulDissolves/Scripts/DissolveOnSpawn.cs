using UnityEngine;
using System.Collections;

namespace BeautifulDissolves {
	public class DissolveOnSpawn : Dissolve {

		void OnEnable()
		{
			TriggerDissolve();
		}
	}
}