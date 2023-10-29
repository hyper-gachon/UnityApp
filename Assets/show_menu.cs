using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro; 

public class ToggleTextPanel : MonoBehaviour
{
    public GameObject textPanel;
    public TextMeshProUGUI responseTextMeshPro;  // TextMeshProUGUI 컴포넌트에 대한 참조

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TogglePanel);
    }

    private void TogglePanel()
    {
        textPanel.SetActive(!textPanel.activeSelf);
        if (textPanel.activeSelf)
        {
            StartCoroutine(SendRequest(textPanel.name));

        }
    }

    private IEnumerator SendRequest(string panelName)
    {
        string url = "localhost:8080";
        if (panelName.Equals("Panel_Notice"))
            url = url + "/crawling/fullnotice";
        else if (panelName.Equals("Panel_Academy"))
            url = url + "/crawling/academic_schedule";
        else if (panelName.Equals("Panel_Homework"))
            url = url + "/crawling/cybercampus";
        else if (panelName.Equals("Panel_Bus"))
            url = url + "/crawling/bus";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
                if (responseTextMeshPro != null)
                {
                    responseTextMeshPro.text = request.downloadHandler.text;  // 텍스트 업데이트
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI component is not set.");
                }
            }
            else
            {
                Debug.LogError("Request failed: " + request.error);
            }
        }
    }
}
