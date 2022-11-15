using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapInitializer : MonoBehaviour
{
    [SerializeField]
    private bool isPreset = false;

    [SerializeField]
    private Vector3 mapPos;
    [SerializeField]
    private Vector3 finishPos;
    [SerializeField]
    private Vector3 finishRotation;

    public Vector3[] checkpointsPos;
    public Vector3[] checkpointsRotations;

    [SerializeField]
    private GameObject checkpint;

    [SerializeField]
    private Button readyButton;

    public GameObject finishLine;

    private GenerateBaseMap mapGenerator;

    // Start is called before the first frame update
    void Start()
    {
        mapGenerator = GameObject.Find("MapGenerator").GetComponent<GenerateBaseMap>();
        readyButton = GameObject.Find("ReadyButton").GetComponent<Button>();
        if (isPreset)
        {
            transform.position = mapPos;
            finishLine = GameObject.Find("FinishLine");
            finishLine.transform.position = finishPos;
            finishLine.transform.eulerAngles = finishRotation;

            finishLine.transform.GetChild(1).gameObject.SetActive(true);

            for(int i = 0; i < checkpointsPos.Length; i++)
            {
                GameObject checkpointClone = Instantiate(checkpint, checkpointsPos[i], Quaternion.identity);
                checkpointClone.transform.eulerAngles = checkpointsRotations[i];
            }

            readyButton.interactable = true;

            for(int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<AnimationTrigger>().isDown)
                {
                    transform.GetChild(i).GetComponent<AnimationTrigger>().playAnimationDown();
                }
                else
                {
                    transform.GetChild(i).GetComponent<AnimationTrigger>().playAnimationUp();
                }
            }
        }
    }

    public Vector3 MapPos
    {
        set { mapPos = value; }
        get { return mapPos; }
    }

    public Vector3 FinishPos
    {
        set { finishPos = value; }
        get { return finishPos; }
    }

    public Vector3 FinishRotation
    {
        set { finishRotation = value; }
        get { return finishRotation; }
    }

    public bool IsPreset
    {
        get { return isPreset; }
        set { isPreset = value; }
    }
}
