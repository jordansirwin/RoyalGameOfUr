﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour {

	public enum GameStates
	{
		RollTheDice,
		MoveAStone
	}

	public GameStates GameState;
	public int TotalRoll;
	public int PlayerTurn;

}
