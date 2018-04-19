using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour {

	[HideInInspector] public float time = 0.0f;
	
	void Update () {
		time += Time.deltaTime;
	}
}
