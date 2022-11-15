using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMove : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotateSpeed;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        this.transform.position += transform.forward * Time.deltaTime * movementSpeed;

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(0, Time.deltaTime * (-rotateSpeed), 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);
        }
    }

    public void stopGame()
    {
        movementSpeed = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody.transform.tag == "obstacle")
        {
            stopGame();
        }
    }
}
