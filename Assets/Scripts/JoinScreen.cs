using UnityEngine;
using System.Collections;

public class JoinScreen : MonoBehaviour {
	
	
	public GUISkin gSkin;
	// Use this for initialization
	void Start () {
		GlobalVars.numberOfPlayers = 0;
		GlobalVars.playerCharacters = new int[4];
		GlobalVars.playersJoined = new bool[4];
		GlobalVars.gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < 4; i++) {
			if(!GlobalVars.playersJoined[i] && Input.GetButtonDown("aButton" + (i + 1).ToString())) {
				GlobalVars.playersJoined[i] = true;
				GlobalVars.numberOfPlayers ++;
			} else if(GlobalVars.playersJoined[i] && Input.GetButtonDown("bButton" + (i + 1).ToString())) {
				GlobalVars.playersJoined[i] = false;
				GlobalVars.numberOfPlayers --;
			}
		}
		
		if(Input.GetButtonDown("startButton") && GlobalVars.numberOfPlayers > 1) {
			Application.LoadLevel("race");
		}
	}
	
	float GetXPos(int num) {
		if (num == 1) {
			return Screen.width * .5f;
		}else if (num == 3) {
			return Screen.width * .5f;
		}
		
		return 0f;
	}
	
	float GetYPos(int num) {
		if (num == 2) {
			return Screen.height * .5f;
		} else if (num == 3) {
			return Screen.height * .5f;
		}
		
		return 0f;
	}
	
	void OnGUI() {
		GUI.skin = gSkin;
		float leftMargin = Screen.width * .15f;
		float topMargin = Screen.height * .22f;
		for(int i = 0; i < 4; i++) {
			if(GlobalVars.playersJoined[i]) {
				GUI.Box(new Rect(GetXPos(i) + leftMargin, GetYPos(i) + topMargin, 300f, 100f), "HYPE");
			} else {
				GUI.Box(new Rect(GetXPos(i) + leftMargin, GetYPos(i) + topMargin, 300f, 100f), "Press A to join the hype!");
			}
		}
		
		if(GlobalVars.numberOfPlayers > 1) {
			GUI.Box(new Rect(Screen.width * .35f, Screen.height * .45f, 500f, 100f), "Press the START button to begin");
		}
	}
}
