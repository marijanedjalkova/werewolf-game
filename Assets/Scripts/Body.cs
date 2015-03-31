using UnityEngine;
using System.Collections;

public class Body : MonoBehaviour {


	public AudioClip[] deathSounds;

	void Start () {
	
		AudioSource s = this.GetComponent<AudioSource> ();
		s.pitch = Random.Range (0.9f,1.0f);
		s.volume = Random.Range (0.9f, 1.0f);
		AudioClip c = deathSounds[Random.Range (0, deathSounds.Length)];
		s.PlayOneShot (c, 1.0f);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
