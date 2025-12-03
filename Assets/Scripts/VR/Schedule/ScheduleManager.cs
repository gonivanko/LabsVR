using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ScheduleManager : MonoBehaviour
{
    private const string baseUrl = "https://api.campus.kpi.ua/";
    private const string groupId = "792c3bf7-0027-4d82-b210-0b5ba9944602";

    private const string currentTimeUrl = baseUrl + "time/current/";
    private const string scheduleUrl = baseUrl + "schedule/lessons?groupId=" + groupId;

    
    [SerializeField] private TMP_Text dayLabel;
    [SerializeField] private Transform scheduleContainer;
    [SerializeField] private GameObject headingRowPrefab;
    [SerializeField] private GameObject lessonRowPrefab;
    [SerializeField] private TMP_Text switchWeekButtonLabel;


    private int dayToDisplay = 1;
    private bool isFirstWeek = true;

    private CurrentTime currentTime;
    private Schedule schedule;

    private HashSet<string> subjects = new HashSet<string>
    {
        "Розроблення VR/AR застосунків",
        "Моделювання систем",
        "Безпека програмного забезпечення",
        "Економіка ІТ-індустрії та підприємництво",
        "Права і свободи людини",
        "Практичний курс іноземної мови професійного спрямування. Частина 2",
        "Розробка програмного забезпечення. Проєктний та програмний менеджмент",
        "Основи комп’ютерного моделювання"
    };


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

    public void IncrementDay()
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

        DisplayDaySchedule();
    }

    public void DecrementDay()
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

        DisplayDaySchedule();
    }

    public void ToggleWeek()
    {
        isFirstWeek = !isFirstWeek;
        DisplayDaySchedule();
    }

    private void ParseScheduleJson(string jsonString)
    {
        schedule = JsonUtility.FromJson<Schedule>(jsonString);

        var subjects = Schedule.GetAllUniqueSubjects(schedule);

        foreach (var subject in subjects)
        {
            Debug.Log(subject);
        }

        isFirstWeek = currentTime.currentWeek == 1;

        dayToDisplay = currentTime.currentDay;

        DisplayDaySchedule();
    }

    private void SetDayLabel()
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
    
    private int GetPairNumber(TimeSpan time)
    {
        if (time < new TimeSpan(8, 30, 0))
            return 0;
        else if (time < new TimeSpan(10, 25, 0))
            return 1;
        else if (time < new TimeSpan(12, 20, 0))
            return 2;
        else if (time < new TimeSpan(14, 15, 0))
            return 3;
        else if (time < new TimeSpan(16, 10, 0))
            return 4;
        else if (time < new TimeSpan(18, 05, 0))
            return 5;
        else if (time < new TimeSpan(20, 0, 0))
            return 6;
        else
            return 7;
    }

    private void SetWeekLabel()
    {
        if (isFirstWeek)
            switchWeekButtonLabel.text = "Тиждень: 1";
        else
            switchWeekButtonLabel.text = "Тиждень: 2";
    }

    private DaySchedule[] GetWeekSchedule()
    {
        if (isFirstWeek) return schedule.scheduleFirstWeek;

        return schedule.scheduleSecondWeek;
    }

    private DaySchedule GetDaySchedule(int day)
    {
        var weekSchedule = GetWeekSchedule();
        return weekSchedule[day - 1];
    }

    private void RemoveRows()
    {
        foreach (Transform child in scheduleContainer) Destroy(child.gameObject);
    }

    private void AddNewRows(DaySchedule daySchedule)
    {
        var headingRow = Instantiate(headingRowPrefab, scheduleContainer);

        foreach (var pair in daySchedule.pairs)
        {
            var row = Instantiate(lessonRowPrefab, scheduleContainer);

            var texts = row.GetComponentsInChildren<TMP_Text>();

            var pairTime = DateTime.Parse(pair.time).TimeOfDay;

            texts[0].text = GetPairNumber(pairTime).ToString();
            texts[1].text = pairTime.ToString("hh\\:mm");
            texts[2].text = pair.name;
            texts[3].text = pair.type;
            texts[4].text = pair.teacherName;
        }
    }

    private void DisplayDaySchedule()
    {
        SetDayLabel();
        SetWeekLabel();

        var currentDaySchedule = GetDaySchedule(dayToDisplay);

        currentDaySchedule.FilterPairs(subjects);

        RemoveRows();

        AddNewRows(currentDaySchedule);
    }

    private void ParseCurrentTimeJson(string jsonString)
    {
        currentTime = JsonUtility.FromJson<CurrentTime>(jsonString);
    }
}