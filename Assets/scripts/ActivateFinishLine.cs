using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFinishLine : MonoBehaviour
{
    public float counter;

    private void Awake()
    {
        //counter = 5f;
        //GetComponent<Collider>().enabled = false;
    }

    private void Start()
    {
        counter = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(counter >= 0)
        {
            counter -= Time.deltaTime;

        }

    }
}
