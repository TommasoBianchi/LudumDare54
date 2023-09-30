using System.Collections.Generic;

public class History
{
    List<List<string>> choiceIDsByDay = new List<List<string>>();
    List<HashSet<string>> choiceIDSetByDay = new List<HashSet<string>>();

    public void StartDay()
    {
        choiceIDsByDay.Add(new List<string>());
        choiceIDSetByDay.Add(new HashSet<string>());
    }

    public void AddChoice(Choice choice)
    {
        choiceIDsByDay[choiceIDsByDay.Count - 1].Add(choice.ID);
        choiceIDSetByDay[choiceIDsByDay.Count - 1].Add(choice.ID);
    }

    public int DaysSinceChoiceTaken(Choice choice)
    {
        return DaysSinceChoiceTaken(choice.ID);
    }

    public int DaysSinceChoiceTaken(string choiceID)
    {
        for (int i = choiceIDSetByDay.Count - 1; i >= 0; --i)
        {
            if (choiceIDSetByDay[i].Contains(choiceID))
            {
                return i;
            }
        }

        return int.MaxValue;
    }
}