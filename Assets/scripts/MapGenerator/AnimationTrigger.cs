using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public bool isDown;

    [SerializeField]
    public Material upMaterial;
    [SerializeField]
    public Material downMaterial;
    [SerializeField]
    public Material mouseOverMaterial;

    private void Start()
    {
        GetComponent<Animator>().Play("ShapeUp");
    }

    public void playAnimationUp()
    {
        GetComponent<MeshRenderer>().material = upMaterial;
        GetComponent<Animator>().Play("ShapeUp");
        isDown = false;
    }

    public void playAnimationDown()
    {
        GetComponent<MeshRenderer>().material = downMaterial;
        GetComponent<Animator>().Play("ShapeDown");
        isDown = true;
    }

    public void setSizeUp()
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x, 4, this.transform.localScale.z);
    }

    public void setSizeDown()
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x, 1, this.transform.localScale.z);
    }
}
