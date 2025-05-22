public class User
{
    public string Name;

    public int RightAnswersCount { get; private set; }

    public string Diagnosis;

    public User(string name, int rightAnswersCount = 0)
    {
        Name = name;
        RightAnswersCount = rightAnswersCount;
    }

    public void Deconstruct(out string name, out int rightAnswersCount, out string diagnosis)
    {
        name = Name;
        rightAnswersCount = RightAnswersCount;
        diagnosis = Diagnosis;
    }

    public override string ToString()
    {
        return $"{Name}#{RightAnswersCount}#{Diagnosis}";
    }

    public void AddRightAnswer()
    {
        RightAnswersCount++;
    }

    public void ResetStats()
    {
        RightAnswersCount = 0;
        Diagnosis = string.Empty;
    }
}