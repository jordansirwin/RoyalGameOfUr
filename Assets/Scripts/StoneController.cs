using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour {

	public TileController StartingTile;

	private TileController currentTile;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnMouseUp() {
		Debug.Log ("Clicked!");

		TileController nextTile;
		if (currentTile == null) {
			nextTile = StartingTile;
		} else {
			nextTile = currentTile.NextTiles[0];
		}

		transform.position = nextTile.gameObject.transform.position;
		currentTile = nextTile;
	}
}
