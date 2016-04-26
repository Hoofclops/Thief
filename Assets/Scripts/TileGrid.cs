using UnityEngine;
using System.Collections.Generic;

public enum TileType
{
	FLOOR, WALL, EMPTY
}

public class Tile 
{
	private int posX;
	private int posY;
	private bool traversable;
	private TileType type;

	public Tile(int x, int y)
	{
		posX = x;
		posY = y;
	}

	public Tile(int x, int y, bool tr, TileType ty)
	{
		posX = x;
		posY = y;
		traversable = tr;
		type = ty;
	}

	public int PosX{
		get{return posX;}
	}
	public int PosY{
		get{return posY;}
	}
	public TileType Type{
		get{return type;}
		set{type = value;}
	}
	public bool Traversable{
		get{return traversable;}
		set{traversable = value;}
	}
}

public class TileGrid : MonoBehaviour {

	public string mGridName;

	private int mWidth = 5;
	public int Width{ get { return mWidth; } set { mWidth = value; } }
	private int mHeight = 5;
	public int Height{ get { return mHeight; } set { mHeight = value; } }
//	private bool mLoadSuccesful = false;
//	public bool LoadSuccesful{ get { return mLoadSuccesful; } }

	private List<Tile> mTileMap;
	private List<bool> mTravMap;
	private List<TileType> mTypeMap;

	void Start()
	{
		mTileMap = new List<Tile> ();
		if (mGridName == "") {
			Debug.LogError ("Grid name must be specified");
			Debug.Break ();
		}
		//attempt to load data
		GridData data;
		if (IOManager.Instance.LoadGrid (out data, mGridName)) {
			mWidth = data.Width;
			mHeight = data.Height;
			BuildGrid (mWidth, mHeight, data.TravMap, data.TypeMap);
		} 
		else if (GameModeManager.Instance.mMode == GameMode.TAG_PAINT) {
			BuildGrid (mWidth, mHeight);
		} 
		else {
			Debug.LogError ("Grid data failed to load");
			Debug.Break ();
		}
	}

	public void BuildGrid(int width, int height)
	{
		if (mTileMap != null)
			mTileMap = new List<Tile> ();

		mWidth = width;
		mHeight = height;
		
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				mTileMap.Add (new Tile (x, y)); 	
			}
		}
	}

	public void BuildGrid(int width, int height, List<bool> travMap, List<TileType> typeMap)
	{
		if (mTileMap != null)
			mTileMap = new List<Tile> ();

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				mTileMap.Add (new Tile (x, y, travMap[PosToInd (x, y)], typeMap[PosToInd (x, y)])); 	
			}
		}
	}

	public Tile GetTile(int x, int y)
	{
		return mTileMap [PosToInd (x, y)];
	}

	public GridData PackageGrid()
	{
		List<bool> travMap = new List<bool> ();
		List<TileType> tileMap = new List<TileType> ();

		int len = mWidth * mHeight;
		for (int i = 0; i < len; i++) {
			travMap.Add (mTileMap [i].Traversable);
			tileMap.Add (mTileMap [i].Type);
		}

		return new GridData (mWidth, mHeight, travMap, tileMap);
	}

	int PosToInd(int x, int y)
	{	
		return (y * mWidth) + x;
	}

	void IndToPos(int index, out int x, out int y)
	{
		x = index % mWidth;
		y = index / mWidth;
	}
}