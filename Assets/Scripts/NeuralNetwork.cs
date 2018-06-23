using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NeuralNetwork : IComparable<NeuralNetwork> {

	private uint[] layers;
	private float[][] neurons;
	//each neuron stores the weights coming into it
	private float[][][] weights;

	private float fitness;
	private const float MUTATION_RATE = 0.8f;


	//neural network constructor
	public NeuralNetwork(uint[] n_layers)
	{
		this.layers = new uint[n_layers.Length];

		//copy the layers from the input
		for (int i = 0; i < n_layers.Length; i++) 
		{
			this.layers [i] = n_layers [i];
		}

		// initialize weights and neurons
		initNeurons ();
		initWeights ();
	}
	//copy other network into this network
	public NeuralNetwork(NeuralNetwork other)
	{
		//copy the size of each layer
		this.layers = new uint[other.layers.Length];
		for (int i = 0; i < this.layers.Length; i++)
		{
			this.layers[i] = other.layers[i];
		}
		//initialize neurons based on the layers
		initNeurons();
		initWeights();

		//copy other's current weights into the new neural network
		copyWeights(other.weights);

	}
	// initialize neurons
	private void initNeurons() 
	{
		//create a list to make a variable length 2D array
		List<float[]> neuronList = new List<float[]>();

		// Add an array of size of layers[i] + 1 for each layer
		// + 1 to include a bias neuron
		for (int i = 0; i < this.layers.Length; i++) 
		{
			neuronList.Add(new float[this.layers[i] + 1]);
		}

		//convert list to array
		neurons = neuronList.ToArray();

		//initialize bias neurons to 1.0
		for (int i = 0;i<this.layers.Length;i++)
		{
			neurons[i][this.layers[i]] = 1.0f;
		}

	}

	private void initWeights()
	{
		//create a list to make a variable length 3D array
		List<float[][]> weightList = new List<float[][]>();

		//skip the input layer as there are no connections into it
		for (int i = 1; i < this.layers.Length; i++) 
		{
			List<float[]> layerWeightList = new List<float[]> ();

			uint neuronsInCurrentLayer = (uint) neurons [i].Length;

			//skip bias neuron as it has no incoming weights
			for (int j = 0; j < neuronsInCurrentLayer - 1; j++) 
			{
				uint neuronsInPreviousLayer = (uint) neurons [i - 1].Length;
				//initialize an array to store current neurons' weights
				float[] neuronWeights = new float[neuronsInPreviousLayer];

				//loop through neurons from previous layer
				for (int k = 0; k < neuronsInPreviousLayer; k++)
				{
					//assign random numbers betwenn -1 and 1 to be the initial weights
					neuronWeights [k] = UnityEngine.Random.Range (-1f, 1f);
				}
			
				//add neuron weights from current neuron to the list of all neuron weights in the layer
				layerWeightList.Add (neuronWeights);
				
			}
			//add layer weights to the weight list
			weightList.Add (layerWeightList.ToArray ());

		}

		this.weights = weightList.ToArray ();

	}
	public float[] FeedForward(float[] inputs)
	{

		//put inputs into the network
		for (int i = 0; i < inputs.Length; i++)
		{
			neurons [0] [i] = inputs [i];
		}
		//iterate through layers, except for input layer
		for (int i = 1; i < layers.Length; i++) 
		{

			//iterate through each neuron in the layer
			for (int j = 0; j < neurons[i].Length - 1; j++) 
			{
				float sum = 0f;
				//iterate through each of the neurons in the previous layer
				for (int k = 0; k < neurons[i-1].Length; k++) 
				{
					//look at the connection between the jth neuron and the kth neuron
					sum += neurons[i-1][k] * weights[i-1][j][k];

				}
				//normalize the sum

				neurons[i][j] = (float) Math.Tanh(sum);

			}

		}
		//return the output layer
		return this.neurons [this.neurons.Length - 1];

	}
	private void copyWeights(float[][][] other)
	{
		

		for(int i = 0; i<other.Length; i++)
		{
			

			for (int j = 0; j < other [i].Length; j++) 
			{
				for (int k = 0;k< other[i][j].Length;k++)
				{
					this.weights[i][j][k] = other[i][j][k];
				}

			}

		}



	}
	//mutate the weights of a network
	public void Mutate()
	{
		for (int i = 0; i < this.weights.Length; i++) 
		{
			for (int j = 0; j < this.weights [i].Length; j++) 
			{
				for (int k = 0; k < this.weights[i][j].Length; k++)
				{
					//get current weight
					float weight = weights[i][j][k];

					//generate random number
					float randomNum = UnityEngine.Random.Range(0.0f,100.0f);

					if (randomNum < 0.25f * MUTATION_RATE) weight *= -1;
					else if (randomNum < 0.50f * MUTATION_RATE) weight = UnityEngine.Random.Range(0f,1f);
					else if (randomNum < 0.75f * MUTATION_RATE)
					{
						float factor = UnityEngine.Random.Range(0f,1f) + 1f;
						weight *= factor;
					}
					else if (randomNum < MUTATION_RATE)
					{
						float factor = UnityEngine.Random.Range(0f,1f);
						weight *= factor;
					}

					weights[i][j][k] = weight;
						
				}

			}

		}

	}


	public float GetFitness()        {return this.fitness;}
	public void SetFitness(float x)  {this.fitness = x;   }
	public void AddFitness (float x) {this.fitness += x;  }

	public int CompareTo(NeuralNetwork other)
	{
		if (other == null) return 1;
		else if (this.fitness > other.fitness) return 1;
		else if (this.fitness < other.fitness) return -1;
		else return 0;
	}
}
