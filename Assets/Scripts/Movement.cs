using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {


	private float activeButton;
	private float magRequired = 0.1f;

	public int player = 3;
	public GameObject combatObj;
	[HideInInspector]
	public float speed = 0f;
	
	public int character = 0;
	private float acceleration = 3.5f;
	private float deacceleration = 2.7f;
	private float deaccelerationRate = 30f;
	private float maxDeacceleration = 30f;
	private float baseDeacceleration = 0f;

	private float dangerZonePercent = 0.3f;
	private float warningPercent = 0.05f;
	private float maxSpeed = 150f;
	private float maxDangerZoneEnergyDrain = .4f;
	private float minDangerZoneEnergyDrain = .1f;
	private float dangerZone;
	
	private float extraEnergyGainAfterBattle = 0f;
	private float sleepPeriod = 1.25f;
	private float tieSpeedBonus = 0f;

	private float xStartingScreenPosition;
	private float yStartingScreenPosition;
	public bool isInit = false;
	private float iconSmallSize = 0f;
	private float iconLargeSize = 0.2f;
	private float startingX = 0f;
	private float startingY = 0f;
	private float startingIconLeftX;
	private float startingIconLeftY;
	private float startingIconRightX;
	private float startingIconRightY;
	public Texture2D leftIcon;
	public Texture2D rightIcon;
	public GameObject cameraGameObject;
	
	[HideInInspector]
	public bool inCombat = false;

	public GUISkin gStyle;
	private Texture2D clearTex;
	public int currentplace = 0;
	/* ENERGY BAR */

	private float currentEnergy = 100f;
	private float maxEnergy = 100f;
	private Texture2D energyTex;
	public Color energyColor;
	public float[] dangerPercents;

	/* HYPE BAR*/
	public Color goodColor;
	public Color warningColor;
	public Color dangerColor;
	public Color playerColor;

	private Texture2D greenTex;
	private Texture2D yellowTex;
	private Texture2D redTex;
	private Texture2D playerTex;

	private Texture2D greenTexActive;
	private Texture2D yellowTexActive;
	private Texture2D redTexActive;

	private float hypeBarOpacityNonActive = .75f;
	private float hypeBarOpacityActive = 1f;

	private float hypeBarLength = 0.2f;
	private float hypeBarGoodLength;
	private float hypeBarWarningLength;
	private float hypeBarDangerLength;

	private float playerHypePosition;
	
	private GameObject[] players;
	
	public GameObject iluminationObj;
	public GameObject particlesObj;
	[HideInInspector]
	public bool isSleeping = false;
	
	private Vector3 standardCamPosition;
	private Vector3 standardCamRotation;
	
	public GameObject combatCamObj;
	
	private float maxFOV = 75f;
	private float lowestFOV = 40f;
	public Material mat;
	public GameObject p1;
	public GameObject p2;
	public GameObject p3;
	public GameObject p4;
	public GameObject spawnPoint;
	private GameObject spawnedPlayer;
	private bool isGod = false;
	
	private float currentZPosition;
	private bool falling = false;
	
	public Texture2D greenGUI;
	public Texture2D redGUI;
	public Texture2D blueGUI;
	public Texture2D yellowGUI;
	
	public Texture2D[] triggers;
	public GameObject particleExplosion;
	public GameObject demZs;
	public float zCombat;
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(0.1f);
		ChangeVariablesByClass();
		standardCamPosition = cameraGameObject.transform.localPosition;
		standardCamRotation = cameraGameObject.transform.localRotation.eulerAngles;
		GlobalVars.playerCharacters[player- 1] = player - 1;
		//mat.SetColor("_OutlineColor", GlobalVars.IntToColor(GlobalVars.playerCharacters[player]));
		particlesObj.particleSystem.startColor = GlobalVars.IntToColor(GlobalVars.playerCharacters[player - 1]);
		iluminationObj.light.color = GlobalVars.IntToColor(GlobalVars.playerCharacters[player - 1]);
		iconSmallSize = Screen.width * .5f * iconSmallSize;
		iconLargeSize = Screen.width * .5f * iconLargeSize;
		hypeBarLength = Screen.width * .5f * hypeBarLength;
		players = GameObject.FindGameObjectsWithTag("Player");
		SetTextures ();
		SetHypeBarLengths ();
		if (player == 1) {
			cameraGameObject.camera.rect = new Rect(0f, .51f, .5f, .49f);
			spawnedPlayer = (GameObject)Instantiate(p1, spawnPoint.transform.position, spawnPoint.transform.rotation);
		} else if (player == 2) {
			startingX = Screen.width * .5f;
			cameraGameObject.camera.rect = new Rect(.5f, .51f, .5f, .49f);
			spawnedPlayer = (GameObject)Instantiate(p2, spawnPoint.transform.position, spawnPoint.transform.rotation);
		} else if (player == 3) {
			startingY = Screen.height * .51f;
			cameraGameObject.camera.rect = new Rect(0f, 0f, .5f, .49f);
			spawnedPlayer = (GameObject)Instantiate(p3, spawnPoint.transform.position, spawnPoint.transform.rotation);
		} else if (player == 4) {
			startingX = Screen.width * .5f;
			startingY = Screen.height * .51f;
			cameraGameObject.camera.rect = new Rect(.5f, 0f, .5f, .49f);
			spawnedPlayer = (GameObject)Instantiate(p4, spawnPoint.transform.position, spawnPoint.transform.rotation);
		}
		spawnedPlayer.transform.parent = spawnPoint.transform;
		startingIconLeftX = Screen.width * .5f * .65f;
		startingIconLeftY = Screen.height * .5f * .2f;

		startingIconRightX = Screen.width * .5f * .75f;
		startingIconRightY = Screen.height * .5f * .2f;
		dangerZone = maxSpeed * dangerZonePercent;
		activeButton = magRequired;
	}
	
	// Update is called once per frame
	void Update () {
		if(!GlobalVars.gameOver) {
			if(isInit) {
				if(!inCombat && !isSleeping) {
					
					baseDeacceleration += deaccelerationRate * Time.deltaTime;
					if(baseDeacceleration > maxDeacceleration) {
						baseDeacceleration = maxDeacceleration;
					}
					// speed *= deacceleration;
					speed -= (baseDeacceleration + deacceleration) * Time.deltaTime;
					if (speed < 0f) {
						speed = 0;
					}
					if (activeButton < 0 && Input.GetAxis ("Triggers" + player.ToString ()) < activeButton) {
						CorrectButtonPressed();
						baseDeacceleration = 0f;
					} else if (activeButton > 0 && Input.GetAxis ("Triggers" + player.ToString ()) > activeButton) {
						CorrectButtonPressed();
						baseDeacceleration = 0f;
					}
					playerHypePosition = (speed / maxSpeed) * hypeBarLength;
					ChangeEnergy(Time.deltaTime);
					
				} 
				transform.Translate (new Vector3 ((speed + 15f) * Time.deltaTime, 0f, 0f));
				CheckPlace(Time.deltaTime);
				
			}
			
			if(inCombat) {
				if(transform.position.z < zCombat) {
					transform.Translate(new Vector3(0f, 0f, Time.deltaTime * 6f));
				}
			} else {
				if(transform.position.z > 0) {
					transform.Translate(new Vector3(0f, 0f, Time.deltaTime * -2f));
				}
			}
			CheckForVictory();
			cameraGameObject.camera.fieldOfView = (1 - (speed/maxSpeed)) * (maxFOV - lowestFOV) + lowestFOV;
		}
		else{
			
			StartCoroutine(Win());
		}
		
		if(spawnedPlayer != null) {
			if(isSleeping) {
				
			}else if(inCombat) {
				spawnedPlayer.animation.CrossFade("attack");
			} else {
				spawnedPlayer.animation.CrossFade("idle");
			}
		}
		
		if(falling){
			this.transform.Translate(0f, -5.0f * Time.deltaTime, 0f);
		}
	}
	
	IEnumerator Win() {
		yield return new WaitForSeconds(5f);
		Application.LoadLevel("joinscreen");
		
	}
	
	void CheckPlace(float deltaTime) {
		int numPlayersBehind = 0;
		for(int i = 0; i < players.Length; i++) {
			if(players[i].transform.position.x < transform.position.x) {
				numPlayersBehind ++;
			}
		}
		currentplace = players.Length - numPlayersBehind;
		if(dangerZonePercent + .01 < dangerPercents[currentplace - 1]) {
			dangerZonePercent += .1f * deltaTime;
		} else if(dangerZonePercent - .01 > dangerPercents[currentplace - 1]) {
			dangerZonePercent -= .1f * deltaTime;
		}
		SetHypeBarLengths();
	}

	void SetHypeBarLengths () {
		hypeBarGoodLength = hypeBarLength * (1f - warningPercent - dangerZonePercent);
		hypeBarWarningLength = hypeBarLength * warningPercent;
		hypeBarDangerLength = hypeBarLength * dangerZonePercent;
	}

	void SetTextures() {
		greenTex = new Texture2D (1, 1);
		greenTex.SetPixel (0, 0, new Color (goodColor.r, goodColor.g, goodColor.b, hypeBarOpacityNonActive));
		greenTex.Apply ();
		
		yellowTex = new Texture2D (1, 1);
		yellowTex.SetPixel (0, 0, new Color (warningColor.r, warningColor.g, warningColor.b, hypeBarOpacityNonActive));
		yellowTex.Apply ();
		
		redTex = new Texture2D (1, 1);
		redTex.SetPixel (0, 0, new Color (dangerColor.r, dangerColor.g, dangerColor.b, hypeBarOpacityNonActive));
		redTex.Apply ();


		greenTexActive = new Texture2D (1, 1);
		greenTexActive.SetPixel (0, 0, new Color (goodColor.r, goodColor.g, goodColor.b, hypeBarOpacityActive));
		greenTexActive.Apply ();
		
		yellowTexActive = new Texture2D (1, 1);
		yellowTexActive.SetPixel (0, 0, new Color (warningColor.r, warningColor.g, warningColor.b, hypeBarOpacityActive));
		yellowTexActive.Apply ();
		
		redTexActive = new Texture2D (1, 1);
		redTexActive.SetPixel (0, 0, new Color (dangerColor.r, dangerColor.g, dangerColor.b, hypeBarOpacityActive));
		redTexActive.Apply ();

		clearTex = new Texture2D (1, 1);
		clearTex.SetPixel (0, 0, new Color (0,0,0, 0));
		clearTex.Apply ();

		playerTex = new Texture2D (1, 1);
		playerTex.SetPixel (0, 0, new Color (playerColor.r, playerColor.g, playerColor.b, 1));
		playerTex.Apply ();
		
		energyTex = new Texture2D (1, 1);
		energyTex.SetPixel (0, 0, energyColor);
		energyTex.Apply ();
	}
	
	
	
	void ChangeEnergy(float deltaTime) {
		deltaTime *= 50f;
		if(playerHypePosition < hypeBarGoodLength) {
			// Gain energy
			currentEnergy += ((1f - (playerHypePosition / hypeBarGoodLength)) + .2f) * deltaTime;
		} else if (playerHypePosition < hypeBarGoodLength + hypeBarWarningLength) {
			// Lose 0-10%
			//currentEnergy -= (((playerHypePosition - hypeBarGoodLength) / hypeBarWarningLength) * .1f) * deltaTime;
		} else {
			currentEnergy -= (((playerHypePosition - hypeBarGoodLength - hypeBarWarningLength) / hypeBarDangerLength) * maxDangerZoneEnergyDrain + minDangerZoneEnergyDrain) * deltaTime;
		}
		
		if(currentEnergy > maxEnergy) {
			currentEnergy = maxEnergy;
		} else if(currentEnergy < 0) {
			currentEnergy = 0f;
			StartCoroutine(FallAsleep());
		}
	}
	
	IEnumerator FallAsleep() {
		isSleeping = true;
		currentEnergy = 0;
		speed = 0f;
		spawnedPlayer.animation.Play("die");
		currentZPosition = this.transform.position.y;
		iluminationObj.light.range = 5;
		falling = true;
		Instantiate(demZs, transform.position, Quaternion.identity);
		yield return new WaitForSeconds(1f);
		HideShit(false);
		Instantiate(particleExplosion, transform.position, Quaternion.identity);
		yield return new WaitForSeconds(sleepPeriod);
		HideShit(true);
		iluminationObj.light.range = 70;
		isSleeping = false;
		falling = false;
		StartCoroutine(MakeInv(3.5f));
		this.transform.position = new Vector3(transform.position.x, currentZPosition, transform.position.z);
	}
	
	IEnumerator MakeInv(float duration) {
		isGod = true;
		yield return new WaitForSeconds(duration);
		isGod = false;
	}
	
	void HideShit(bool hide) {
		Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
		for(int i = 0; i < renderers.Length; i++) {
			renderers[i].enabled = hide;
		}
	}

	void CorrectButtonPressed() {
		activeButton *= -1;
		speed += acceleration;
		if (speed > maxSpeed) {
			speed = maxSpeed;
		}
	}
	
	void CheckForVictory() {
		if(transform.position.x > GlobalVars.goalXPosition) {
			GlobalVars.gameOver = true;
			transform.position = new Vector3(GlobalVars.goalXPosition, 0f, 0f);
		}	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			Debug.Log("MORTAL KOMBAT");
			EnterCombat(other.gameObject);		
		}	
	}
	
	void EnterCombat(GameObject other) {
		Movement m = other.gameObject.GetComponent<Movement>();
		if(!m.inCombat && !inCombat && !isSleeping && !m.isSleeping && !isGod && !m.isGod) {
			if(other.transform.position.x < transform.position.x) {
				MoveCameraToCombat(cameraGameObject, -2.5f);
				MoveCameraToCombat(m.cameraGameObject, 2.5f);
				iTween.RotateTo(spawnedPlayer, new Vector3(0f, 90f, 0f), .5f);
			} else {
				iTween.RotateTo(m.spawnedPlayer, new Vector3(0f, 90f, 0f), .5f);
				MoveCameraToCombat(cameraGameObject, 2.5f);
				MoveCameraToCombat(m.cameraGameObject, -2.5f);
			}
			
			inCombat = true;
			m.inCombat = true;
			float slowestSpeed = speed;
			slowestSpeed = (m.speed + speed) / 2.5f;
			speed = slowestSpeed;
			m.speed = slowestSpeed;
			
			if(transform.position.x < other.transform.position.x) {
				transform.position = new Vector3(other.transform.position.x - 5f, 0, 0);
			} else {
				other.transform.position = new Vector3(transform.position.x - 5f, 0, 0);
			}
			
			GameObject cObj1 = (GameObject)Instantiate(combatObj, new Vector3(0f,0f,0f), Quaternion.identity);
			Combat2 c1 = cObj1.GetComponent<Combat2>();
			c1.playerNumber = player;
			c1.movement = this;
			c1.canEndIt = true;
			GameObject cObj2 = (GameObject)Instantiate(combatObj, new Vector3(0f,0f,0f), Quaternion.identity);
			Combat2 c2 = cObj2.GetComponent<Combat2>();
			c2.playerNumber = m.player;
			c2.movement = m;

			c1.otherCombat = c2;
			c2.otherCombat = c1;
			float matchTime;
			int playersPlaces = currentplace + m.currentplace;
			if(playersPlaces < 3) {
				matchTime = 4f;
			} else if(playersPlaces < 6) {
				matchTime = 3.75f;
			} else {
				matchTime = 3.25f;
			}
			
			c1.StartMatch();
			c2.StartMatch();
		
		} else {
			//speed = m.speed;
			//inCombat = true;
		}	
	}
	
	void MoveCameraToCombat(GameObject camObj, float deltaX) {
		float animateTime = .5f;
		Hashtable movementHash = new Hashtable();
		movementHash.Add("position", new Vector3(combatCamObj.transform.localPosition.x + deltaX, combatCamObj.transform.localPosition.y, combatCamObj.transform.localPosition.z));
		movementHash.Add("islocal", true);
		movementHash.Add("time", animateTime);
		iTween.MoveTo(camObj, movementHash);
		
		Hashtable rotateHash = new Hashtable();
		rotateHash.Add("time", animateTime);
		rotateHash.Add("islocal", true);
		rotateHash.Add ("rotation", combatCamObj.transform.rotation.eulerAngles);
		iTween.RotateTo(camObj, rotateHash);
		
	}
	
	void MoveCameraToIdle(GameObject camObj, float time) {
		Hashtable movementHash = new Hashtable();
		movementHash.Add("position", standardCamPosition);
		movementHash.Add("islocal", true);
		movementHash.Add("time", time);
		iTween.MoveTo(camObj, movementHash);
		
		Hashtable rotateHash = new Hashtable();
		rotateHash.Add("time", time);
		rotateHash.Add("islocal", true);
		rotateHash.Add ("rotation", standardCamRotation);
		iTween.RotateTo(camObj, rotateHash);
	}
	
	public void ExitCombat(int victory) {
		if(victory == 0) {
			StartCoroutine(FallAsleep());
			MoveCameraToIdle(cameraGameObject, 4f);
		} else if(victory == 1){
			MoveCameraToIdle(cameraGameObject, 2f);
			speed *= (1f + tieSpeedBonus);
		} else if(victory == 2) {
			MoveCameraToIdle(cameraGameObject, 3f);
			speed *= (.65f + tieSpeedBonus);
		}
		if(currentEnergy < maxEnergy * (.5f + extraEnergyGainAfterBattle)) {
			currentEnergy = maxEnergy * (.5f + extraEnergyGainAfterBattle);
		}
		StartCoroutine(MakeInv(3.5f));
		iTween.RotateTo(spawnedPlayer, new Vector3(0f, 270f, 0f), .25f);
		inCombat = false;
	}
	
	string GetPlaceString() {
		switch (currentplace) {
			case 1 :
				return "1st";
				break;
			case 2 :
				return "2nd";
				break;
			case 3 :
				return "3rd";
				break;
			case 4 :
				return "4th";
				break;
			default :
				return "derp";
				break;
		}
	}
	
	void ChangeVariablesByClass() {
	/*
		private float acceleration = 3.5f;
		private float deacceleration = 2.7f;
		private float deaccelerationRate = 30f;
		private float maxDeacceleration = 30f;
		private float baseDeacceleration = 0f;
	
		private float dangerZonePercent = 0.3f;
		private float warningPercent = 0.05f;
		private float maxSpeed = 150f;
		maxDangerZoneEnergyDrain = .4f
	*/
		if(GlobalVars.playerClasses[player - 1] == 0) {
			maxDangerZoneEnergyDrain *= .75f; // Max energy drain is 80% of the default
			minDangerZoneEnergyDrain *= .5f; // Min energy drain is 50% of the default
			acceleration *= .6f; // Acceleration is 60% of the default.
		} else if(GlobalVars.playerClasses[player - 1] == 1) {
			// Sleep for shorter period, note that that the sleepPeriod variable is only a portion of the total sleeping time
			// So cutting it in half will not actually reduce sleep ammount by half
			sleepPeriod *= .5f;
			extraEnergyGainAfterBattle = .2f;
			maxSpeed *= .82f;
		} else if(GlobalVars.playerClasses[player - 1] == 2) {
			acceleration *= 1.4f;
			maxSpeed *= .9f;
		} else if(GlobalVars.playerClasses[player - 1] == 3) {
			sleepPeriod *= 1.4f;
			tieSpeedBonus = .13f;
		}
	}

	void OnGUI() {
		GUI.skin = gStyle;

		/* SPEED BAR */
		float barHeight = Screen.height * .03f;
		float barXDistance = Screen.width * .357f;
		float barYDistance = Screen.height * .42f;

		if (playerHypePosition < hypeBarGoodLength) {
			gStyle.box.normal.background = greenTexActive;
		} else {
			gStyle.box.normal.background = greenTex;		
		}

		GUI.Box (new Rect (startingX + barXDistance, startingY + barYDistance, hypeBarGoodLength, barHeight),"");

		if (playerHypePosition > hypeBarGoodLength && playerHypePosition < hypeBarGoodLength + hypeBarWarningLength) {
			gStyle.box.normal.background = yellowTexActive;
		} else {
			gStyle.box.normal.background = yellowTex;		
		}

		GUI.Box (new Rect (startingX + barXDistance + hypeBarGoodLength, startingY + barYDistance, hypeBarWarningLength, barHeight), "");
		if (playerHypePosition > hypeBarGoodLength + hypeBarWarningLength) {
			gStyle.box.normal.background = redTexActive;
		} else {
			gStyle.box.normal.background = redTex;
		}
		GUI.Box (new Rect (startingX + barXDistance + hypeBarGoodLength + hypeBarWarningLength, startingY + barYDistance, hypeBarDangerLength, barHeight), "");

		/* PLAYER IS HYPE*/
		gStyle.box.normal.background = playerTex;
		GUI.Box (new Rect (startingX + barXDistance + playerHypePosition, startingY + barYDistance - 4, 2f, barHeight + 8), "");
		
		if(!inCombat) {
			/* ENERGY BAR */
			float barWidth = (currentEnergy / maxEnergy) * hypeBarLength;
			barYDistance = Screen.height * .45f;
			gStyle.box.normal.background = energyTex;
			GUI.Box(new Rect(startingX + barXDistance, startingY + barYDistance, barWidth, barHeight), "");
			GUI.Box(new Rect(startingX + barXDistance + barWidth, startingY + barYDistance, 2, barHeight), "");
			
		}
		/* GUI Overlay and place indicator */
		if(player == 1) {
			gStyle.box.normal.background = redGUI;
			GUI.Box(new Rect(startingX, startingY, Screen.width * .5f, Screen.height * .49f),"" );
			gStyle.box.normal.background = clearTex;
			gStyle.box.normal.textColor = GlobalVars.redColor;
			GUI.Box (new Rect(startingX + Screen.width * .5f - 125f, startingY + 15f, 125f, 100f), GetPlaceString());
		} else if(player == 2) {
			gStyle.box.normal.background = blueGUI;
			GUI.Box(new Rect(startingX, startingY, Screen.width * .5f, Screen.height * .49f),"" );
			gStyle.box.normal.background = clearTex;
			gStyle.box.normal.textColor = GlobalVars.blueColor;
			GUI.Box (new Rect(startingX + Screen.width * .5f - 125f, startingY + 15f, 125f, 100f), GetPlaceString());
		} else if(player == 3) {
			gStyle.box.normal.background = greenGUI;
			GUI.Box(new Rect(startingX, startingY, Screen.width * .5f, Screen.height * .49f),"" );
			gStyle.box.normal.background = clearTex;
			gStyle.box.normal.textColor = GlobalVars.greenColor;
			GUI.Box (new Rect(startingX + Screen.width * .5f - 125f, startingY + 15f, 125f, 100f), GetPlaceString());
		} else if(player == 4) {
			gStyle.box.normal.background = yellowGUI;
			GUI.Box(new Rect(startingX, startingY, Screen.width * .5f, Screen.height * .49f),"" );
			gStyle.box.normal.background = clearTex;
			gStyle.box.normal.textColor = GlobalVars.yellowColor;
			GUI.Box (new Rect(startingX + Screen.width * .5f - 125f, startingY + 15f, 125f, 100f), GetPlaceString());
		}
		if (isInit && !inCombat) {
			/* LEFT and RIGHT Triggers*/
			gStyle.box.normal.background = clearTex;
			float leftIconWidth = iconLargeSize;
			float rightIconWidth = iconLargeSize;
			int index = (player - 1) * 2;
			
			if(activeButton < 0) {
				leftIconWidth = iconLargeSize;
				rightIconWidth = iconLargeSize;
				index += 1;
				GUI.Box(new Rect(startingX + Screen.width * .41f, startingY + Screen.height * .37f, rightIconWidth, rightIconWidth), triggers[index]);
			} else {
				GUI.Box(new Rect(startingX + Screen.width * .298f, startingY + Screen.height * .37f, leftIconWidth, leftIconWidth), triggers[index]);
			}
		}
		
		/* PLACE INDICATOR */
		
		
	}
}
