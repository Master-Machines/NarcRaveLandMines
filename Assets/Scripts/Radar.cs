using UnityEngine;
using System.Collections;

public class Radar : MonoBehaviour {
	
	private GameObject[] players;
	private float[] positions;
	
	private Texture2D tex;
	public GUISkin gSkin;
	private float tenth;
	private float halfWayTop;
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(1f);
		tenth = 0f;
		halfWayTop = Screen.height * .5f;
		players = GameObject.FindGameObjectsWithTag("Player");
		positions = new float[players.Length];
		tex = new Texture2D(1,1);
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < players.Length; i++) {
			positions[i] = players[i].transform.position.x / GlobalVars.goalXPosition;
		}
	}
	
	void OnGUI() {
		GUI.skin = gSkin;
		tex.SetPixel(0, 0, new Color(1, 1, 1, .5f));
		tex.Apply();
		gSkin.box.normal.background = tex;
		GUI.Box(new Rect(tenth, halfWayTop - 13, Screen.width, 26), "");
		
		for(int i = 0; i < players.Length; i++) {
			tex.SetPixel(0, 0, GlobalVars.IntToColor(GlobalVars.playerCharacters[i]));
			tex.Apply();
			gSkin.box.normal.background = tex;
			GUI.Box(new Rect(tenth + Screen.width * (positions[i]), halfWayTop - 13, 3, 26), "");
		}
		
	}
}
