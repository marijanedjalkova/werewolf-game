using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	public AudioClip introClip;
	public AudioClip loopClip;

	// Use this for initialization
	public void StartMusic () {
		StartCoroutine(PlayMusic ());
	}
	
	// Update is called once per frame
	IEnumerator PlayMusic () {
		audio.clip = introClip;
		audio.loop = false;
		audio.Play();
		yield return new WaitForSeconds(4.8f);
		audio.clip = loopClip;
		audio.loop = true;
		audio.Play ();
	}
}
