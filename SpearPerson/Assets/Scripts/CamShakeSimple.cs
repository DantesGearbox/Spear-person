using UnityEngine;
using System.Collections;

public class CamShakeSimple : MonoBehaviour {
	
	Vector3 originalCameraPosition = new Vector3(0,0,-10);
	private float shakeAmt = 0.01f;
	public Camera mainCamera;

	public void ShakeCam(){
		InvokeRepeating("CameraShake", 0, .01f);
		Invoke("StopShaking", 5.0f);
	}

	void CameraShake()
	{
		if(shakeAmt>0) 
		{
			float quakeAmt = Random.value*shakeAmt*2 - shakeAmt;
			Vector3 pp = mainCamera.transform.position;
			pp.y+= quakeAmt; // can also add to x and/or z
			//pp.x+= quakeAmt;
			mainCamera.transform.position = pp;
		}
	}

	void StopShaking()
	{
		CancelInvoke("CameraShake");
		mainCamera.transform.position = originalCameraPosition;
	}

}
