using System;
using System.Collections.Generic;

[Serializable]
public class Schedule
{
    public string groupCode;
    public DaySchedule[] scheduleFirstWeek;
    public DaySchedule[] scheduleSecondWeek;
}

[Serializable]
public class DaySchedule
{
    public string dayName;
    public Pair[] pairs;

    public void FilterPairs(HashSet<string> allowedSubjects)
    {
        var result = new List<Pair>();

        foreach (var pair in pairs)
            if (allowedSubjects.Contains(pair.name))
                result.Add(pair);

        pairs = result.ToArray();
    }
}

[Serializable]
public class Pair
{
    public string teacherName;
    public string lecturerId;
    public Lecturer lecturer;
    public string type;
    public string time;
    public string name;
    public string place;
    public string location;
    public string tag;
    public string[] dates;
}

[Serializable]
public class Lecturer
{
    public string id;
    public string name;
}