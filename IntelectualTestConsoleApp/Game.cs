public class Game
{
    private bool _isPlaying = true;
    private bool _isTesting = false;

    public void Play()
    {
        Console.WriteLine("Добро пожаловать в интеллектуальный тест!\n");
        var user = CreateUser();

        while (_isPlaying)
        {
            Console.Clear();
            Console.WriteLine($"{user.Name}, выберите вариант из меню: ");

            Console.WriteLine("\n1. Пройти тест\n2. Посмотреть результаты\n3. Выйти из игры\n");

            int userInput;

            while (TryGetUserInput(out userInput) == false) {}

            switch(userInput)
            {
                case 1:
                    TestUser(user);
                    break;
                case 2:
                    PrintResults();
                    break;
                case 3:
                    ExitGame();
                    break;
                default:
                    PrintInvalidInputMessage();
                    break;
            }
        }
    }

    private User CreateUser()
    {
        Console.Write("Введите ваше имя: ");

        int beginLine = Console.CursorTop - 1;

        string name = Console.ReadLine().Trim();

        while (string.IsNullOrEmpty(name))
        {
            ClearLines(beginLine);

            Console.Write("Имя не может быть пустым." +
                          "\nПожалуйста, введите ваше имя: ");

            name = Console.ReadLine().Trim();
        }

        return new User(name);
    }

    private bool TryGetUserInput(out int userInput)
    {
        int beginLine = Console.CursorTop - 1;

        try
        {
            userInput = int.Parse(Console.ReadLine());
            return true;
        }
        catch(FormatException)
        {
            Console.WriteLine("Нужно ввести число!");
        }
        catch(OverflowException)
        {
            Console.WriteLine("Число слишком большое!");
        }

        Console.ReadKey();
        ClearLines(beginLine);

        userInput = 0;
        return false;
    }

    private void ClearLines(int beginLine)
    {
        int currentLine = Console.CursorTop;
        int linesCount = currentLine - beginLine;

        for (int i = 0; i < linesCount; i++)
        {
            int cursorPositionY = currentLine - i;

            Console.SetCursorPosition(0, cursorPositionY);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, cursorPositionY);
        }
    }

    private void TestUser(User user)
    {
        _isTesting = true;

        while (_isTesting)
        {
            Console.Clear();

            Console.WriteLine($"{user.Name}, Вам нужно ответить на несколько вопросов. Начнём!\n");

            var questions = new QuestionsStorage();
            int questionsCount = questions.Count;

            int questionNumber = 1;

            while (questions.Count > 0)
            {
                var (question, rightAnswer) = questions.GetRandomQuestion();

                Console.WriteLine($"Вопрос №{questionNumber}: {question}\n");

                int userAnswer;

                while (TryGetUserInput(out userAnswer) == false) {}

                Console.WriteLine();

                if (userAnswer == rightAnswer)
                {
                    user.AddRightAnswer();
                }

                questionNumber++;
            }

            user.Diagnosis = CalculateDiagnosis(user.RightAnswersCount, questionsCount);

            Loading("Тест окончен! Подсчитываем результаты");

            var (name, rightAnswersCount, diagnosis) = user;

            Console.WriteLine($"\nВы ответили правильно на {rightAnswersCount} из {questionsCount} вопросов.");
            Console.WriteLine($"Поздравляю, {name}. Вы - {diagnosis}!");

            WriteResultsToFile(name, rightAnswersCount, diagnosis);

            Console.WriteLine("\nХотите пройти тест еще раз? (да/нет)\n");

            _isTesting = Console.ReadLine().ToLower() == "да" ? true : false;
        }
    }

    private void Loading(string loadingText)
    {
        int totalDots = (int)(1 / 0.3);

        for (int i = 1; i <= totalDots; i++)
        {
            Console.Write($"\r{loadingText}{new string('.', i)}");
            Thread.Sleep(300);
        }

        Console.WriteLine();
    }

    private string CalculateDiagnosis(int rightAnswersCount, int questionsCount)
    {
        var diagnoses = new Dictionary<int, string>
        {
            { 0, "Идиот" },
            { 1, "Кретин" },
            { 2, "Дурак" },
            { 3, "Нормальный" },
            { 4, "Талант" },
            { 5, "Гений" }
        };

        double koeff = (double)rightAnswersCount / questionsCount;
        int indexOfDiagnose = (int)Math.Round(koeff * (diagnoses.Count - 1));

        return diagnoses[indexOfDiagnose];
    }

    private void WriteResultsToFile(string userName, int rightAnswersCount, string diagnose)
    {
        string resultFilePath = GetResultsFilePath();

        using (StreamWriter writer = new StreamWriter(resultFilePath, true, System.Text.Encoding.Default))
        {
            writer.WriteLine($"{userName} {rightAnswersCount} {diagnose}");
        }
    }

    private string GetResultsFilePath()
    {
        string currentDirectoryPath = Directory.GetCurrentDirectory();
        string projectDirectoryPath = Path.Combine(currentDirectoryPath, @"..\..\..\");
        string resultsFileName = "results.txt";
        string resultsFileNamePath = Path.Combine(projectDirectoryPath, resultsFileName);

        return resultsFileNamePath;
    }

    private void PrintResults()
    {
        Console.Clear();

        string pathToResults = GetResultsFilePath();

        const int FirstColumnWidth = 16;
        const int SecondColumnWidth = 25;
        const int ThirdColumnWidth = 10;

        string header = $"|| {"Имя пользователя",-FirstColumnWidth} || {"Кол-во правильных ответов",-SecondColumnWidth} || {"Диагноз",ThirdColumnWidth} ||";
        string separator = new string('=', header.Length);

        Console.WriteLine(separator);
        Console.WriteLine($"{header}");
        Console.WriteLine(separator);

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

                    Console.WriteLine($"|| {userName,FirstColumnWidth} || {rightAnswersCount,SecondColumnWidth} || {diagnose,ThirdColumnWidth} ||");
                    
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

        Console.WriteLine(separator);
        Console.ReadKey();
    }

    private void ExitGame()
    {
        _isPlaying = false;
        Console.WriteLine("Спасибо за игру!");
    }

    private void PrintInvalidInputMessage()
    {
        Console.WriteLine("Неверный ввод. Пожалуйста, выберите 1, 2 или 3.");
        Console.ReadKey();
    }
}