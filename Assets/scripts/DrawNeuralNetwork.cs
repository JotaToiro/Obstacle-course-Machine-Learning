using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DrawNeuralNetwork : MonoBehaviour
{
    public GameObject neuron;
    public GameObject neuronOutput;
    public GameObject neuronInput;
    [SerializeField]
    private float posX;
    [SerializeField]
    private float posy;
    [SerializeField]
    private float nextLayerIncrement;
    [SerializeField]
    private float nextNeuronIncrement;

    private int nHiddenLayers = 1;
    private int lengthInput;
    private int lengthHidden;
    private int lengthOutput;

    public GameObject linePrefeb;
    public Transform parent;

    public Gradient gradient;

    private GameObject[] lines;

    private bool isFirstGeneration = true;

    [SerializeField]
    private TrainManager trainManager;

    private float[] bestGenome;
    private GameObject bestIndividual;

    private TextMeshProUGUI outputText;
    private TextMeshProUGUI[] inputText;

    float timer = 0;
    float timeToRun = 1;

    private float[] inputDrawPositions;
    private Vector3[] inputNeuronsPositions;

    private float[] hiddenDrawPositions;
    private Vector3[] hiddenNeuronsPositions;

    private float[] outputDrawPositions;
    private Vector3 outputNeuronsPosition;


    //change car material vars
    public Material fadeWheel;
    public Material fadeBody;

    public Material normalWheel;
    public Material normalBody;

    public void Start()
    {
        lines = new GameObject[24];
        bestGenome = null;
        timer = 0;
        lengthInput = trainManager.inputLength;
        lengthHidden = trainManager.hiddenLength;
        lengthOutput = trainManager.outputLength;
        inputDrawPositions = getNeuronDrawPositions(lengthInput);
        inputNeuronsPositions = new Vector3[lengthInput];

        hiddenDrawPositions = getNeuronDrawPositions(lengthHidden);
        hiddenNeuronsPositions = new Vector3[lengthHidden];

        outputDrawPositions = getNeuronDrawPositions(lengthOutput);

        outputNeuronsPosition = new Vector3(posX + nextLayerIncrement * 2, posy, 10);

        inputText = new TextMeshProUGUI[lengthInput];

        createStructure();
    }

    public void Update()
    {
        float[] currBestGenome;
        GameObject currBestIndividual;
        (currBestGenome, currBestIndividual) = getBestGenome();
        if(currBestGenome != bestGenome)
        {
            bestGenome = currBestGenome;
            bestIndividual = currBestIndividual;
            changeCarsMaterial();
            updateNeuralNetwork(bestGenome);
        }

        if(bestIndividual != null && !isFirstGeneration)
        {
            outputText.text = bestIndividual.GetComponent<Brain>().outputString;
            for(int i = 0; i < lengthInput; i++)
            {
                inputText[i].text = bestIndividual.GetComponent<Brain>().inputLayer[i].ToString("F2");
            }
        }
    }
    public void changeCarsMaterial()
    {
        GameObject[] individuals = trainManager.individualClones;

        for(int i = 0; i < individuals.Length; i++)
        {
            if (individuals[i] != bestIndividual)
            {
                individuals[i].GetComponent<CarParts>().body.GetComponent<Renderer>().material = fadeBody;
                individuals[i].GetComponent<CarParts>().wheel1.GetComponent<Renderer>().material = fadeWheel;
                individuals[i].GetComponent<CarParts>().wheel2.GetComponent<Renderer>().material = fadeWheel;
                individuals[i].GetComponent<CarParts>().wheel3.GetComponent<Renderer>().material = fadeWheel;
                individuals[i].GetComponent<CarParts>().wheel4.GetComponent<Renderer>().material = fadeWheel;
            }
            else
            {
                individuals[i].GetComponent<CarParts>().body.GetComponent<Renderer>().material = normalBody;
                individuals[i].GetComponent<CarParts>().wheel1.GetComponent<Renderer>().material = normalWheel;
                individuals[i].GetComponent<CarParts>().wheel2.GetComponent<Renderer>().material = normalWheel;
                individuals[i].GetComponent<CarParts>().wheel3.GetComponent<Renderer>().material = normalWheel;
                individuals[i].GetComponent<CarParts>().wheel4.GetComponent<Renderer>().material = normalWheel;
            }
        }
    }

    public void updateNeuralNetwork(float[] bestGenome)
    {
        int x = 0;

        for (int i = 0; i < lengthHidden; i++)
        {
            for (int j = 0; j < lengthInput; j++)
            {
                if(isFirstGeneration)
                {
                    Vector2 midPoint = (inputNeuronsPositions[j] + hiddenNeuronsPositions[i]) * 0.5f;
                    Vector3 position = new Vector3(midPoint.x, midPoint.y, -10);
                    lines[x] = GameObject.Instantiate(linePrefeb, position, Quaternion.identity, parent);

                    Vector3 dir = getDir(inputNeuronsPositions[j], hiddenNeuronsPositions[i]);
                    float length = getLength(inputNeuronsPositions[j], hiddenNeuronsPositions[i]);

                    lines[x].transform.right = dir;
                    float weightValue = bestGenome[x] + 2;
                    lines[x].GetComponent<RectTransform>().sizeDelta = new Vector2(length, weightValue + 1);
                    lines[x].GetComponent<Image>().color = gradient.Evaluate(weightValue / 4);

                    lines[x].transform.SetSiblingIndex(0);
                }
                else
                {
                    float weightValue = bestGenome[x] + 2;
                    lines[x].GetComponent<RectTransform>().sizeDelta = new Vector2(lines[x].GetComponent<RectTransform>().sizeDelta.x, weightValue + 1);
                    lines[x].GetComponent<Image>().color = gradient.Evaluate(weightValue / 4);
                }
                
                x++;
            }
        }

        for (int i = 0; i < lengthHidden; i++)
        {
            if(isFirstGeneration)
            {
                Vector2 midPoint = (hiddenNeuronsPositions[i] + outputNeuronsPosition) * 0.5f;
                Vector3 position = new Vector3(midPoint.x, midPoint.y, 0);
                lines[x] = GameObject.Instantiate(linePrefeb, position, Quaternion.identity, parent);

                Vector3 dir = getDir(hiddenNeuronsPositions[i], outputNeuronsPosition);
                float length = getLength(hiddenNeuronsPositions[i], outputNeuronsPosition);

                lines[x].transform.right = dir;
                float weightValue = bestGenome[x] + 2;
                lines[x].GetComponent<RectTransform>().sizeDelta = new Vector2(length, weightValue +1);
                lines[x].GetComponent<Image>().color = gradient.Evaluate(weightValue / 4);

                lines[x].transform.SetSiblingIndex(0);
            }
            else
            {
                float weightValue = bestGenome[x] + 2;
                lines[x].GetComponent<RectTransform>().sizeDelta = new Vector2(lines[x].GetComponent<RectTransform>().sizeDelta.x, weightValue +1);
                lines[x].GetComponent<Image>().color = gradient.Evaluate(weightValue / 4);
            }
            x++;
        }
        isFirstGeneration = false;
    }

    private void createStructure()
    {
        GameObject inputLayer = new GameObject();
        inputLayer.transform.parent = parent;
        inputLayer.transform.position = new Vector2(posX, posy);
        GameObject hiddenLayer = new GameObject();
        hiddenLayer.transform.parent = parent;
        hiddenLayer.transform.position = new Vector2(posX + nextLayerIncrement, posy);
        GameObject outputLayer = new GameObject();
        outputLayer.transform.parent = parent;
        outputLayer.transform.position = new Vector2(posX + nextLayerIncrement * 2, posy);

        for (int i = 0; i < inputDrawPositions.Length; i++)
        {
            GameObject inputNeuron = GameObject.Instantiate(neuronInput, new Vector3(posX, inputDrawPositions[i], -1), Quaternion.identity, inputLayer.transform);
            inputNeuronsPositions[i] = inputNeuron.transform.position;
            inputText[i] = inputNeuron.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        for (int i = 0; i < hiddenDrawPositions.Length; i++)
        {
            hiddenNeuronsPositions[i] = GameObject.Instantiate(neuron, new Vector3(posX + nextLayerIncrement, hiddenDrawPositions[i], -1), Quaternion.identity, hiddenLayer.transform).transform.position;
        }
        GameObject outputNeuron = GameObject.Instantiate(neuronOutput, new Vector3(posX + nextLayerIncrement * 2, posy, -1), Quaternion.identity, outputLayer.transform);
        outputText = outputNeuron.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private float[] getNeuronDrawPositions(int nOfNeurons)
    {
        bool isEven = nOfNeurons % 2 == 0;
        float[] drawPositions = new float[nOfNeurons];
        if (nOfNeurons == 1)
        {
            drawPositions[0] = posy;
            return drawPositions;
        }
        float startPos;
        if (isEven)
        {
            startPos = posy - nOfNeurons / 2 * nextNeuronIncrement + nextNeuronIncrement/2;
        }
        else
        {
            startPos = posy - nOfNeurons / 2 * nextNeuronIncrement;
        }
        for (int i = 0; i < nOfNeurons; i++)
        {
            drawPositions[i] = startPos + nextNeuronIncrement*i;
        }
        return drawPositions;
    }

    private Vector2 getDir(Vector2 start, Vector2 end)
    {
        Vector3 directionalVector = end - start;
        Vector3 unitary = directionalVector.normalized;
        return unitary;
    }

    private float getLength(Vector2 start, Vector2 end)
    {
        float distance = Vector2.Distance(start, end);
        return distance;
    }

    private (float[], GameObject) getBestGenome()
    {
        GameObject[] individuals = trainManager.individualClones;
        GameObject currBestGenome = null;
        float currBestScore = -1;
        for(int i = 0; i < individuals.Length; i++)
        {
            Brain individualBrain = individuals[i].GetComponent<Brain>();
            if(individualBrain.currentScore > currBestScore)
            {
                currBestScore = individualBrain.currentScore;
                currBestGenome = individuals[i];
            }
        }

        return (currBestGenome.GetComponent<Brain>().weights, currBestGenome);
    }
}

