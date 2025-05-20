Console.Write("Добро пожаловать в интеллектуальный тест!\nВведите ваше имя: ");
string userName = Console.ReadLine().Trim();

bool isTesting = true;

while (isTesting)
{
    var questionsAndAnswers = new List<(string question, int rightAnswer)>()
    {
        ("Сколько будет: 2 + 2 * 2?", 6),
        ("Бревно нужно распилить на 10 частей, сколько нужно сделать распилов?", 9),
        ("На двух руках 10 пальцев. Сколько пальцев на 5 руках?", 25),
        ("Укол делают каждые полчаса, сколько минут для трех уколов?", 60),
        ("Пять свечей горело, две потухли. Сколько свечей осталось?", 2),
        ("Сколько месяцев в году имеют 28 дней?", 12),
        ("Сколько яиц можно съесть натощак? ", 1)
    };

    int questionsCount = questionsAndAnswers.Count;
    int rightAnswersCount = 0;
    int questionNumber = 1;

    Random random = new Random();

    Console.Clear();

    while (questionsAndAnswers.Count > 0)
    {
        int randomIndex = random.Next(0, questionsAndAnswers.Count);
        var (question, rightAnswer) = questionsAndAnswers[randomIndex];

        Console.WriteLine($"Вопрос №{questionNumber}:\n{question}\n");

        bool isNumber = int.TryParse(Console.ReadLine().Trim(), out int userAnswer);

        if (isNumber && userAnswer == rightAnswer)
        {
            rightAnswersCount++;
        }

        questionNumber++;
        questionsAndAnswers.RemoveAt(randomIndex);
    }

    string diagnose = GetDiagnose(rightAnswersCount, questionsCount);

    Console.WriteLine($"\nВы ответили правильно на {rightAnswersCount} из {questionsCount} вопросов.");
    Console.WriteLine($"Поздравляю, {userName}. Вы - {diagnose}!");
    Console.WriteLine("\nХотите пройти тест еще раз? (да/нет)\n");

    isTesting = Console.ReadLine().ToLower() == "да" ? true : false;
}

static string GetDiagnose(int rightAnswersCount, int questionsCount)
{
    double koeff = (double)rightAnswersCount / questionsCount;
    int indexOfDiagnose = (int)Math.Floor(koeff * 5);

    var diagnoses = new Dictionary<int, string>
        {
            { 0, "Идиот" },
            { 1, "Кретин" },
            { 2, "Дурак" },
            { 3, "Нормальный" },
            { 4, "Талант" },
            { 5, "Гений" }
        };

    return diagnoses[indexOfDiagnose];
}