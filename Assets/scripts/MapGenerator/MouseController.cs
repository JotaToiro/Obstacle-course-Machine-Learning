using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private AnimationTrigger oldOutline;
    void Update()
    {
        if (GetComponent<GenerateBaseMap>().isEdit)
        {
            if (Input.GetMouseButton(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "Shape")
                    {
                        AnimationTrigger animationTrigger = hit.transform.GetComponent<AnimationTrigger>();
                        animationTrigger.playAnimationUp();
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "Shape")
                    {
                        AnimationTrigger animationTrigger = hit.transform.GetComponent<AnimationTrigger>();
                        animationTrigger.playAnimationDown();
                    }
                }
            }

            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit2;
            if (Physics.Raycast(ray2, out hit2))
            {
                if (hit2.transform.tag == "Shape")
                {
                    AnimationTrigger outline = hit2.transform.GetComponent<AnimationTrigger>();
                    if (oldOutline != null && outline != oldOutline)
                    {
                        if (oldOutline.isDown)
                        {
                            oldOutline.GetComponent<Renderer>().material = oldOutline.downMaterial;
                        }
                        else
                        {
                            oldOutline.GetComponent<Renderer>().material = oldOutline.upMaterial;
                        }
                    }

                    if (outline.GetComponent<Renderer>().material != outline.mouseOverMaterial)
                    {
                        outline.GetComponent<Renderer>().material = outline.mouseOverMaterial;
                        oldOutline = outline;
                    }
                }
            }
            else
            {
                if (oldOutline != null)
                {
                    if (oldOutline.isDown)
                    {
                        oldOutline.GetComponent<Renderer>().material = oldOutline.downMaterial;
                    }
                    else
                    {
                        oldOutline.GetComponent<Renderer>().material = oldOutline.upMaterial;
                    }
                }
            }
        }
    }
}
