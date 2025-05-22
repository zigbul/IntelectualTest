public class Question
{
    public string Text { get; }
    public int RightAnswer { get; }

    public Question(string text, int rightAnswer)
    {
        Text = text;
        RightAnswer = rightAnswer;
    }

    public void Deconstruct(out string text, out int rightAnswer)
    {
        text = Text;
        rightAnswer = RightAnswer;
    }
}
