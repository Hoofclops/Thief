using UnityEngine;
using System.Collections;

public class FOVDetection : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") {
			Debug.Log ("Player Detected");
		}
	}
}
