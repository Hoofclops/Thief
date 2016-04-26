using UnityEngine;
using System.Collections;

public enum GameMode{
	TAG_PAINT, PLAY, TEST,
}

public class GameModeManager : MonoBehaviour {
	private static GameModeManager instance;
	public static GameModeManager Instance{ get { return instance; } }
	void Awake()
	{
		if (instance != null && instance != this) {
			Destroy (gameObject);
		}
		else
			instance = this;
	}

	public GameMode mMode;

	void Start () 
	{
		if (mMode == GameMode.TAG_PAINT) {
			gameObject.GetComponent<TagPainter> ().enabled = true;
		} 
		else if (mMode == GameMode.PLAY) {
			gameObject.GetComponent<TagPainter> ().enabled = false;
		}
	}
}
