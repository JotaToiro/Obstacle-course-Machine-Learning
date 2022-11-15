using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float speed;

    private Vector3 oldMousePosition;

    void Update()
    {
        
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * speed);
        }
        

        if (Input.GetMouseButtonDown(2))
        {
            oldMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 moveVector = Camera.main.ScreenToViewportPoint(Input.mousePosition) - oldMousePosition;
            moveVector = new Vector3(-moveVector.y, moveVector.z, moveVector.x);
            Camera.main.transform.position += moveVector*15;
            oldMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        this.transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + (-Input.mouseScrollDelta.y), transform.position.z), 10f);
    }
}
