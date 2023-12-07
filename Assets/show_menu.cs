using System.Collections;
using System.Collections.Generic;
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
        string url = "http://52.79.77.43:8080/api";
        if (panelName.Equals("Panel_Notice"))
            url = url + "/crawling/fullnotice";
        else if (panelName.Equals("Panel_Academy"))
            url = url + "/crawling/academic_schedule";
        else if (panelName.Equals("Panel_Homework"))
            url = url + "/crawling/cybercampus";
        else if (panelName.Equals("Panel_Bus"))
            url = url + "/crawling/bus";

        using (UnityWebRequest request = UnityWebRequest.Get(url)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success) {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log(jsonResponse);

                if (responseTextMeshPro != null) {
                    responseTextMeshPro.text = ParseJson(jsonResponse, panelName); // JSON 파싱 후 텍스트 업데이트
                }
                else {
                    Debug.LogError("TextMeshProUGUI component is not set.");
                }
            }
            else {
                Debug.LogError("Request failed: " + request.error);
            }
        }
    }

    private string ParseJson(string json, string panelName) {
        if (panelName.Equals("Panel_Notice")) {
            var items = JsonUtility.FromJson<NoticeList>("{\"items\":" + json + "}").items;
        string displayText = "";

        for (int i = 0; i < 3 && i < items.Length; i++) {
            displayText += $"제목: {items[i].noticeTitle}\n작성자: {items[i].writer}\n작성일: {items[i].date}\n\n";
        }

        return displayText;
        } else if (panelName.Equals("Panel_Academy")) {
            var schedule = JsonUtility.FromJson<AcademicScheduleList>(json);
            string displayText = $"2023년 12월: {schedule.month}\n";

            foreach (var item in schedule.academicSchedule) {
                displayText += $"날짜: {item.acaScheduleDate}\t일정 내용: {item.acaSchedule}\n";
            }

            return displayText;
        } else if (panelName.Equals("Panel_Homework")) {
            return "과제 목록";
        } else if (panelName.Equals("Panel_Bus")) {
            return "무당이 시간표";
        } else {
            return "";
        }
    }

    [System.Serializable]
    public class NoticeItem {
        public string noticeNum;
        public string noticeTitle;
        public string writer;
        public string date;
    }

    [System.Serializable]
    public class NoticeList {
        public NoticeItem[] items;
    }

    [System.Serializable]
    public class AcademicScheduleItem {
        public string acaScheduleDate;
        public string acaSchedule;
    }

    [System.Serializable]
    public class AcademicScheduleList {
        public string month;
        public AcademicScheduleItem[] academicSchedule;
    }
}

