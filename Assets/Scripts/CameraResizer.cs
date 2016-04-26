using UnityEngine;
using System.Collections;

public class CameraResizer : MonoBehaviour {
	public int spriteSize = 32;

	// Use this for initialization
	void Start () {
		float newSize = Screen.height / (float)spriteSize / 2.0f;
		Camera.main.orthographicSize = newSize;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
