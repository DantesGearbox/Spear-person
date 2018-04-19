using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

	public GameObject platformPrefab;
	public GameObject platformHandler;
	private float timeLimit = 0.03f;

	private void OnTriggerEnter2D(Collider2D collision) {
		
		if(collision.tag == "Spear"){
			if(collision.gameObject.GetComponent<Spear>().time > timeLimit){
				platformHandler = Instantiate(platformPrefab, collision.transform.position, collision.transform.rotation) as GameObject;
			}
			Destroy(collision.gameObject);
		}
	}
}



/*
 * -----Spear-platform problems-----
 * 
 * 
 * 
 */
