using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BeautifulDissolves {
	public class Dissolve : MonoBehaviour {
		
		[SerializeField]
		protected DissolveSettings m_DissolveSettings;

		public UnityEvent OnDissolveStart;
		public UnityEvent OnDissolveFinish;

		protected Material[] m_Materials;
		protected IEnumerator m_CurrentCoroutine;
		protected bool m_Dissolving;

		private bool m_CanDissolve = true;

		protected virtual Material[] GetMaterials()
		{
			// Finds the first renderer on itself or on any of its children
			Renderer renderer = GetComponentInChildren<Renderer>();

			return renderer != null ? renderer.materials : null;
		}

		void Awake()
		{
			m_Materials = GetMaterials ();
		}

		public void TriggerDissolve()
		{
			TriggerDissolve(m_DissolveSettings);
		}

		public void TriggerDissolve(DissolveSettings settings)
		{
			if (m_DissolveSettings == null) {
				Debug.LogWarning ("No dissolve settings found for gameobject: " + transform.name);
				return;
			}

			TriggerDissolve (settings.Atomic, settings.DisableAfterDissolve, settings.DissolveCurve, settings.DissolveStartPercent, settings.Time, settings.Speed);
		}

		public void TriggerReverseDissolve()
		{
			TriggerDissolve (m_DissolveSettings.Atomic, m_DissolveSettings.DisableAfterDissolve, m_DissolveSettings.DissolveCurve, 1f, m_DissolveSettings.Time, -m_DissolveSettings.Speed);
		}

		public void TriggerDissolve(bool atomic, bool disableAfterDissolve, AnimationCurve dissolveCurve, float dissolveStartPercent, float time, float speed)
		{
			if ((atomic && m_Dissolving) || !m_CanDissolve) {
				return;
			}

			if (m_Materials != null && m_Materials.Length > 0) {
				m_Dissolving = true;
				InvokeDissolveStartEvents ();
				StartCoroutine(DissolveHelper.CurveDissolve(m_Materials, dissolveCurve, time, dissolveStartPercent, speed, () => {
					m_Dissolving = false;
					InvokeDissolveEndEvents();
				}));
			}

			if (disableAfterDissolve) {
				m_CanDissolve = false;
			}
		}

		private void InvokeDissolveStartEvents() {
			// Dispatch dissolve start event
			if (OnDissolveStart != null) {
				OnDissolveStart.Invoke();
			}
		}

		private void InvokeDissolveEndEvents() {
			// Dispatch dissolve finished event
			if (OnDissolveFinish != null) {
				OnDissolveFinish.Invoke();
			}
		}

		void OnDestroy()
		{
			if (m_Materials != null) {
				// Clean up material instance
				for (int i = 0; i < m_Materials.Length; i++) {
					if (m_Materials[i] != null) {
						Destroy(m_Materials[i]);
					}
				}
			}
		}
	}
}
