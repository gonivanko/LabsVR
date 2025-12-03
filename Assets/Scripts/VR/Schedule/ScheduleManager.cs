using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ScheduleManager : MonoBehaviour
{
    private const string scheduleUrl =
        "https://api.campus.kpi.ua/schedule/lessons?groupId=792c3bf7-0027-4d82-b210-0b5ba9944602";

    private const string currentTimeUrl = "https://api.campus.kpi.ua/time/current";
    [SerializeField] private TMP_Text dayLabel;
    [SerializeField] private Transform scheduleContainer;
    [SerializeField] private GameObject headingRowPrefab;
    [SerializeField] private GameObject lessonRowPrefab;
    [SerializeField] private TMP_Text switchWeekButtonLabel;
    private CurrentTime currentTime;
    private int dayToDisplay = 1;
    private bool isFirstWeek = true;


    private Schedule schedule;


    private void Start()
    {
        Get(currentTimeUrl, error => { Debug.Log(error); },
            text =>
            {
                ParseCurrentTimeJson(text);
                Get(scheduleUrl, error => { Debug.Log(error); }, text =>
                {
                    Debug.Log(text);
                    ParseScheduleJson(text);
                });
            });
    }

    private void Get(string url, Action<string> onError, Action<string> onSuccess)
    {
        StartCoroutine(GetCoroutine(url, onError, onSuccess));
    }

    private IEnumerator GetCoroutine(string url, Action<string> onError, Action<string> onSuccess)
    {
        using (var webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
                onError(webRequest.error);
            else
                onSuccess(webRequest.downloadHandler.text);
        }
    }

    public void incrementDay()
    {
        if (dayToDisplay == 6)
        {
            dayToDisplay = 1;
            isFirstWeek = !isFirstWeek;
        }
        else
        {
            dayToDisplay += 1;
        }

        displayDaySchedule();
    }

    public void decrementDay()
    {
        if (dayToDisplay == 1)
        {
            dayToDisplay = 6;
            isFirstWeek = !isFirstWeek;
        }
        else
        {
            dayToDisplay -= 1;
        }

        displayDaySchedule();
    }

    public void toggleWeek()
    {
        isFirstWeek = !isFirstWeek;
        displayDaySchedule();
    }

    private void ParseScheduleJson(string jsonString)
    {
        schedule = JsonUtility.FromJson<Schedule>(jsonString);

        isFirstWeek = currentTime.currentWeek == 1;

        dayToDisplay = currentTime.currentDay;

        displayDaySchedule();
    }

    private void setDayLabel()
    {
        switch (dayToDisplay)
        {
            case 1:
                dayLabel.text = "Понеділок";
                break;
            case 2:
                dayLabel.text = "Вівторок";
                break;
            case 3:
                dayLabel.text = "Середа";
                break;
            case 4:
                dayLabel.text = "Четвер";
                break;
            case 5:
                dayLabel.text = "П'ятниця";
                break;
            case 6:
                dayLabel.text = "Субота";
                break;
            case 7:
                dayLabel.text = "Неділя";
                break;
            default:
                dayLabel.text = "Невідомий день";
                break;
        }
    }

    private void setWeekLabel()
    {
        if (isFirstWeek)
            switchWeekButtonLabel.text = "Тиждень: 1";
        else
            switchWeekButtonLabel.text = "Тиждень: 2";
    }

    private DaySchedule[] getWeekSchedule()
    {
        if (isFirstWeek) return schedule.scheduleFirstWeek;

        return schedule.scheduleSecondWeek;
    }

    private DaySchedule getDaySchedule(int day)
    {
        var weekSchedule = getWeekSchedule();
        return weekSchedule[day - 1];
    }

    private void removeRows()
    {
        foreach (Transform child in scheduleContainer) Destroy(child.gameObject);
    }

    private void addNewRows(DaySchedule daySchedule)
    {
        var headingRow = Instantiate(headingRowPrefab, scheduleContainer);

        foreach (var pair in daySchedule.pairs)
        {
            var row = Instantiate(lessonRowPrefab, scheduleContainer);

            var texts = row.GetComponentsInChildren<TMP_Text>();

            texts[1].text = pair.time;
            texts[2].text = pair.name;
            texts[3].text = pair.type;
            texts[4].text = pair.teacherName;
        }
    }

    private void displayDaySchedule()
    {
        setDayLabel();
        setWeekLabel();

        var currentDaySchedule = getDaySchedule(dayToDisplay);

        removeRows();

        addNewRows(currentDaySchedule);
    }

    private void ParseCurrentTimeJson(string jsonString)
    {
        currentTime = JsonUtility.FromJson<CurrentTime>(jsonString);
    }
}