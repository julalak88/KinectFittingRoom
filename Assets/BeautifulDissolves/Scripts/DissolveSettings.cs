using UnityEngine;
using System.Collections;

namespace BeautifulDissolves {
	[CreateAssetMenu(fileName = "DissolveSettings", menuName = "BeautifulDissolves/Settings", order = 1)]
	public class DissolveSettings : ScriptableObject {
		public bool Atomic = true;
		public bool DisableAfterDissolve = true;
		public AnimationCurve DissolveCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		[Range(0f, 1f)]
		public float DissolveStartPercent = 0f;
		public float Time = 4f;
		public float Speed = 1f;
	}
}
