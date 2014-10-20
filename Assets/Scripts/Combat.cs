using UnityEngine;
using System.Collections;
using System;

public class Combat : MonoBehaviour {

	[HideInInspector]
	public int playerNumber;
	private bool endingCombat = false;
	[HideInInspector]
	public int otherPlayerNumber;
	private float endTime;
	private float runTime;
	[HideInInspector]
	public float score = 0.0f;
	private bool started = false;
	public Texture2D xButton;
	public Texture2D yButton;
	public Texture2D aButton;
	public Texture2D bButton;
	private Texture2D clearTex;
	private string currentButton;
	private float counter;
	private int randomNumber;
	public GUISkin gStyle;
	private float startingX = 0f;
	private float startingY = 0f;
	private int lastNumber = -1;
	[HideInInspector]
	public Movement movement;
	public Combat otherPlayerCombat;
	private int[] nextButtons;
	private int currentButtonIndex;
	private float minusAmount;
	private float guiDeltaAmount = .15f;
	private int correcto = 0;
	public Texture2D correct;
	public Texture2D incorrect;
	void Start() {
		clearTex = new Texture2D (1, 1);
		clearTex.SetPixel (0, 0, new Color (0,0,0, 0));
		clearTex.Apply ();
	}
	// Use this for initialization
	public void StartMatch (float run) {//you get the player number and how long the match will be
		runTime = run;
		minusAmount = guiDeltaAmount;
		loadNextButton();
		endTime = Time.time + run;
		//load up combat scene, maybe attach this to the combat scene
		if (playerNumber == 1) {
		} else if (playerNumber == 2) {
			startingX = Screen.width * .5f;
		} else if (playerNumber == 3) {
			startingY = Screen.height * .5f;
		} else if (playerNumber == 4) {
			startingX = Screen.width * .5f;
			startingY = Screen.height * .5f;
		}
		started = true;
	}

	void loadNextButton(){
		nextButtons = new int[100];
		for(int i = 0; i < 100; i++) {
			randomNumber = UnityEngine.Random.Range (0, 4);
			while (randomNumber == lastNumber) {
				randomNumber = UnityEngine.Random.Range (0, 4);
			}
			lastNumber = randomNumber;
			nextButtons[i] = randomNumber;
		}
		convertIntToButtonString(nextButtons[currentButtonIndex]);
		
	}
	void convertIntToButtonString(int num){
		switch (num) {
		case 0:
			//button A
			currentButton = "aButton" + playerNumber.ToString();
			break;
		case 1:
			//button B
			currentButton = "bButton" + playerNumber.ToString();
			break;
		case 2:
			//button X
			currentButton = "xButton" + playerNumber.ToString();
			break;
		case 3:
			//button Y
			currentButton = "yButton" + playerNumber.ToString();
			break;
		default:
			print ("you fucking broke it");
			break;
		}
	}
	
	string ConvertIntToStringForReal(int num) {
		switch (num) {
		case 0:
			//button A
			return ("a");
			break;
		case 1:
			//button B
			return ("b");
			break;
		case 2:
			//button X
			return ("x");
			break;
		case 3:
			//button Y
			return ("y");
			break;
		default:
			return ("a");
			break;
		}
	}

	void endCombat(){
		currentButton = "None";
		if(score > otherPlayerCombat.score) {
			movement.ExitCombat(true);
		} else {
			movement.ExitCombat(false);
		}
		//remove current button on screen
		//check score vs other player
		//get score from other object, compare to this one
		//display winner or loser
		//tell parent object if its a winner or loser
		Destroy (this);
	}
	// Update is called once per frame
	void Update () {
		if(started) {
			if (Time.time > endTime && !endingCombat) {
				if(score == otherPlayerCombat.score) {
					endTime += .5f;
				} else {
					endingCombat = true;
					endCombat();
				}
			}
			if (Input.GetButtonDown("aButton" + playerNumber.ToString()) || Input.GetButtonDown("bButton" + playerNumber.ToString()) || Input.GetButtonDown("xButton" + playerNumber.ToString()) || Input.GetButtonDown("yButton" + playerNumber.ToString())) { //this will be for if the buttons are a b x y of the controler you are using
				if (Input.GetButtonDown(currentButton)) {//compare button down vs current button
					Debug.Log("Correct for player " + playerNumber);
					StartCoroutine (CorrectIncorrect(true));
				} else {
					Debug.Log("incorrect for player " + playerNumber);
					StartCoroutine (CorrectIncorrect(false));
				}
			}
		}
		if(minusAmount > 0) {
			minusAmount -=  20;
		}
		if(minusAmount < 0) {
			minusAmount = 0;
		}
	}
	
	IEnumerator CorrectIncorrect(bool c) {
		if(c) {
			score++;
			correcto = 1;
		} else {
			score--;
			correcto = 2;
		}
		yield return new WaitForSeconds(.1f);
		currentButtonIndex ++;
		convertIntToButtonString(nextButtons[currentButtonIndex]);
		minusAmount += 90;
		yield return new WaitForSeconds(.12f);
		correcto = 0;
	}
	
	void OnGUI(){
		if(started) {
			GUI.skin = gStyle;
			gStyle.box.normal.background = clearTex;
			float leftIconWidth = 80;
			float leftMargin = Screen.width * .05f;
			float topMargin = -90f;
			for(int i = 0; i < 4; i++) {
				topMargin += 90;
				string letter = ConvertIntToStringForReal(nextButtons[currentButtonIndex + (3 - i)]);
				if(letter == "a") {
					GUI.Box(new Rect(startingX + leftMargin, startingY + topMargin - minusAmount, leftIconWidth, leftIconWidth), aButton);
				} else if(letter == "b"){
					GUI.Box(new Rect(startingX + leftMargin, startingY + topMargin - minusAmount, leftIconWidth, leftIconWidth), bButton);
				} else if(letter == "x"){
					GUI.Box(new Rect(startingX + leftMargin, startingY + topMargin - minusAmount, leftIconWidth, leftIconWidth), xButton);
				} else if(letter == "y"){
					GUI.Box(new Rect(startingX + leftMargin, startingY + topMargin - minusAmount, leftIconWidth, leftIconWidth), yButton);
				}
				
				if(i == 3) {
					if(correcto == 1) {
						GUI.Box(new Rect(startingX + leftMargin - 10, startingY + topMargin, leftIconWidth + 20, leftIconWidth + 20), correct);
					} else if(correcto == 2) {
						GUI.Box(new Rect(startingX + leftMargin - 10, startingY + topMargin, leftIconWidth + 20, leftIconWidth + 20), incorrect);
					}
				}
			}
		}
	}
}
