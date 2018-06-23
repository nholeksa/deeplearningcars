using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SensorScript : MonoBehaviour {

	public LayerMask LayerToSense;
	public SpriteRenderer Smiley;
	public float output {get;private set;}

	private const float MAX_DIST = 10f;
	private const float MIN_DIST = 0.01f;

	void Start () 
	{
		Smiley.gameObject.SetActive(true);
	}
	void FixedUpdate() 
	{
		Vector2 direction = Smiley.transform.position - this.transform.position;
		direction.Normalize();

		RaycastHit2D hit = Physics2D.Raycast(this.transform.position,direction,MAX_DIST,LayerToSense);

		if (hit.collider == null) 
		{
			//Debug.Log("no wall");
			hit.distance = MAX_DIST;
		}
		else if(hit.distance < MIN_DIST) 
		{
			Debug.Log("found something");
			hit.distance = MIN_DIST;
		}

		output = hit.distance/MAX_DIST;
		Smiley.transform.position = (Vector2) this.transform.position + direction * hit.distance;



	}
	public void Hide()
	{
		Smiley.gameObject.SetActive(false);
	}
		
}
