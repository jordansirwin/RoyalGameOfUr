using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateMachine : MonoBehaviour {

	public enum GameStates
	{
		RollTheDice,
		SelectAStone,
		MovingStone
	}

	public Text InvalidMoveText;

	public GameStates GameState;
	public int TotalRoll;

	public int TurnNumber;
	public int PlayerTurn { get { return TurnNumber % 2; } }

	public void NextTurn() {
		Debug.Log ("Next turn!");
		TurnNumber++;
		GameState = GameStates.RollTheDice;
	}

	public IEnumerator ShowInvalidMoveText() {
		InvalidMoveText.enabled = true;
		while (true) {
			yield return new WaitForSeconds (1f);
			break;
		}

		InvalidMoveText.enabled = false;
	}
}
