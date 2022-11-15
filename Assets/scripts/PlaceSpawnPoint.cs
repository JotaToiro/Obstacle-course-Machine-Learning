using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceSpawnPoint : MonoBehaviour
{
    Vector3 mouseClickPosition;

    [SerializeField]
    private GameObject arrow;
    [SerializeField]
    private GameObject finish;
    [SerializeField]
    private GameObject finishLine;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Vector3 offset;

    public MapInitializer map;

    public bool finishPlaced = false;

    [SerializeField]
    private GameObject checkpoint;

    private GameObject checkPointClone;

    void Start()
    {
        checkPointClone = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<GenerateBaseMap>().isEdit)
        {
            if (!finishPlaced)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, layerMask))
                    {
                        mouseClickPosition = hit.point + offset;
                        arrow.transform.position = mouseClickPosition;

                        arrow.SetActive(true);
                        finish.SetActive(true);
                        finishLine.SetActive(true);
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    Vector3 currMousePos;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, layerMask))
                    {
                        currMousePos = hit.point + offset;
                        Vector3 dir = (currMousePos - mouseClickPosition).normalized;
                        Vector3 pos = mouseClickPosition + dir * 2.5f;


                        arrow.transform.position = pos;
                        arrow.transform.right = dir;
                        arrow.transform.eulerAngles = new Vector3(90, arrow.transform.eulerAngles.y, 0);


                        finishLine.transform.position = mouseClickPosition;
                        finishLine.transform.forward = dir;
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    arrow.SetActive(false);
                    map.MapPos = map.gameObject.transform.position;
                    map.FinishPos = finishLine.transform.position;
                    map.FinishRotation = finishLine.transform.eulerAngles;
                    map.IsPreset = true;
                    finishPlaced = true;
                }
            }
            else//----------------------------------------------------------------------------------------------
            {
                
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, layerMask))
                    {
                        mouseClickPosition = hit.point + offset;
                        arrow.transform.position = mouseClickPosition;

                        arrow.SetActive(true);

                        checkPointClone = Instantiate(checkpoint, mouseClickPosition, Quaternion.identity);
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    Vector3 currMousePos;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, layerMask))
                    {
                        currMousePos = hit.point + offset;
                        Vector3 dir = (currMousePos - mouseClickPosition).normalized;
                        Vector3 pos = mouseClickPosition + dir * 2.5f;


                        arrow.transform.position = pos;
                        arrow.transform.right = dir;
                        arrow.transform.eulerAngles = new Vector3(90, arrow.transform.eulerAngles.y, 0);


                        checkPointClone.transform.position = mouseClickPosition;
                        checkPointClone.transform.forward = dir;
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    arrow.SetActive(false);
                    map.MapPos = map.gameObject.transform.position;
                    map.FinishPos = finishLine.transform.position;
                    map.FinishRotation = finishLine.transform.eulerAngles;
                    map.IsPreset = true;
                }
            }
            
        }
    }
}
