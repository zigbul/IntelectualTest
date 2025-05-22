public class QuestionsStorage
{
    private List<Question> _questions;
    private Random _random = new Random();

    public int Count => _questions.Count;

    public QuestionsStorage()
    {
        _questions = new List<Question>()
        {
            new("Сколько будет: 2 + 2 * 2?", 6),
            new("Бревно нужно распилить на 10 частей, сколько нужно сделать распилов?", 9),
            new("На двух руках 10 пальцев. Сколько пальцев на 5 руках?", 25),
            new("Укол делают каждые полчаса, сколько минут для трех уколов?", 60),
            new("Пять свечей горело, две потухли. Сколько свечей осталось?", 2),
            new("Сколько месяцев в году имеют 28 дней?", 12),
            new("Сколько яиц можно съесть натощак? ", 1)
        };
    }

    public Question GetRandomQuestion()
    {
        int randomIndex = _random.Next(0, _questions.Count);
        var question = _questions[randomIndex];

        _questions.RemoveAt(randomIndex);

        return question;
    }
}