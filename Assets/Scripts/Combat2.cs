using UnityEngine;
using System.Collections;

public class Combat2 : MonoBehaviour {
	[HideInInspector]
	public int playerNumber;

	private float startingX = 0f;
	private float startingY = 0f;
	public Combat2 otherCombat;
	public Movement movement;
	public bool canEndIt = false;
	private bool started = false;
	private int selectedAttack = -1;
	private bool introOver = false;
	private bool isEnding = false;
	// Use this for initialization
	public void StartMatch () {
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
		StartCoroutine(TimeIt());
	}
	
	IEnumerator TimeIt() {
		// INTRO TIME
		yield return new WaitForSeconds(1f);
		introOver = true;
		// TIME TO SELECT BUTTON
		yield return new WaitForSeconds(1.5f);
		EndIt();
		Destroy(gameObject);
	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		if(introOver && selectedAttack == -1) {
			if(Input.GetButtonDown("aButton" + playerNumber.ToString())) {
				selectedAttack = 0;
				CheckIfOtherPlayerHasSelected();
			} else if(Input.GetButtonDown("bButton" + playerNumber.ToString())) {
				selectedAttack = 1;
				CheckIfOtherPlayerHasSelected();
			} else if(Input.GetButtonDown("yButton" + playerNumber.ToString())) {
				selectedAttack = 2;
				CheckIfOtherPlayerHasSelected();
			}
		}
	}
	
	void CheckIfOtherPlayerHasSelected() {
		if(otherCombat.selectedAttack > -1) {
			if(canEndIt) {
				EndIt();
			} else {
				otherCombat.EndIt();
			}
					
		}
	}
	
	public void EndIt() {
		if(!isEnding && canEndIt) {
			isEnding = true;
			int sa = selectedAttack;
			int osa = otherCombat.selectedAttack;
			if(sa == osa) {
				if(movement.currentplace < otherCombat.movement.currentplace) {
					movement.ExitCombat(1);
					otherCombat.movement.ExitCombat(2);
				} else {
					movement.ExitCombat(2);
					otherCombat.movement.ExitCombat(1);
				}
				return;
			}
			if(sa == -1) {
				movement.ExitCombat(0);
				otherCombat.movement.ExitCombat(1);
				return;
			}
			if(osa == -1) {
				otherCombat.movement.ExitCombat(0);
				movement.ExitCombat(1);
				return;
			}
			
			if(sa == 0 && osa == 2) {
				movement.ExitCombat(1);
				otherCombat.movement.ExitCombat(0);
			} else if(sa == 0 && osa == 1) {
				movement.ExitCombat(0);
				otherCombat.movement.ExitCombat(1);
			} else if(osa == 0 && sa == 2) {
				movement.ExitCombat(0);
				otherCombat.movement.ExitCombat(1);
			} else if(osa == 0 && sa == 1) {
				movement.ExitCombat(1);
				otherCombat.movement.ExitCombat(0);
			} else if(sa == 2 && osa == 1) {
				movement.ExitCombat(1);
				otherCombat.movement.ExitCombat(0);
			} else if(sa == 1 && osa == 2) {
				movement.ExitCombat(0);
				otherCombat.movement.ExitCombat(1);
			}
		}
	}
	
	void OnGUI() {
	
	}
	
	
}
