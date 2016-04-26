using UnityEngine;
using System.Collections;

public class PathInfo : MonoBehaviour {

	public bool mShowWayPoints = false;
	public Color mWayPointColor = Color.green;

	// Use this for initialization
	void Start () {
		//visualize waypoints or not
		foreach (Transform child in transform) {
			child.GetComponent<SpriteRenderer> ().enabled = mShowWayPoints;
			child.GetComponent<SpriteRenderer> ().color = mWayPointColor;
		}
	}
}
