using UnityEngine;
using System.Collections;

public class JoinScreen : MonoBehaviour {
	
	
	public GUISkin gSkin;
	public string[] classNames;
	public string[] classDescriptions;
	
	public Color activeColor;
	public Color inactiveColor;
	
	private int[] playerGuiStates = new int[4];
	private int[] playerCurrentSelections = new int[4];
	private bool[] playerCanChange = new bool[4]{true, true, true, true};
	
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
			if(playerCanChange[i] && playerGuiStates[i] == 1 && Mathf.Abs(Input.GetAxis("VertialDPad" + (i+ 1).ToString())) > .2f) {
				if(Input.GetAxis("VertialDPad" +  (i+ 1).ToString()) > 0) {
					playerCurrentSelections[i] --;
					StartCoroutine(StopPlayerFromChanging(i));
					if(playerCurrentSelections[i] < 0) {
						playerCurrentSelections[i] = 0;
					}
				} else {
					playerCurrentSelections[i] ++;
					StartCoroutine(StopPlayerFromChanging(i));
					if(playerCurrentSelections[i] > classNames.Length - 1) {
						playerCurrentSelections[i] = classNames.Length - 1;
					}
				}
			}
			if(Input.GetButtonDown("aButton" + (i + 1).ToString())) {
				if(playerGuiStates[i] == 1) {
					GlobalVars.playersJoined[i] = true;
					GlobalVars.numberOfPlayers ++;
					playerGuiStates[i] ++;
					GlobalVars.playerClasses[i] = playerCurrentSelections[i];
				} else if(playerGuiStates[i] == 0) {
					playerGuiStates[i] ++;
				}
				
			} 
			
			
			if(Input.GetButtonDown("bButton" + (i + 1).ToString())) {
				if(playerGuiStates[i] == 2) {
					playerGuiStates[i] --;
					GlobalVars.playersJoined[i] = false;
					GlobalVars.numberOfPlayers --;
				} else if (playerGuiStates[i] == 1) {
					playerGuiStates[i] --;
				}
			}
		}
		
		if(Input.GetButtonDown("startButton") && GlobalVars.numberOfPlayers > 1) {
			Application.LoadLevel("race");
		}
	}
	
	IEnumerator StopPlayerFromChanging(int playerNum) {
		playerCanChange[playerNum] = false;
		yield return new WaitForSeconds(.2f);
		playerCanChange[playerNum] = true;
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
		gSkin.box.normal.textColor = inactiveColor;
		float leftMargin = Screen.width * .15f;
		float topMargin = Screen.height * .22f;
		for(int i = 0; i < 4; i++) {
			if(playerGuiStates[i] == 2) {
				GUI.Box(new Rect(GetXPos(i) + leftMargin, GetYPos(i) + topMargin, 300f, 100f), "HYPE");
			} else if(playerGuiStates[i] == 1) {
				//Class selection
				GUI.Box (new Rect(GetXPos(i) + leftMargin, GetYPos(i) + (30f), 200f, 75), "Select a class");
				for(int g = 0; g < classNames.Length; g++) {
					if(playerCurrentSelections[i] == g) {
						gSkin.box.normal.textColor = activeColor;
						GUI.Box (new Rect(GetXPos(i) + leftMargin / 2f, GetYPos(i) + (topMargin / 2.5f) * (g + 1), 200f, 75), classNames[g]);
						gSkin.box.normal.textColor = inactiveColor;
						GUI.Box (new Rect(GetXPos(i) + leftMargin * 1.25f, GetYPos(i) + (topMargin / 2.5f), 350f, 300f), classDescriptions[g]);
					} else {
						GUI.Box (new Rect(GetXPos(i) + leftMargin / 2f, GetYPos(i) + (topMargin / 2.5f) * (g + 1), 200f, 75), classNames[g]);
					}
					
				}
				
			} else {
				GUI.Box(new Rect(GetXPos(i) + leftMargin, GetYPos(i) + topMargin, 300f, 100f), "Press A to join the hype!");
			}
		}
		
		if(GlobalVars.numberOfPlayers > 1) {
			GUI.Box(new Rect(Screen.width * .35f, Screen.height * .45f, 500f, 100f), "Press the START button to begin");
		}
	}
}
