using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {

	public float mSpeed = 1.0f;

	void Update() {
		float translationX = Input.GetAxis("Horizontal") * mSpeed * Time.deltaTime;
		float translationY = Input.GetAxis("Vertical") * mSpeed * Time.deltaTime;
		transform.Translate(translationX, translationY, 0);
	}
}
