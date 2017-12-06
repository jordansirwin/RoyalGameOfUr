using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnText : MonoBehaviour {

	private GameStateMachine stateMachine;
	private Text turnText;

	private string[] humanizer = { "One", "Two" };

	// Use this for initialization
	void Start () {
		stateMachine = GameObject.FindObjectOfType<GameStateMachine> ();
		turnText = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		turnText.text = string.Format ("Player {0}'s Turn!", humanizer[stateMachine.PlayerTurn]);
	}
}
