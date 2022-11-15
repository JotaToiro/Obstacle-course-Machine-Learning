using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRayCast : MonoBehaviour
{
    public float distance = 0;
    public LayerMask layerMask;
    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        //line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit objectHit;
        if (Physics.Raycast(transform.position, transform.forward, out objectHit, 50, layerMask))
        {
            distance = objectHit.distance;
            //distance2.text = inputLayer[1].ToString();
        }

        //line.SetPosition(0, this.transform.position);
        //line.SetPosition(1, objectHit.point);
        //Vector3 forward = transform.TransformDirection(Vector3.forward) * 50;
        //Debug.DrawRay(transform.position, forward, Color.green);

    }

}
