using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckpointScore : MonoBehaviour
{
    public float firstIndividualTime;

    public float c = 5000;

    public List<GameObject> individualsPassed;

    [SerializeField]
    private TrainManager trainManager;

    private AppManager appManager;

    private bool isFirstTime = true;


    private void Start()
    {
        individualsPassed = new List<GameObject>();
        
        appManager = GameObject.Find("AppManager").GetComponent<AppManager>();
    }

    private void Update()
    {
        if (appManager.isReady)
        {
            if (isFirstTime)
            {
                trainManager = GameObject.Find("TrainManager").GetComponent<TrainManager>();
                isFirstTime = false;
            }
            if (trainManager.allDone)
            {
                resetValues();
            }
        }
    }


    public float getPoints(float individualTime, GameObject individual)
    {
        if (individualsPassed.Contains(individual) || individualsPassed.Count == 0)
        {
            individualsPassed.Clear();
            firstIndividualTime = individualTime;
        }
        individualsPassed.Add(individual);

        return calculatePointsFunction(individualTime);
    }

    public float calculatePointsFunction(float x)
    {
        return (-1000 * (x - firstIndividualTime)) + c;
    }

    private void resetValues()
    {
        individualsPassed.Clear();
        firstIndividualTime = 0;
    }
}
