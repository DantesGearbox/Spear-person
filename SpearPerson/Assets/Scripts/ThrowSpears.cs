using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSpears : MonoBehaviour {

	VolgarrController controller;
	RaycastCollisionChecks colInfo;

	public GameObject spearPrefab;
	private GameObject spearHandler;

	private float throwStrength = 20.0f;

	private float throwTime = 0.1f;
	private float throwCounter = 0.0f;
	private bool throwingSpearNow = false;
	private float throwCooldownTime = 0.3f;
	private float throwCooldownCounter = 0.0f;
	private bool spearCooldown = false;
	private bool canThrowSpear = true;

	private bool canAirSpear = true;

	//Key input
	public KeyCode spearKey;

	void Start () {
		controller = GetComponentInParent<VolgarrController>();
		colInfo = GetComponentInParent<RaycastCollisionChecks>();
	}
	
	void Update () {

		//Air throw
		if (!colInfo.down) {
			if (Input.GetKeyDown(spearKey) && canAirSpear) {
				spearHandler = Instantiate(spearPrefab, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
				Rigidbody2D rb = spearHandler.GetComponent<Rigidbody2D>();
				rb.velocity = new Vector2(throwStrength * controller.direction, 0);

				canAirSpear = false;
			}
		}

		//Ground throw
		if (colInfo.down){
			canAirSpear = true;
			if (canThrowSpear) {
				if (Input.GetKeyDown(spearKey)) {
					throwingSpearNow = true;
					controller.throwingSpear = true;
					canThrowSpear = false;
				}
			}

			if (throwingSpearNow) {
				throwCounter += Time.deltaTime;
				if (throwCounter > throwTime) {
					spearHandler = Instantiate(spearPrefab, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
					Rigidbody2D rb = spearHandler.GetComponent<Rigidbody2D>();
					rb.velocity = new Vector2(throwStrength * controller.direction, 0);
					spearCooldown = true;
					throwingSpearNow = false;
					throwCounter = 0.0f;
				}
			}

			if (spearCooldown) {
				throwCooldownCounter += Time.deltaTime;
				if (throwCooldownCounter > throwCooldownTime) {
					canThrowSpear = true;
					spearCooldown = false;
					controller.throwingSpear = false;
					throwCooldownCounter = 0.0f;
				}
			}
		}
	}
}
