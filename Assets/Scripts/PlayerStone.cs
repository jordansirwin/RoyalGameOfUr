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


	// Use this for initialization
	void Start () {
		gameState = GameObject.FindObjectOfType<GameStateMachine> ();
	}
	
	// Update is called once per frame
	void Update () {

		// if we're not going anywhere, exit
		if (targetTile == null) {
			return;
		}

		// move until we're close enough
		if ((transform.position - targetTile.gameObject.transform.position).magnitude <= 0.1f) {
			currentTile = targetTile;
			targetTile = null;
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

		targetTile = GetTargetTile (gameState.TotalRoll);
	}

	private BoardTile GetTargetTile(int moveCount) {
		if (moveCount == 0) {
			Debug.Log ("Invalid move because 0 movecount");
			return null;
		}

		BoardTile tile = currentTile;
		for (int i = 0; i < moveCount; i++) {
			// if we are just starting, set to StartingTile and continue the loop
			if (tile == null) {
				tile = StartingTile;
				continue;
			}
			// if there aren't any remaining tiles, invalid move
			else if (tile.NextTiles == null || tile.NextTiles.Length == 0) {
				Debug.Log ("Invalid move because not enough tiles left");
				return null;
			}

			// step through next tile
			// normal paths without forks
			if (tile.NextTiles.Length == 1) {
				tile = tile.NextTiles [0];
			} else {
				// choose the path that matches player turn (zero based so this works!)
				tile = tile.NextTiles [gameState.PlayerTurn];
			}
		}

		return tile;
	}
}
