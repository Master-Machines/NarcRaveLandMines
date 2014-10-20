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
	private float acceleration = 2f;
	private float deacceleration = 0.8f;
	private float deaccelerationRate = 13f;
	private float maxDeacceleration = 20f;
	private float baseDeacceleration = 0f;

	private float dangerZonePercent = 0.3f;
	private float warningPercent = 0.2f;
	private float maxSpeed = 120f;
	private float dangerZone;

	private float xStartingScreenPosition;
	private float yStartingScreenPosition;
	private bool isInit = false;
	private float iconSmallSize = 0f;
	private float iconLargeSize = 0.08f;
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
	private int currentplace = 0;
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
	private Vector3 standardCamOrientation;
	
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
	
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(0.1f);
		Debug.Log("WAIT>>> WHAT>>> " + player);
		GlobalVars.playerCharacters[player- 1] = player - 1;
		//mat.SetColor("_OutlineColor", GlobalVars.IntToColor(GlobalVars.playerCharacters[player]));
		particlesObj.particleSystem.startColor = GlobalVars.IntToColor(GlobalVars.playerCharacters[player - 1]);
		iluminationObj.light.color = GlobalVars.IntToColor(GlobalVars.playerCharacters[player - 1]);
		iconSmallSize = Screen.width * .5f * iconSmallSize;
		iconLargeSize = 200f;
		hypeBarLength = Screen.width * .5f * hypeBarLength;
		hypeBarLength = 204f;
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
		isInit = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!GlobalVars.gameOver) {
			if(isInit) {
				if(!inCombat) {
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
				CheckPlace(Time.deltaTime);	
				transform.Translate (new Vector3 ((speed + 15f) * Time.deltaTime, 0f, 0f));
			}
			CheckForVictory();
			cameraGameObject.camera.fieldOfView = (1 - (speed/maxSpeed)) * (maxFOV - lowestFOV) + lowestFOV;
		}
		else{
			
			StartCoroutine(Win());
		}
		
		
		if(isSleeping) {
		
		}else if(inCombat) {
			spawnedPlayer.animation.CrossFade("attack");
		} else {
			spawnedPlayer.animation.CrossFade("idle");
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
			currentEnergy -= (((playerHypePosition - hypeBarGoodLength) / hypeBarWarningLength) * .1f) * deltaTime;
		} else {
			currentEnergy -= (((playerHypePosition - hypeBarGoodLength - hypeBarWarningLength) / hypeBarDangerLength) * .9f + .1f) * deltaTime;
		}
		
		if(currentEnergy > maxEnergy) {
			currentEnergy = maxEnergy;
		} else if(currentEnergy < 0) {
			currentEnergy = 0f;
			StartCoroutine(FallAsleep());
		}
	}
	
	IEnumerator FallAsleep() {
		isInit = false;
		isSleeping = true;
		currentEnergy = maxEnergy / 3f;
		speed = 0f;
		spawnedPlayer.animation.Play("die");
		isGod = true;
		currentZPosition = this.transform.position.z;
		iluminationObj.light.range = 5;
		falling = true;
		Instantiate(demZs, transform.position, Quaternion.identity);
		yield return new WaitForSeconds(1f);
		HideShit(false);
		Instantiate(particleExplosion, transform.position, Quaternion.identity);
		yield return new WaitForSeconds(1f);
		HideShit(true);
		iluminationObj.light.range = 70;
		isInit = true;
		isSleeping = false;
		falling = false;
		this.transform.position = new Vector3(transform.position.x, currentZPosition, transform.position.z);
		yield return new WaitForSeconds(currentplace);
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
			inCombat = true;
			m.inCombat = true;
			float slowestSpeed = speed;
			if(m.speed > speed) {
				slowestSpeed = m.speed;
			}
			speed = slowestSpeed;
			m.speed = slowestSpeed;
			
			if(transform.position.x < other.transform.position.x) {
				transform.position = new Vector3(other.transform.position.x - 5f, 0, 0);
			} else {
				other.transform.position = new Vector3(transform.position.x - 5f, 0, 0);
			}
			
			GameObject cObj1 = (GameObject)Instantiate(combatObj, new Vector3(0f,0f,0f), Quaternion.identity);
			Combat c1 = cObj1.GetComponent<Combat>();
			c1.playerNumber = player;
			c1.otherPlayerNumber = m.player;
			c1.movement = this;
			
			GameObject cObj2 = (GameObject)Instantiate(combatObj, new Vector3(0f,0f,0f), Quaternion.identity);
			Combat c2 = cObj2.GetComponent<Combat>();
			c2.playerNumber = m.player;
			c2.otherPlayerNumber = player;
			c2.movement = m;

			c1.otherPlayerCombat = c2;
			c2.otherPlayerCombat = c1;
			float matchTime;
			int playersPlaces = currentplace + m.currentplace;
			if(playersPlaces < 3) {
				matchTime = 4f;
			} else if(playersPlaces < 6) {
				matchTime = 3.75f;
			} else {
				matchTime = 3.25f;
			}
			
			c1.StartMatch(matchTime);
			c2.StartMatch(matchTime);
		
		} else {
			//speed = m.speed;
			//inCombat = true;
		}	
	}
	
	public void ExitCombat(bool victory) {
		if(!victory) {
			StartCoroutine(FallAsleep());
		}
		inCombat = false;
	}

	void OnGUI() {
		GUI.skin = gStyle;

		/* SPEED BAR */
		float barHeight = 26f;
		float barXDistance = 670f;
		float barYDistance = 380f;

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
			barYDistance = 413f;
			gStyle.box.normal.background = energyTex;
			GUI.Box(new Rect(startingX + barXDistance, startingY + barYDistance, barWidth, barHeight), "");
			GUI.Box(new Rect(startingX + barXDistance + barWidth, startingY + barYDistance, 2, barHeight), "");
			
		}

		if(player == 1) {
			gStyle.box.normal.background = redGUI;
			GUI.Box(new Rect(startingX, startingY, Screen.width * .5f, Screen.height * .49f),"" );
		} else if(player == 2) {
			gStyle.box.normal.background = blueGUI;
			GUI.Box(new Rect(startingX, startingY, Screen.width * .5f, Screen.height * .49f),"" );
		} else if(player == 3) {
			gStyle.box.normal.background = greenGUI;
			GUI.Box(new Rect(startingX, startingY, Screen.width * .5f, Screen.height * .49f),"" );
		} else if(player == 4) {
			gStyle.box.normal.background = yellowGUI;
			GUI.Box(new Rect(startingX, startingY, Screen.width * .5f, Screen.height * .49f),"" );
		}
		if (isInit && !inCombat) {
			
			gStyle.box.normal.background = clearTex;
			float leftIconWidth = iconLargeSize;
			float rightIconWidth = iconLargeSize;
			int index = (player - 1) * 2;
			
			if(activeButton < 0) {
				leftIconWidth = iconLargeSize;
				rightIconWidth = iconLargeSize;
				index += 1;
				GUI.Box(new Rect(startingX + 785f, startingY + 345f, rightIconWidth, rightIconWidth), triggers[index]);
			} else {
				GUI.Box(new Rect(startingX + 575f, startingY + 345f, leftIconWidth, leftIconWidth), triggers[index]);
			}
		}
	}
}
