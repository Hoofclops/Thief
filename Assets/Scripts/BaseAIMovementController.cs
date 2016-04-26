using UnityEngine;
using System.Collections.Generic;

public enum Direction
{
	UP, LEFT, DOWN, RIGHT
}

public class BaseAIMovementController : MonoBehaviour {

	public float mSpeed = 1.0f;
	public float mWaitTime = 2.0f;
	public float mWayPointBuffer = 0.1f;
	public GameObject mPathObject;
	public Direction mMovDir = Direction.UP;

	protected List<GameObject> mWayPoints = new List<GameObject>();

	private int mWayPointIndex = 0;
	private float mWaitTimer = 0.0f;

	void Start()
	{
		foreach (Transform wayPoint in mPathObject.transform) {
			mWayPoints.Add (wayPoint.gameObject);
		}
	}

	void Update()
	{
		if (mWaitTimer > 0) {
			mWaitTimer -= Time.deltaTime;
			return;
		} 
			
		float dist = Vector3.Distance (mWayPoints [mWayPointIndex].transform.position, transform.position);
		if (dist < mWayPointBuffer) {
			transform.position = mWayPoints [mWayPointIndex].transform.position;
			mWaitTimer = mWaitTime;
			mWayPointIndex = mWayPointIndex == mWayPoints.Count - 1 ? 0 : mWayPointIndex + 1;
		}
		else {
			MoveToWayPoint (mWayPoints[mWayPointIndex]);
		}
	}

	void MoveToWayPoint (GameObject wayPoint)
	{
		Vector3 dir = Vector3.Normalize(wayPoint.transform.position - transform.position);
		dir *= Time.deltaTime * mSpeed;
		dir.z = 0;
		transform.Translate (dir);

		UpdateDirection ();
	}

	void UpdateDirection()
	{
		Vector3 toPos = mWayPoints [mWayPointIndex].transform.position;
		Vector3 curPos = transform.position;
		float xDiff = toPos.x - curPos.x;
		float yDiff = toPos.y - curPos.y;

		if (Mathf.Abs (xDiff) >= Mathf.Abs (yDiff)) {
			if (xDiff > mWayPointBuffer)
				mMovDir = Direction.RIGHT;
			else if (xDiff < -mWayPointBuffer)
				mMovDir = Direction.LEFT;
		}
		else {
			if (yDiff > mWayPointBuffer)
				mMovDir = Direction.UP;
			else if (yDiff < -mWayPointBuffer)
				mMovDir = Direction.DOWN;
		}
	}
}
