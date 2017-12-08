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
	private BoardTile targetTile;
	private BoardTile[] moveQueue;

	// Use this for initialization
	void Start () {
		gameState = GameObject.FindObjectOfType<GameStateMachine> ();
	}

	private int queueCounter;
	// Update is called once per frame
	void Update () {

		// if we're not going anywhere, exit
		if (moveQueue == null) {
			return;
		}

		if (targetTile == null) {
			if (moveQueue.Length > queueCounter) {
				targetTile = moveQueue [queueCounter];
			} else {
				queueCounter = 0;
				moveQueue = null;
				return;
			}
		}

		// move until we're close enough
		if ((transform.position - targetTile.gameObject.transform.position).magnitude <= 0.1f) {
			currentTile = targetTile;
			targetTile = null;
			queueCounter++;
			return;
		}

		// keep moving
		transform.position = Vector3.SmoothDamp(transform.position, targetTile.gameObject.transform.position, ref velocity, smoothTime);
	}


	void OnMouseUp() {

		// Make sure valid player clicks on valid stone
		if (gameState.PlayerTurn != BelongsToPlayer) {
			Debug.Log ("Invalid stone for player");
			return;
		}

		Debug.Log ("Clicked a stone");

		GetMoveQueue (gameState.TotalRoll);
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
