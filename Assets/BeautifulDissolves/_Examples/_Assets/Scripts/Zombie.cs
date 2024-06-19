using UnityEngine;
using System.Collections;

namespace BeautifulDissolves {
	public class Zombie : MonoBehaviour {

		[SerializeField]
		private Animator m_Animator;
		[SerializeField]
		private AudioSource m_Audio;
		[SerializeField]
		private ParticleSystem m_DeathParticles;

		[SerializeField]
		private Dissolve m_Dissolve;

		// Click zombie to kill
		void OnMouseDown()
		{
			GetComponent<Collider>().enabled = false;
			m_Animator.SetTrigger("Dead");
			m_Audio.Play();
			m_DeathParticles.Play();

			Invoke ("InvokeDissolve", 1f);
		}

		void InvokeDissolve()
		{
			m_Dissolve.TriggerDissolve ();
		}

		public void DestroySelf()
		{
			Destroy(gameObject);
		}
	}
}
