using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class IOManager : MonoBehaviour {
	private static IOManager instance;
	public static IOManager Instance{ get { return instance; } }
	void Awake()
	{
		if (instance != null && instance != this) {
			Destroy (gameObject);
		}
		else
			instance = this;
	}

	public void SaveGrid(GridData data, string gridName)
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/" + gridName + ".dat", FileMode.OpenOrCreate);
		bf.Serialize (file, data);
		file.Close ();
	}

	public bool LoadGrid(out GridData data, string gridName)
	{
		string path = Application.persistentDataPath + "/" + gridName + ".dat";
		if (File.Exists (path)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (path, FileMode.Open);
			data = (GridData)(bf.Deserialize (file));
			return true;
		} 
		else {
			data = null;
			return false;
		}
	}
}

[Serializable]
public class GridData
{
	private int mWidth;
	private int mHeight;
	private List<bool> mTraversableMap;
	private List<TileType> mTypeMap;

	public GridData(int w, int h, List<bool> travMap, List<TileType> tileMap)
	{
		mWidth = w;
		mHeight = h;
		mTraversableMap = travMap;
		mTypeMap = tileMap;
	}

	public int Width{
		get{ return mWidth; }
	}

	public int Height{
		get{ return mHeight; }
	}

	public List<bool> TravMap{
		get{ return mTraversableMap; }
	}

	public List<TileType> TypeMap{
		get{ return mTypeMap; }
	}
}