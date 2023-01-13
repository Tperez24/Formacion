
using System;
using System.Collections.Generic;
using Random = System.Random;

namespace Neuronal_Network
{
    public class NeuronalNetwork : IComparable<NeuronalNetwork>
    {
        private int[] _layers;
        private float[][] _neurons;
        private float[][][] _weights;
        private float _fitness;
        
        public NeuronalNetwork(IReadOnlyList<int> layers)
        {
            _layers = new int[layers.Count];
            for (var i = 0; i < layers.Count; i++)
            {
                _layers[i] = layers[i];
            }

            InitNeurons();
            InitWeights();
        }

        /// <summary>
        /// Deep copy constructor
        /// </summary>
        /// <param name="copyNetwork">Network to deep copy</param>
        public NeuronalNetwork(NeuronalNetwork copyNetwork)
        {
            _layers = new int[copyNetwork._layers.Length];
            for (var i = 0; i < copyNetwork._layers.Length; i++)
            {
                _layers[i] = copyNetwork._layers[i];
            }
            
            InitNeurons();
            InitWeights();
            CopyWeights(copyNetwork._weights);
        }

        private void CopyWeights(float[][][] copyWeights)
        {
            for (int i = 0; i < _weights.Length; i++)
            {
                for (int j = 0; j < _weights[i].Length; j++)
                {
                    for (int k = 0; k < _weights[i][j].Length; k++)
                    {
                        _weights[i][j][k] = copyWeights[i][j][k];
                    }
                }
            }
        }

        public void AddFitness(float fit) => _fitness += fit;

        public void SetFitness(float fit) => _fitness = fit;

        public float GetFitness() => _fitness;
        private void InitNeurons()
        {
            var neuronsList = new List<float[]>();

            for (var i = 0; i < _layers.Length; i++)
            {
                neuronsList.Add(new float[_layers[i]]);
            }

            _neurons = neuronsList.ToArray();
        }

        private void InitWeights()
        {
            var weightsList = new List<float[][]>();
            
            for (var i = 0; i < _layers.Length; i++)
            {
                var layerWeightList = new List<float[]>();
                var neuronsInPreviousLayers = _layers[i - 1];

                for (var j = 0; j < _neurons[i].Length; j++)
                {
                    var neuronWeights = new float[neuronsInPreviousLayers];
                    for (var k = 0; k < neuronsInPreviousLayers; k++)
                    {
                        neuronWeights[k] = UnityEngine.Random.Range(-0.5f,0.5f);
                    }
                    layerWeightList.Add(neuronWeights);
                }
                
                weightsList.Add(layerWeightList.ToArray());
            }

            _weights = weightsList.ToArray();
        }
        
        public float[] FeedForward(float[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                _neurons[0][i] = inputs[i];
            }

            for (var i = 0; i < _layers.Length; i++)
            {
                for (int j = 0; j < _neurons[i].Length; j++)
                {
                    //bias
                    float value = 0.25f;
                    
                    for (int k = 0; k < _neurons[i-1].Length; k++)
                    {
                        value += _weights[i - 1][j][k] * _neurons[i - 1][k];
                    }

                    _neurons[i][j] = (float)Math.Tanh(value);
                }
            }

            return _neurons[^1];
        }

        public void Mutate()
        {
            for (int i = 0; i < _weights.Length; i++)
            {
                for (int j = 0; j < _weights[i].Length; j++)
                {
                    for (int k = 0; k < _weights[i][j].Length; k++)
                    {
                        var weight = _weights[i][j][k];

                        var randomNumber = UnityEngine.Random.Range(-0.5f,0.5f) * 1000f;

                        switch (randomNumber)
                        {
                            case <= 2f:
                                weight *= -1f;
                                break;
                            case <=4f:
                                weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                                break;
                            case <=6f:
                                weight *= UnityEngine.Random.Range(0, 1f) +1f;
                                break;
                            case <=8f:
                                weight *= UnityEngine.Random.Range(0, 1f);
                                break;
                        }

                        _weights[i][j][k] = weight;
                    }
                }
            }
        }

        public int CompareTo(NeuronalNetwork other)
        {
            if (other == null) return 1;
            return 0;
        }
    }
}
