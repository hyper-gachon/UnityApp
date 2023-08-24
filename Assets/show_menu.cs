using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToggleTextPanel : MonoBehaviour
{
    public GameObject textPanel;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TogglePanel);
    }

    private void TogglePanel()
    {
        textPanel.SetActive(!textPanel.activeSelf);
    }
}