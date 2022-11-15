using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class TrainManager : MonoBehaviour
{
    public DrawNeuralNetwork drawNeuralNetwork;
    public GameObject individual;
    public GameObject[] individualClones;
    public float[] topScores;
    public float[,] topGenomes;
    public Transform spawnPoint;
    [SerializeField] private int nIndividuals;

    public int[] topScorsIndexes;

    public int finishedIndividuals = 0;

    public bool allDone = true;

    public int bestSelectionCount = 3;

    public float highestLastAverageScore = 0;

    public int numberOfGenerations = 1;

    public TextMeshProUGUI generationsText;

    private int genomeLenght = 24;

    public GameObject finishLine;

    public int inputLength;
    public int hiddenLength;
    public int outputLength;


    // Start is called before the first frame update
    void Start()
    {
        topGenomes = new float[bestSelectionCount, genomeLenght];
        topScores = new float[bestSelectionCount];
        topScorsIndexes = new int[bestSelectionCount];

        individualClones = new GameObject[nIndividuals];
        for(int i = 0; i < nIndividuals; i++)
        {
            individualClones[i] = GameObject.Instantiate(individual, spawnPoint.position, spawnPoint.rotation);
        }
    }

    private void Update()
    {
        for(int i = 0; i < individualClones.Length; i++)
        {
            allDone = true;
            if (!individualClones[i].GetComponent<Brain>().isDone)
            {
                allDone = false;
                break;
            }
        }

        if (allDone)
        {
            finishLine.GetComponent<ActivateFinishLine>().counter = 5;

            int numberOfPickedIndexes = 0;
            int[] alreadyPickedIndexes = new int[individualClones.Length];

            for(int i = 0; i < individualClones.Length; i++)
            {
                if(individualClones[i].GetComponent<Brain>().lastScore > topScores[getSmallestNumberIndex()] && !checkNumberInArray(i, alreadyPickedIndexes))
                {
                    int index = getSmallestNumberIndex();
                    topScores[index] = individualClones[i].GetComponent<Brain>().lastScore;
                    for(int j = 0; j < 24; j++)
                    {
                        topGenomes[index, j] = individualClones[i].GetComponent<Brain>().weights[j];
                    }
                    alreadyPickedIndexes[numberOfPickedIndexes] = i;
                    numberOfPickedIndexes++;
                }
            }

            numberOfGenerations++;
            generationsText.text = "Generation: " + numberOfGenerations.ToString();


            for (int i = 0; i < individualClones.Length; i++)
            {
                
                individualClones[i].transform.position = spawnPoint.position;
                individualClones[i].transform.rotation = spawnPoint.rotation;
                individualClones[i].gameObject.SetActive(true);
                individualClones[i].GetComponent<Brain>().enableLines();

                for (int j = 0; j < genomeLenght; j++)
                {
                    int parentGenomeIndex = Random.Range(0, bestSelectionCount);
                    float[] parentGenome = new float[genomeLenght];

                    for(int z = 0; z < genomeLenght; z++)
                    {
                        parentGenome[z] = topGenomes[parentGenomeIndex, z];
                    }
                    
                    int random = Random.Range(0, 2);
                    if(random == 0)
                    {
                        generateCrossOverGenom(parentGenome, individualClones[i]);
                    }
                    else
                    {
                        generateCrossOverGenom2(parentGenome, individualClones[i]);
                    }
                    
                    individualClones[i].GetComponent<Brain>().stopped = false;
                    individualClones[i].GetComponent<Brain>().isDone = false;
                    individualClones[i].GetComponent<Brain>().scoreTimer = 0;
                    individualClones[i].GetComponent<Brain>().timesCrossedTheFinish = 0;

                }
            }
            //drawNeuralNetwork.drawNeuralNetwork(getBestGenome(), 1, inputLength, hiddenLength, outputLength);
        }
    }

    public int getSmallestNumberIndex()
    {
        float min = Mathf.Infinity;
        int minIndex = -1;
        for(int i = 0; i < topScores.Length; i++)
        {
            if(topScores[i] < min)
            {
                min = topScores[i];
                minIndex = i;
            }
        }

        return minIndex;
    }

    public float calculateAverage()
    {
        float total = 0;
        for(int i = 0; i < bestSelectionCount; i++)
        {
            total += individualClones[i].GetComponent<Brain>().lastScore;
        }

        return total / bestSelectionCount;
    }

    public float calculateAverage2()
    {
        float total = 0;
        for (int i = 0; i < bestSelectionCount; i++)
        {
            total += topScores[i];
        }

        return total / bestSelectionCount;
    }

    public bool checkNumberInArray(int number, int[]array)
    {
        for(int i = 0; i < array.Length; i++)
        {
            if(number == array[i])
            {
                return true;
            }
        }
        return false;
    }

    private void generateCrossOverGenom(float[] parentGenome, GameObject individual)
    {
        int random1;
        int random2;


        int max;
        int min;


        do
        {
            random1 = Random.Range(0, genomeLenght);
            random2 = Random.Range(0, genomeLenght);

            if (random1 > random2)
            {
                max = random1;
                min = random2;
            }
            else
            {
                max = random2;
                min = random1;
            }
        } while (max - min < 3);




        for (int i = 0; i < min; i++)
        {
            individual.GetComponent<Brain>().lastGanome[i] = Random.Range(-2f, 2f);
        }

        for (int i = min; i < max; i++)
        {
            individual.GetComponent<Brain>().lastGanome[i] = parentGenome[i];
        }

        for (int i = max; i < genomeLenght; i++)
        {
            individual.GetComponent<Brain>().lastGanome[i] = Random.Range(-2f, 2f);
        }
    }
    private void generateCrossOverGenom2(float[] parentGenome, GameObject individual)
    {
        int random1 = 0;

        random1 = Random.Range(4, 24);

        int random2 = Random.Range(0, 2);

        if(random2 == 0)
        {
            for (int i = 0; i < random1; i++)
            {
                individual.GetComponent<Brain>().lastGanome[i] = parentGenome[i];
            }

            for (int i = random1; i < genomeLenght; i++)
            {
                individual.GetComponent<Brain>().lastGanome[i] = Random.Range(-2f, 2f);
            }
        }
        else
        {
            for (int i = random1; i < genomeLenght; i++)
            {
                individual.GetComponent<Brain>().lastGanome[i] = parentGenome[i];
            }

            for (int i = 0; i < random1; i++)
            {
                individual.GetComponent<Brain>().lastGanome[i] = Random.Range(-2f, 2f);
            }
        }
        
    }

    private float[] getBestGenome()
    {
        float[] bestGenome = new float[genomeLenght];
        for(int i = 0; i < genomeLenght; i++)
        {
            bestGenome[i] = topGenomes[1, i];
        }

        return bestGenome;
    }
}
