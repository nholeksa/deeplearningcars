using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


	private Car player;
	public GameObject mainCamera;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame, runs after all items processed in update
	void LateUpdate () {
		if (player != null)
		{
			mainCamera.transform.position = player.transform.position + new Vector3(0, 0, -10);
		}
	}

	public void SetPlayer(Car p)
	{
		player = p;
	}
		
		
}