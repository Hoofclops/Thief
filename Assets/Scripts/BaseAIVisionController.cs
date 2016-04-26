using UnityEngine;
using System.Collections;

public class BaseAIVisionController : MonoBehaviour {

	public BaseAIMovementController mMovController;
	public GameObject mVisionObj;
	public float mRotSpeed = 1.5f;
	public float mRotBuffer = 0.01f;
	private bool mRotating = false;
	private Direction mCurDir = Direction.UP;
	private int[] mVisionAngles = { 0, 90, 180, 270 };

	// Update is called once per frame
	void Update () {
		//if rotation has been achieved stop rotating, else keep rotating, else check direction has changed or not
		if (mRotating) {
			int desiredAngle = mVisionAngles [(int)mMovController.mMovDir];
			Vector3 curVect = mVisionObj.transform.eulerAngles;
			Vector3 toVect = new Vector3 (0, 0, desiredAngle);
			//avoid rotating the long way around
			if(curVect.z < desiredAngle) curVect.z += 360;

			if(Vector3.Distance(curVect, toVect) <= mRotBuffer) {
				mVisionObj.transform.eulerAngles = toVect;
				mRotating = false;
			}
			else {
				mVisionObj.transform.eulerAngles = Vector3.Lerp (curVect, toVect, Time.deltaTime * mRotSpeed);
			}
		}

		if (mCurDir != mMovController.mMovDir) {
			mRotating = true;
			mCurDir = mMovController.mMovDir;
		}
	}
}
