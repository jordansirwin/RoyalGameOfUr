using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStone : MonoBehaviour {

	public BoardTile StartingTile;
	public int BelongsToPlayer;

	private BoardTile currentTile;
	private GameStateMachine gameState;
	private float smoothTime = 0.25f;
	private Vector3 velocity;
	private Vector3? targetPosition;
	private BoardTile[] moveQueue;

	// Use this for initialization
	void Start () {
		gameState = GameObject.FindObjectOfType<GameStateMachine> ();
	}

	private int queueCounter;
//	private bool isMoving = false;

	// Update is called once per frame
	void Update () {

		// if we're not going anywhere, exit
		if (gameState.GameState != GameStateMachine.GameStates.MovingStone
			|| moveQueue == null) {
			return;
		}

		// movement should be like this:
		// 1. move stone up above board
		// 2. slide stone over each tile in movement
		// 3. set stone down on tile

		// need a target to move to
		if (queueCounter < moveQueue.Length) {
			targetPosition = moveQueue [queueCounter].transform.position;
		}

		// if we're moving we should be above the board
		if (queueCounter == 0) {

			// remove stone from current tile since we're leaving it
			if (currentTile != null) {
				currentTile.CurrentStone = null;
			}

			var moveUp = targetPosition.Value.y + Vector3.up.y;
			if (moveUp - transform.position.y >= 0.1f) {
//				Debug.Log ("Moving up");
				transform.position = Vector3.SmoothDamp (transform.position, new Vector3 (transform.position.x, moveUp, transform.position.z), ref velocity, smoothTime);
				return;
			}
		} 
		// if at end of queue, move down on baord
		else if (queueCounter >= moveQueue.Length) {
			if ((targetPosition.Value - transform.position).magnitude >= 0.1f) {
//				Debug.Log ("Moving down");
				transform.position = Vector3.SmoothDamp (transform.position, targetPosition.Value, ref velocity, smoothTime);
				return;
			} else {
				// end of queue, stop moving
//				Debug.Log ("Done moving");
				currentTile.CurrentStone = this;
				moveQueue = null;
//				isMoving = false;

				// set game state for next turn
				gameState.NextTurn ();
				return;
			}
		}

		// move until we're close enough
		var moveOver = new Vector3(targetPosition.Value.x, transform.position.y, targetPosition.Value.z);
		if ((moveOver - transform.position).magnitude >= 0.1f) {
//			Debug.Log ("Moving over");
			transform.position = Vector3.SmoothDamp(transform.position, moveOver, ref velocity, smoothTime);
			return;
		}
			
		// target next tile in queue and keep moving!
//		Debug.Log("Next move tile in queue!");
		currentTile = moveQueue[queueCounter];
		queueCounter++;
	}

	bool isValidTargetTile(BoardTile targetTile) {

		Debug.Log ("isValidTargetTile");

		// if no tile, false
		if (targetTile == null) {
			Debug.Log ("Invalid target tile");
			return false;
		}

		// if tile without existing stone
		if (targetTile.CurrentStone == null) {
			return true;
		}

		// if tile has opponent player stone, return true (will bop stone later)
		if (targetTile.CurrentStone.BelongsToPlayer != gameState.PlayerTurn) {
			return true;
		}

		// if tile has current player stone, return false
		if (targetTile.CurrentStone.BelongsToPlayer == gameState.PlayerTurn) {
			Debug.Log ("You already have a stone on this tile");
			return false;
		}

		// prolly invalid?
		Debug.Log ("Invalid move due to default validation check");
		return false;
	}


	void OnMouseUp() {

		Debug.Log ("OnMouseUp");

		// only 1 thing at a time
		if (gameState.GameState != GameStateMachine.GameStates.SelectAStone) {
			return;
		}

		// Make sure valid player clicks on valid stone
		if (gameState.PlayerTurn != BelongsToPlayer) {
			Debug.Log ("Invalid stone for player");
			return;
		}

		Debug.Log ("Clicked a stone");

		moveQueue = GetMoveQueue (gameState.TotalRoll);

		var lastTileInMove = moveQueue == null || moveQueue.Length == 0 
			? null 
			: moveQueue [moveQueue.Length - 1];
		if(isValidTargetTile(lastTileInMove) == false) {
			moveQueue = null;
			return;
		}

		// gtg for moving
		Debug.Log("Legal move let's go!");
		queueCounter = 0;
		gameState.GameState = GameStateMachine.GameStates.MovingStone;
//		isMoving = true;
	}

	private BoardTile[] GetMoveQueue(int moveCount) {
		Debug.Log ("GetMoveQueue");

		// initialize move queue
		var queue = new BoardTile[moveCount];

		if (moveCount == 0) {
			Debug.Log ("Invalid move because 0 movecount");
			return null;
		}

		// begin with tile we are on
		var prevTile = currentTile;

		// loop through the linked tiles until we build a list of them all or find invalid move
		for (int i = 0; i < moveCount; i++) {
			// if we aren't on the board, begin with starting tile
			if (prevTile == null) {
				queue [i] = StartingTile;
				prevTile = queue [i];
				continue;
			}

			// if movement still left but no more tiles, illegal move
			if (prevTile.NextTiles == null || prevTile.NextTiles.Length == 0) {
				Debug.Log ("Invalid move because not enough tiles left");
				return null;
			}

			// if next tile is single path
			if (prevTile.NextTiles.Length == 1) {
				queue[i] = prevTile.NextTiles [0];
			} else {
				// choose the path that matches player turn (zero based so this works!)
				queue[i] = prevTile.NextTiles [gameState.PlayerTurn];
			}

			prevTile = queue [i];
		}

		return queue;
	}
}
