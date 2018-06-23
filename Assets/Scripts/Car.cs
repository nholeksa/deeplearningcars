using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Car : MonoBehaviour, IComparable<Car> {

	private SensorScript[] sensors;
	private bool initialized = false;
	//where is it trying to go
	private NeuralNetwork net;
	private Rigidbody2D rBody;
	public bool isAlive = true;

	private float CAR_SPEED = 10f;
	private const float ROTATION_SPEED = 200f;

	// Use this for initialization
	void Start () {
		rBody = GetComponent<Rigidbody2D>();
		sensors = GetComponentsInChildren<SensorScript>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(isAlive)
		{
			//SOME AMOUNT OF RAYCASTED POINTS
			float[] inputs = new float[5];

			for (int i = 0; i<5;i++)
			{
				inputs[i] = sensors[i].output;
			}




			//FEED THEM FORWARD
			float[] outputs = net.FeedForward(inputs);



			//APPLY OUTPUTS: 0 is speed, 1 is rotation
			rBody.velocity = outputs[0] * CAR_SPEED * transform.up;

			rBody.angularVelocity = outputs[1] * ROTATION_SPEED;


			//UPDATE FITNESS OF CAR
			net.AddFitness(rBody.velocity.magnitude * Time.deltaTime);


		}
	}
	private void OnCollisionEnter2D(Collision2D collider)
	{
		if (collider.gameObject.tag == "Wall")
		{
			Die();
		}
	}
	public float getVelocity() {return rBody.velocity.magnitude;}
	public float getAngularVelocity() {return rBody.angularVelocity;}


	public void Initialize(NeuralNetwork inputNet)
	{
		this.net = inputNet;
		this.initialized = true;
	}
	public int CompareTo(Car other)
	{
		if(other == null) return 1;
		else if(this.GetFitness() > other.GetFitness()) return 1;
		else if(this.GetFitness() < other.GetFitness()) return -1;
		else return 0;
	}
	public float GetFitness()
	{
		return this.net.GetFitness();
	}
	private void Die()
	{
		isAlive = false;
		rBody.velocity = new Vector2(0f, 0f);
		rBody.gravityScale = 0f;
		rBody.angularVelocity = 0f;
		for (int i = 0; i < 5;i++)
		{
			sensors[i].Hide();
		}
				
	}
	
}
