using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStone : MonoBehaviour {

	public BoardTile StartingTile;

	private BoardTile currentTile;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnMouseUp() {
		Debug.Log ("Clicked!");

		BoardTile nextTile;
		if (currentTile == null) {
			nextTile = StartingTile;
		} else {
			nextTile = currentTile.NextTiles[0];
		}

		transform.position = nextTile.gameObject.transform.position;
		currentTile = nextTile;
	}
}
