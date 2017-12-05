using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceResultText : MonoBehaviour {

	private GameStateMachine stateMachine;
	private Text text;

	// Use this for initialization
	void Start () {
		stateMachine = GameObject.FindObjectOfType<GameStateMachine> ();
		text = FindObjectOfType<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "= " + stateMachine.TotalRoll;
	}
}
