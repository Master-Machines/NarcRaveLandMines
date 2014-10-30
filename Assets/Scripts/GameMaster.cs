using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
	public GameObject playerPrefab;
	private int[] playerPositions;
	private int guiState = -1;
	public GUISkin gSkin;
	
	private float left;
	private float right;
	// Use this for initialization
	void Start () {
		if(GlobalVars.numberOfPlayers == 2) {
			playerPositions = new int[2]{0, 1};
		} else if(GlobalVars.numberOfPlayers == 3) {
			playerPositions = new int[3]{0, 1, 2};
		} else if(GlobalVars.numberOfPlayers == 4) {
			playerPositions = new int[4]{0, 1, 2, 3};
		}
		randomizePlayerPositions();
		CreatePlayers();
		StartCoroutine(StartCountdown());
	}
	
	IEnumerator StartCountdown() {
		float waitTime = .5f;
		yield return new WaitForSeconds(waitTime);
		left = -.3f;
		right = .25f;
		guiState ++;
		yield return new WaitForSeconds(waitTime);
		left = -.3f;
		right = .25f;
		guiState ++;
		yield return new WaitForSeconds(waitTime);
		left = -.3f;
		right = .25f;
		guiState ++;
		waitTime += .25f;
		yield return new WaitForSeconds(waitTime);
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		for(int i = 0; i < players.Length; i++) {
			players[i].GetComponent<Movement>().isInit = true;
		}
		guiState = -1;
	
	}
	
	void CreatePlayers() {
		int counter = 0;
		for(int i = 0; i < 4; i++) {
			if(GlobalVars.playersJoined[i]) {
				counter ++;
				GameObject playa = (GameObject)Instantiate(playerPrefab, new Vector3(100f * playerPositions[counter - 1], 0f, 0f), Quaternion.identity);
				Movement m = playa.GetComponent<Movement>();
				m.player = i + 1;
				m.currentplace = GlobalVars.numberOfPlayers - playerPositions[counter - 1];
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		left += 2f * Time.deltaTime;
		right += 2f * Time.deltaTime;
		if(left > .25) {
			left = .25f;
		}
	}
	
	void randomizePlayerPositions(){
		int lengthOfArray = playerPositions.Length;
		for (int i = 0; i < lengthOfArray; i++) {
			int tempHolder = playerPositions[i];
			int spot = Random.Range (i, lengthOfArray);
			playerPositions[i] = playerPositions[spot];
			playerPositions[spot] = tempHolder;
		}
	}
	
	void OnGUI() {
		GUI.skin = gSkin;
		if(guiState == 0) {
			
			GUI.Box(new Rect(Screen.width * left, Screen.height * .4f, 1000f, 300f), "READY");
		}
		
		if(guiState == 1) {
			GUI.Box(new Rect(Screen.width * right, Screen.height * .4f, 1000f, 300f), "READY");
			GUI.Box(new Rect(Screen.width * left, Screen.height * .4f, 1000f, 300f), "SET");
		}
		
		if(guiState == 2) {
			GUI.Box(new Rect(Screen.width * right, Screen.height * .4f, 1000f, 300f), "SET");
			GUI.Box(new Rect(Screen.width * left, Screen.height * .4f, 1000f, 300f), "RAVE!");
		}
	}
}
