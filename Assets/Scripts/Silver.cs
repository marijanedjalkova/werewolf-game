using UnityEngine;
using System.Collections;

public class Silver : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var health = GameObject.Find ("Health");
		var player = GameObject.Find ("Player");
		bool isTransformed = player.transform;
		float distance = Vector3.Distance (player.transform.position, this.transform.position);
		var healthScript = health.GetComponent<Health>();

		if (isTransformed) {
			if (distance < 1) {
				healthScript.decreaseBy (0.1f);
			} else if (distance < 2) {
				healthScript.decreaseBy (0.05f);
			} else if (distance < 3) {
				healthScript.decreaseBy (0.025f);
			} else if (distance < 5) {
				healthScript.decreaseBy (0.001f);
			}
		}
	}

	public void SetLocation(Vector3 loc){
		this.transform.position = loc;
	}
	
}
