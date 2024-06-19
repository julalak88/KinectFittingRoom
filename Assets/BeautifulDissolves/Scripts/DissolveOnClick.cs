using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace BeautifulDissolves {
	public class DissolveOnClick : Dissolve {

		void OnMouseDown()
		{
			TriggerDissolve();
		}
	}
}