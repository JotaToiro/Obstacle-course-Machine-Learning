using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Brain : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject trainManager;
    public Transform spawnPoint;

    [SerializeField] private float rotateSpeed;
    [SerializeField] private float movementSpeed;

    public GameObject rayCaster1;
    public GameObject rayCaster2;
    public GameObject rayCaster3;
    //public GameObject rayCaster4;
    public GameObject rayCaster5;
    public GameObject rayCaster6;

    public TextMeshProUGUI distance1;
    public TextMeshProUGUI distance2;
    public TextMeshProUGUI distance3;
    public TextMeshProUGUI distance4;
    public TextMeshProUGUI distance5;
    public TextMeshProUGUI distance6;

    private float sigmoidXLimit = 0f;

    public float[] inputLayer = new float[5];
    private float[] hidenLayer = new float[4];
    private int outputLayer;

    public float[] weights = new float[24];
    private float[] genomToMutate = new float[24];

    public bool stopped = false;

    public float currentScore = 0;

    public float scoreTimer = 0f;

    //public float[] lastScores;
    //private float[,] lastGenomes;
    public float lastScore = 0;
    public float[] lastGanome = new float[24];

    private bool rounsEnded = false;

    [SerializeField] private int numberOfTries = 25;

    private bool firstRun = true;

    public bool isDone = false;

    private float timer = 2;

    private int genomeLenght = 24;

    public int timesCrossedTheFinish = 0;

    public string outputString = "";



    private void Start()
    {
        rayCaster1 = transform.GetChild(0).gameObject;
        rayCaster2 = transform.GetChild(1).gameObject;
        rayCaster3 = transform.GetChild(2).gameObject;
        //rayCaster4 = transform.GetChild(3).gameObject;
        rayCaster5 = transform.GetChild(4).gameObject;
        rayCaster6 = transform.GetChild(5).gameObject;
        initializeWeights();
        trainManager = GameObject.Find("TrainManager");
        //lastScores = new float[numberOfTries];
        //lastGenomes = new float[numberOfTries, 24];
        inputLayer = new float[trainManager.GetComponent<TrainManager>().inputLength];
        hidenLayer = new float[trainManager.GetComponent<TrainManager>().hiddenLength];

    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (!stopped && timer < 0)
        {
            scoreTimer += Time.deltaTime;

            inputLayer[0] = rayCaster1.GetComponent<DrawRayCast>().distance;
            inputLayer[1] = rayCaster2.GetComponent<DrawRayCast>().distance;
            inputLayer[2] = rayCaster3.GetComponent<DrawRayCast>().distance;
            //inputLayer[3] = rayCaster4.GetComponent<DrawRayCast>().distance;
            inputLayer[3] = rayCaster5.GetComponent<DrawRayCast>().distance;
            inputLayer[4] = rayCaster6.GetComponent<DrawRayCast>().distance;

            /*
            RaycastHit objectHit1;
            if (Physics.Raycast(rayCaster1.position, rayCaster1.forward, out objectHit1, 50, layerMask))
            {
                inputLayer[0] = objectHit1.distance;
                //distance2.text = inputLayer[1].ToString();
            }



            RaycastHit objectHit2;
            if (Physics.Raycast(rayCaster2.position, rayCaster2.forward, out objectHit2, 50, layerMask))
            {
                inputLayer[1] = objectHit2.distance;
                //distance2.text = inputLayer[1].ToString();
            }

            RaycastHit objectHit3;
            if (Physics.Raycast(rayCaster3.position, rayCaster3.forward, out objectHit3, 50, layerMask))
            {
                inputLayer[2] = objectHit3.distance;
                //distance3.text = inputLayer[2].ToString();
            }

            RaycastHit objectHit4;
            if (Physics.Raycast(rayCaster4.position, rayCaster4.forward, out objectHit4, 50, layerMask))
            {
                inputLayer[3] = objectHit4.distance;
                //distance4.text = inputLayer[3].ToString();
            }

            RaycastHit objectHit5;
            if (Physics.Raycast(rayCaster5.position, rayCaster5.forward, out objectHit5, 50, layerMask))
            {
                inputLayer[4] = objectHit5.distance;
                //distance4.text = inputLayer[3].ToString();
            }
            */



            int output = calculateOutPut();
            outputLayer = output;

            switch (output)
            {
                case 0:
                    outputString = "Left";
                    break;
                case 1:
                    outputString = "Right";
                    break;
                case 2:
                    outputString = "Forward";
                    break;

            }

            //bug.Log(output);

        
            if (output == 0)
            {
                this.transform.Rotate(0, Time.deltaTime * (-rotateSpeed), 0);
            }
            else
            {
                if(output == 1)
                {
                    this.transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);
                }
            }
            this.transform.position += transform.forward * Time.deltaTime * movementSpeed;

            float aux = (inputLayer[0] + inputLayer[1] + (inputLayer[2] * 1f) + inputLayer[3] + inputLayer[4]) / 5;
            currentScore += aux;
        }
    }

    private int calculateOutPut()
    {
        int x = 0;
        for(int i = 0; i < hidenLayer.Length; i++)
        {
            float value = 0;
            for (int j = 0; j < inputLayer.Length; j++)
            {
                value += inputLayer[j] * weights[x];
                x++;
            }
            hidenLayer[i] = activationFunction(value);
        }

        float valueOutput = 0;
        for(int i = 0; i < hidenLayer.Length; i++)
        {
            valueOutput += hidenLayer[i] * weights[x];
            x++;
        }
        //Debug.Log(valueOutput);
        //outputLayer = (valueOutput < 3.7f) ? 0 : 1;
        if(valueOutput < -0.5)
        {
            return 0;
        }
        else
        {
            if(valueOutput > 0.5)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
        

        //return outputLayer;
    }

    private float activationFunction(float x)
    {
        return 1 / (1 + Mathf.Exp(-x + sigmoidXLimit));
    }

    public void initializeWeights()
    {
        for(int i = 0; i < weights.Length; i++)
        {
            this.weights[i] = Random.Range(-2f, 2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "finish")
        {
            //currentScore += 5000;
            
            if (timesCrossedTheFinish < 2)
            {
                if(timesCrossedTheFinish == 0)
                {
                    currentScore += 0;
                }
                else
                {
                    currentScore += other.transform.parent.GetComponent<CheckpointScore>().getPoints(scoreTimer, this.gameObject);
                }

                timesCrossedTheFinish++;
            }
            else
            {
                currentScore += other.transform.parent.GetComponent<CheckpointScore>().getPoints(scoreTimer, this.gameObject);
                stopGame();
            }
        }

        if(other.transform.tag == "Checkpoint")
        {
            currentScore += other.transform.parent.GetComponent<CheckpointScore>().getPoints(scoreTimer, this.gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody.transform.GetComponent<isObstacle>() != null)
        {
            //print("entrou");
            stopGame();
            //trainManager.GetComponent<TrainManager>().addNewGenomeToTop(currentScore, weights);
            //trainManager.GetComponent<TrainManager>().finishedIndividuals += 1;

            //Destroy(this.gameObject);
        }

        
    }

    public void stopGame()
    {

        
        if (stopped == false && currentScore != 0)
        {
            
            disableLines();
            lastScore = currentScore;
            lastGanome = weights;
            currentScore = 0;
            stopped = true;
            isDone = true;
            //GetComponent<Renderer>().enabled = false;
            gameObject.SetActive(false);
        }
        
        /*
        if (stopped == false && currentScore != 0)
        {
            isDone = true;
            stopped = true;
            lastScores[currNumberOfTries] = currentScore;

            

            for (int i = 0; i < 24; i++)
            {
                lastGenomes[currNumberOfTries, i] = weights[i];
            }

            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
            //transform.position = trainManager.GetComponent<TrainManager>().spawnPoint.position;
            //transform.rotation = trainManager.GetComponent<TrainManager>().spawnPoint.rotation;
            //mutate1();
            currNumberOfTries += 1;
            stopped = false;
            currentScore = 0;
            if(currNumberOfTries >= numberOfTries)
            {
                int index = checklargestnumber(lastScores);
                for(int i = 0; i < 24; i++)
                {
                    genomToMutate[i] = lastGenomes[index,i];
                }
                if(lastScores[index] > bestScore)
                {
                    bestScore = lastScores[index];
                    for(int i = 0; i < 24; i++)
                    {
                        bestGenome[i] = lastGenomes[index,i];
                    }
                }
                generateCrossOverGenom(genomToMutate);
                currNumberOfTries = 0;
                for(int i = 0; i < lastScores.Length; i++)
                {
                    lastScores[i] = 0f;
                }
            }
            if (firstRun)
            {
                initializeWeights();
            }
            else
            {
                generateCrossOverGenom(genomToMutate);
            }
        }
        */
    }

    private void generateCrossOverGenom(float[] parentGenome)
    {
        int random1 = 0;
        int random2 = 0;
        while (Mathf.Max(random1, random2) - Mathf.Min(random1, random2) < 3)
        {
            random1 = Random.Range(0, genomeLenght);
            random2 = Random.Range(0, genomeLenght);
        }

        int min = Mathf.Min(random1, random2);
        int max = Mathf.Max(random1, random2);

        for(int i = 0; i < min; i++)
        {
            weights[i] = Random.Range(0f, 2f);
        }

        for (int i = min; i < max; i++)
        {
            weights[i] = parentGenome[i];
        }

        for (int i = max; i < genomeLenght; i++)
        {
            weights[i] = Random.Range(0f, 2f);
        }
    }

    private void mutate1()
    {
        int random = Random.Range(0, weights.Length - 1);
        float[] aux = new float[weights.Length];
        for (int i = random; i < weights.Length; i++)
        {
            aux[i - random] = weights[i];
        }
        for (int i = 0; i < random; i++)
        {
            aux[weights.Length - random + i] = weights[i];
        }
        weights = aux;
    }

    private int checklargestnumber(float[] array)
    {
        float largestNumber = Mathf.NegativeInfinity;
        int currPosition = -1;
        for(int i = 0; i < array.Length; i++)
        {
            if(array[i] > largestNumber)
            {
                currPosition = i;
                largestNumber = array[i];
            }
        }
        return currPosition;
    }

    public void disableLines()
    {
        rayCaster1.GetComponent<LineRenderer>().enabled = false;
        rayCaster2.GetComponent<LineRenderer>().enabled = false;
        rayCaster3.GetComponent<LineRenderer>().enabled = false;
        rayCaster5.GetComponent<LineRenderer>().enabled = false;
        rayCaster6.GetComponent<LineRenderer>().enabled = false;
    }

    public void enableLines()
    {
        rayCaster1.GetComponent<LineRenderer>().enabled = true;
        rayCaster2.GetComponent<LineRenderer>().enabled = true;
        rayCaster3.GetComponent<LineRenderer>().enabled = true;
        rayCaster5.GetComponent<LineRenderer>().enabled = true;
        rayCaster6.GetComponent<LineRenderer>().enabled = true;
    }
}