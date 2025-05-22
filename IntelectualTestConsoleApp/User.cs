public class User
{
    public string Name;

    public int RightAnswersCount { get; private set; }

    public string Diagnosis;

    public User(string name)
    {
        Name = name;
        RightAnswersCount = 0;
    }

    public void Deconstruct(out string name, out int rightAnswersCount, out string diagnosis)
    {
        name = Name;
        rightAnswersCount = RightAnswersCount;
        diagnosis = Diagnosis;
    }

    public void AddRightAnswer()
    {
        RightAnswersCount++;
    }
}