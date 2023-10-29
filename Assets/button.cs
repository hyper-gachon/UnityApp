using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ButtonHandler : MonoBehaviour
{
    public Button NoticeButton;
    public Button AcademicScheduleButton;
    public Button HomeworkButton;
    public Button BusTimetableButton;

    void Start()
    {
        NoticeButton.onClick.AddListener(() => SendQuery("Panel_Notice"));
        AcademicScheduleButton.onClick.AddListener(() => SendQuery("Panel_Academy"));
        HomeworkButton.onClick.AddListener(() => SendQuery("Panel_Homework"));
        BusTimetableButton.onClick.AddListener(() => SendQuery("Panel_Bus"));
    }

    void SendQuery(string panelName)
    {
        string url = "localhost:3000";
        if (panelName.Equals("Panel_Notice"))
            url = url + "/crawling/fullnotice";
        else if (panelName.Equals("Panel_Academy"))
            url = url + "/crawling/academic_schedule";
        else if (panelName.Equals("Panel_Homework"))
            url = url + "/crawling/cybercampus";
        else if (panelName.Equals("Panel_Bus"))
            url = url + "/crawling/bus";

        StartCoroutine(SendRequest(url));
    }

    System.Collections.IEnumerator SendRequest(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Request failed: " + request.error);
            }
        }
    }
}
