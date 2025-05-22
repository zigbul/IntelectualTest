Console.WriteLine("Добро пожаловать в интеллектуальный тест!");
Console.Write("Введите ваше имя: ");

var user = new User(Console.ReadLine().Trim());

bool isTesting = true;

while (isTesting)
{
    var questions = new List<Question>()
    {
        new Question("Сколько будет: 2 + 2 * 2?", 6),
        new Question("Бревно нужно распилить на 10 частей, сколько нужно сделать распилов?", 9),
        new Question("На двух руках 10 пальцев. Сколько пальцев на 5 руках?", 25),
        new Question("Укол делают каждые полчаса, сколько минут для трех уколов?", 60),
        new Question("Пять свечей горело, две потухли. Сколько свечей осталось?", 2),
        new Question("Сколько месяцев в году имеют 28 дней?", 12),
        new Question("Сколько яиц можно съесть натощак? ", 1)
    };

    int questionsCount = questions.Count;
    int questionNumber = 1;

    Random random = new Random();

    while (questions.Count > 0)
    {
        Console.Clear();

        int randomIndex = random.Next(0, questions.Count);
        var (question, rightAnswer) = questions[randomIndex];

        Console.WriteLine($"\nВопрос №{questionNumber}:\n{question}\n");

        bool isNumber = int.TryParse(Console.ReadLine().Trim(), out int userAnswer);

        Console.WriteLine();

        if (isNumber && userAnswer == rightAnswer)
        {
            user.AddRightAnswer();
        }

        questionNumber++;
        questions.RemoveAt(randomIndex);
    }

    user.Diagnosis = GetDiagnose(user.RightAnswersCount, questionsCount);

    var (name, rightAnswersCount, diagnosis) = user;

    Console.WriteLine($"Вы ответили правильно на {rightAnswersCount} из {questionsCount} вопросов.");
    Console.WriteLine($"Поздравляю, {name}. Вы - {diagnosis}!");

    WriteResultsToFile(name, rightAnswersCount, diagnosis);

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

static string GetResultsFilePath()
{
    string currentDirectoryPath = Directory.GetCurrentDirectory();
    string projectDirectoryPath = Path.Combine(currentDirectoryPath, @"..\..\..\");
    string resultsFileName = "results.txt";
    string resultsFileNamePath = Path.Combine(projectDirectoryPath, resultsFileName);

    return resultsFileNamePath;
}

static void PrintResults()
{
    string pathToResults = GetResultsFilePath();

    const int FirstColumnWidth = 16;
    const int SecondColumnWidth = 25;
    const int ThirdColumnWidth = 7;

    string header = $"|| { "Имя пользователя", -FirstColumnWidth} || { "Кол-во правильных ответов", -SecondColumnWidth} || { "Диагноз", ThirdColumnWidth} ||";

    Console.WriteLine($"{header}");

    if (File.Exists(pathToResults) == false)
    {
        using FileStream fs = File.Create(pathToResults);
    }

    bool areResultsNotEmpty = new FileInfo(pathToResults).Length != 0;

    if (areResultsNotEmpty)
    {
        using (StreamReader sr = new StreamReader(pathToResults))
        {
            string resultLine = sr.ReadLine();

            while (string.IsNullOrEmpty(resultLine) == false)
            {   
                string[] resultInfo = resultLine.Split(' ');

                string userName = resultInfo[0];
                int rightAnswersCount = int.Parse(resultInfo[1]);
                string diagnose = resultInfo[2];

                Console.WriteLine($"|| {userName, FirstColumnWidth} || {rightAnswersCount, SecondColumnWidth} || {diagnose, ThirdColumnWidth} ||");
                resultLine = sr.ReadLine();
            }
        }
    }
    else
    {
        string emptyMessage = "Пока результатов нет";
        int totalWidth = header.Length - 6;
        int padding = (totalWidth - emptyMessage.Length) / 2;
        string centeredMessage = emptyMessage.PadLeft(emptyMessage.Length + padding).PadRight(totalWidth);

        Console.WriteLine($"|| {centeredMessage} ||");
    }
}

static void WriteResultsToFile(string userName, int rightAnswersCount, string diagnose)
{
    string resultFilePath = GetResultsFilePath();

    using (StreamWriter writer = new StreamWriter(resultFilePath, true, System.Text.Encoding.Default))
    {
        writer.WriteLine($"{userName} {rightAnswersCount} {diagnose}");
    }
}
