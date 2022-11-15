using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AppManager : MonoBehaviour
{
    private int stage = 0;

    [SerializeField]
    private GameObject trainManager;
    [SerializeField]
    private GenerateBaseMap mapGenerator;
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject neuralNetworkUI;

    [SerializeField]
    private Button readyButton;

    public bool isReady = false;
    
    public void nextConfigButton()
    {
        switch (stage)
        {
            case 0:
                mapGenerator.GenerateMap();
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Next config";
                stage += 1;
                break;
            case 1:
                mapGenerator.isEdit = false;
                readyButton.interactable = true;
                stage += 1;
                mapGenerator.GetComponent<PlaceSpawnPoint>().enabled = true;
                break;
        }
    }

    public void ReadyButton()
    {
        trainManager.SetActive(true);
        button.gameObject.SetActive(false);
        neuralNetworkUI.SetActive(true);
        mapGenerator.isEdit = true;
        isReady = true;
    }
}
