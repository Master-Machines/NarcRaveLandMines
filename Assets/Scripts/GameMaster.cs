using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
	public GameObject playerPrefab;
	private int[] playerPositions;
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
	}
	
	void CreatePlayers() {
		int counter = 0;
		for(int i = 0; i < 4; i++) {
			if(GlobalVars.playersJoined[i]) {
				counter ++;
				GameObject playa = (GameObject)Instantiate(playerPrefab, new Vector3(100f * playerPositions[counter - 1], 0f, 0f), Quaternion.identity);
				Movement m = playa.GetComponent<Movement>();
				m.player = i + 1;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
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
}
