using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceResultText : MonoBehaviour {

	private RollDice rollDice;
	private Text text;

	// Use this for initialization
	void Start () {
		rollDice = GameObject.FindObjectOfType<RollDice> ();
		text = FindObjectOfType<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "= " + rollDice.TotalRoll;
	}
}
