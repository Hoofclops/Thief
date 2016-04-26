using UnityEngine;
using System.Collections.Generic;

public class TagPainter : MonoBehaviour {

	public float mCameraSpeed = 1.0f;
	public int mWidth = 5;
	public int mHeight = 5;
	public TileGrid mGrid;
	public GameObject mDefaultTag;
	public GameObject mTraversableTag;
	public GameObject mTagHolder;

	private bool mActive;
	private GameObject mBrush;
	private List<GameObject> mTagGrid = new List<GameObject>();

	void Start () 
	{
		mWidth = mGrid.Width; 
		mHeight = mGrid.Height;
		BuildTagGrid (mWidth, mHeight, true);

		mBrush = mTraversableTag;
	}
		
	void Update () 
	{
		//Paint Tag
		if (Input.GetMouseButton (0)) {
			LeftMouseClick ();
		}

		//Erase Tag
		if(Input.GetMouseButton(1)) {
			RightMouseClick ();
		}

		//Move Camera
		float translationX = Input.GetAxis("Horizontal") * mCameraSpeed * Time.deltaTime;
		float translationY = Input.GetAxis("Vertical") * mCameraSpeed * Time.deltaTime;
		Camera.main.transform.Translate(translationX, translationY, 0);

		//Draw Grid
		for(int i = 0; i <= mWidth; i++){
			Debug.DrawLine (new Vector3 (i, 0, -5), new Vector3 (i, mHeight, -5), Color.white);
		}

		for (int j = 0; j <= mHeight; j++) {
			Debug.DrawLine (new Vector3 (0, j, -5), new Vector3 (mWidth, j, -5), Color.white);
		}
	}

	void BuildTagGrid(int width, int height, bool loadTagsFromGrid)
	{
		//Destroy all existing tags
		if (mTagGrid != null) {
			foreach (GameObject tag in mTagGrid) {
				Destroy (tag);
			}
		}

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				GameObject newTag = Instantiate(mDefaultTag, new Vector3(x+0.5f, y+0.5f, -10), mDefaultTag.transform.rotation) as GameObject;
				newTag.transform.parent = mTagHolder.transform;
				mTagGrid.Add (newTag);
			}
		}

		if (loadTagsFromGrid) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					if (mGrid.GetTile (x, y).Traversable) {
						PaintTag (mTraversableTag, x, y);
					} 
				}
			}
		}
	}

	void LeftMouseClick()
	{
		Tile selection;
		if (SelectTag (out selection)) {
			if (mBrush.GetComponent<Tag>().mName == "Traversable" && !selection.Traversable) {
				PaintTag (mBrush, selection.PosX, selection.PosY);
				selection.Traversable = true;
//				Debug.LogFormat ("Painting ({0},{1})", selection.PosX, selection.PosY);
			} 
		} 
	}

	void RightMouseClick()
	{
		Tile selection;
		if (SelectTag (out selection)) {
			int x = selection.PosX, y = selection.PosY;

			if(mTagGrid[PosToInd(x,y)].GetComponent<Tag>().mName == "Default") return;

			if (selection.Traversable)
				selection.Traversable = false;
			
			EraseTag (x,y);
			PaintTag (mDefaultTag,x,y);

//			Debug.LogFormat ("Erasing ({0},{1})",x, y);
		} 
	}

	bool SelectTag(out Tile selection)
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		int x = (int)Mathf.Floor (pos.x);
		int y = (int)Mathf.Floor (pos.y);

//		Debug.LogFormat ("Clicked {0},{1}", x, y);
		if (x >= 0 && x < mWidth && y >= 0 && y < mHeight) {
			selection = mGrid.GetTile(x,y);
			if(selection != null)
				return true;
		}
		selection = null;
		return false;
	}

	void PaintTag(GameObject tag, int x, int y){
		int index = PosToInd (x, y);
		if (mTagGrid [index] != null) {
			Destroy (mTagGrid [index]);
		}

		GameObject newTag = Instantiate(tag, new Vector3(x+0.5f, y+0.5f, -10), tag.transform.rotation) as GameObject;
		newTag.transform.parent = mTagHolder.transform;
		mTagGrid [index] = newTag;
	}

	void EraseTag(int x, int y){
		int index = PosToInd (x, y);
		if (mTagGrid [index] != null) {
			Destroy (mTagGrid [index]);
		}	
	}

	void OnGUI()
	{
		if (GUI.Button (new Rect (5, 5, 70, 30), "SaveGrid")) {
			mGrid.Width = mWidth;
			mGrid.Height = mHeight;
			GridData saveData = mGrid.PackageGrid ();
			IOManager.Instance.SaveGrid (saveData, mGrid.mGridName);
		}
		else if (GUI.Button (new Rect(5, 35, 70, 30), "BuildGrid")) {
			mGrid.BuildGrid (mWidth, mHeight);
			BuildTagGrid(mWidth, mHeight, false);
		}
	}

	int PosToInd(int x, int y)
	{	
		return (y * mWidth) + x;
	}
}
