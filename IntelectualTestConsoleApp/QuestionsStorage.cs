public class QuestionsStorage
{
    private List<Question> _questions;
    private Random _random = new Random();

    public int Count => _questions.Count;

    public QuestionsStorage()
    {
        _questions = FileSystem.Load<Question>(FileNames.Questions);
    }

    public Question GetRandomQuestion()
    {
        int randomIndex = _random.Next(0, _questions.Count);
        var question = _questions[randomIndex];

        _questions.RemoveAt(randomIndex);

        return question;
    }

    public void PrintQuestions()
    {
        for (int i = 1; i <= Count; i++)
        {
            var question = _questions[i - 1];

            Console.WriteLine($"{i}. {question.Text}");
        }
    }
}