using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDice : MonoBehaviour {

	public int TotalRoll;
	public Texture[] DiceTexturesForZero;
	public Texture[] DiceTexturesForOne;
	public GameObject DiceContainer;

	private int maxDice = 4;
	private int[] rolls;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RollTheDice() {

		TotalRoll = 0;
		rolls = new int[maxDice];
		for (int i = 0; i < maxDice; i++) {
			rolls[i] = Random.Range (0, 2);
			TotalRoll += rolls [i];
		}

		Debug.Log ("Dice roll was " + TotalRoll);
		AssignDiceArt ();
	}
		
	private void AssignDiceArt() {
		var images = DiceContainer.GetComponentsInChildren<RawImage> ();
		for (int i = 0; i < maxDice; i++) {
			if(rolls[i] == 0) {
				images[i].texture = DiceTexturesForZero[Random.Range(0, DiceTexturesForZero.Length - 1)];
			}
			else  {
				images[i].texture = DiceTexturesForOne[Random.Range(0, DiceTexturesForOne.Length - 1)];
			}	
		}
	}
}
