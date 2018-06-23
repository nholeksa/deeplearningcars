using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimulationManager : MonoBehaviour {

	public GameObject carPrefab;
	//public Text GenerationText;
	//public Text ThrustText;
	//public Text RotationText;
	public CameraController camController;


	private int generation = 0;
	private uint[] netLayers = new uint[] {5,10,10,2};
	private List<NeuralNetwork> netList;
	private List<Car> carList = null;
	private bool isTraining = false;

	private const int POPULATION_SIZE = 10;
	private const float TRAINING_TIME = 15;
	private const float GRADIENT_FACTOR = 10f;

	//Ends the generation
	void Timer() { isTraining = false;}

	// Update is called once per frame
	void Update () {
		//if new generation
		if (isTraining == false)
		{
			//if its the first generation, intialize the neural nets
			if (generation == 0)
			{
				InitializeCarNetworks();
			}
			//otherwise update the neural nets
			else
			{
				//Take the most fit half of the networks and apply them to the least fit half
				netList.Sort();
				for (int i = 0; i < POPULATION_SIZE/2; i++)
				{
					netList[i] = new NeuralNetwork(netList[i + (POPULATION_SIZE/2)]);
				}

				//Reset Fitness of each network
				for (int i = 0; i < POPULATION_SIZE; i++)
				{
					netList[i].Mutate();
					netList[i].SetFitness(0f);
				}


			}

			Debug.Log(generation);
			generation++;
			//UpdateGenerationText();
			isTraining = true;
			Invoke("Timer",TRAINING_TIME);
			CreateCars();
		}
		//Get most fit car
		carList.Sort();
		Car mostFit = carList[POPULATION_SIZE-1];

		//Set camera to follow most fit car
		camController.SetPlayer(mostFit);

		if(Input.GetMouseButtonDown(0))
		{
			isTraining = false;
		}


		//Update car speed and rotation
		//UpdateThrustText(mostFit);
		//UpdateRotationText(mostFit);

	}
	
	//Updates Generation Text
	/*void UpdateGenerationText () {
		GenerationText = generation.ToString();
	}
	void UpdateThrustText(Car mostFit)
	{
		ThrustText = mostFit.getVelocity().ToString();
	}
	void UpdateRotationText(Car mostFit)
	{
		RotationText = mostFit.getAngularVelocity().ToString();
	}*/
	private void CreateCars()
	{
		//if there is a prexisting carList, destory all the objects
		if (carList != null)
		{
			for (int i = 0; i < POPULATION_SIZE; i++)
			{
				GameObject.Destroy(carList[i].gameObject);
			}
		}

		carList = new List<Car>();


		//FIGURE OUT THE STARTING POSITION FOR THE CARS!!!!!!
		for (int i = 0; i< POPULATION_SIZE; i++ )
		{
			//CREATE A CAR OBJECT AT THE POSITION
			Car c1 = ((GameObject)Instantiate(carPrefab,new Vector3(0,0,0),carPrefab.transform.rotation)).GetComponent<Car>();

			//INITIALIZE THE CAR WITH A NEURAL NET AND ???TARGET???
			c1.Initialize(netList[i]);
			carList.Add(c1);
		}
	}
	//initialize all of the neural networks
	private void InitializeCarNetworks()
	{
		//create a list
		netList = new List<NeuralNetwork>();

		for(int i = 0; i < POPULATION_SIZE; i++)
		{
			//create the net objects and add to list
			NeuralNetwork net = new NeuralNetwork(netLayers);
			netList.Add(net);
		}


	}
}
