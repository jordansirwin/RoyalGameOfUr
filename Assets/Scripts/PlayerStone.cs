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
	private bool isMoving = false;

	// Update is called once per frame
	void Update () {

		// if we're not going anywhere, exit
		if (isMoving == false) {
			return;
		}

		// need a target to move to
		if (queueCounter < moveQueue.Length) {
			targetPosition = moveQueue [queueCounter].transform.position;
		}

		// if we're moving we should be above the board
		if (queueCounter == 0) {
			var moveUp = targetPosition.Value.y + Vector3.up.y;
			if (moveUp - transform.position.y >= 0.1f) {
				Debug.Log ("Moving up");
				transform.position = Vector3.SmoothDamp (transform.position, new Vector3 (transform.position.x, moveUp, transform.position.z), ref velocity, smoothTime);
				return;
			}
		} 
		// if at end of queue, move down on baord
		else if (queueCounter >= moveQueue.Length) {
			if ((targetPosition.Value - transform.position).magnitude >= 0.1f) {
				Debug.Log ("Moving down");
				transform.position = Vector3.SmoothDamp (transform.position, targetPosition.Value, ref velocity, smoothTime);
				return;
			} else {
				// end of queue, stop moving
				Debug.Log ("Done moving");
				isMoving = false;
				return;
			}
		}

		// move until we're close enough
		var moveOver = new Vector3(targetPosition.Value.x, transform.position.y, targetPosition.Value.z);
		if ((moveOver - transform.position).magnitude >= 0.1f) {
			Debug.Log ("Moving over");
			transform.position = Vector3.SmoothDamp(transform.position, moveOver, ref velocity, smoothTime);
			return;
		}
			
		// target next tile in queue and keep moving!
		Debug.Log("Next move tile in queue!");
		currentTile = moveQueue[queueCounter];
		queueCounter++;
	}


	void OnMouseUp() {

		// Make sure valid player clicks on valid stone
		if (gameState.PlayerTurn != BelongsToPlayer) {
			Debug.Log ("Invalid stone for player");
			return;
		}

		Debug.Log ("Clicked a stone");

		GetMoveQueue (gameState.TotalRoll);
		queueCounter = 0;
		isMoving = true;
	}

	private BoardTile[] GetMoveQueue(int moveCount) {
		// initialize move queue
		moveQueue = new BoardTile[moveCount];

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
				moveQueue [i] = StartingTile;
				prevTile = moveQueue [i];
				continue;
			}

			// if movement still left but no more tiles, illegal move
			if (prevTile.NextTiles == null || prevTile.NextTiles.Length == 0) {
				Debug.Log ("Invalid move because not enough tiles left");
				return null;
			}

			// if next tile is single path
			if (prevTile.NextTiles.Length == 1) {
				moveQueue[i] = prevTile.NextTiles [0];
			} else {
				// choose the path that matches player turn (zero based so this works!)
				moveQueue[i] = prevTile.NextTiles [gameState.PlayerTurn];
			}

			prevTile = moveQueue [i];
		}

		return moveQueue;
	}
}
