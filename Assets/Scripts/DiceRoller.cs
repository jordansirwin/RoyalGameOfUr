using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour {

	public Texture[] DiceTexturesForZero;
	public Texture[] DiceTexturesForOne;
	public GameObject DiceContainer;

	private int[] rolls;
	private GameStateMachine stateMachine;
	private int maxDice = 4;

	// Use this for initialization
	void Start () {
		stateMachine = GameObject.FindObjectOfType<GameStateMachine> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RollTheDice() {

		// If we're not ready to roll a dice, bail out
		if (stateMachine.GameState != GameStateMachine.GameStates.RollTheDice) {
			return;
		}

		// reset game state for the roll
		stateMachine.TotalRoll = 0;
		rolls = new int[maxDice];

		// roll for each dice and total the score
		for (int i = 0; i < maxDice; i++) {
			rolls[i] = Random.Range (0, 2);
			stateMachine.TotalRoll += rolls [i];
		}
		Debug.Log ("Dice roll was " + stateMachine.TotalRoll);

		// show some art of the dice roll
		UpdateDiceArt ();

		// set the next game state after a roll
		stateMachine.GameState = GameStateMachine.GameStates.MoveAStone;
	}

	private void UpdateDiceArt() {
		var images = DiceContainer.GetComponentsInChildren<RawImage> ();
		for (int i = 0; i < rolls.Length; i++) {
			if(rolls[i] == 0) {
				images[i].texture = DiceTexturesForZero[Random.Range(0, DiceTexturesForZero.Length - 1)];
			}
			else  {
				images[i].texture = DiceTexturesForOne[Random.Range(0, DiceTexturesForOne.Length - 1)];
			}	
		}
	}
}
