using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayer : MonoBehaviour {

	public Transform player;
	public Transform playerStartingPosition;



	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {

			player.position = playerStartingPosition.position;
		}
	}
}
