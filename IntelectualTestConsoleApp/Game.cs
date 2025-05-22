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

            Console.WriteLine("\n1. Пройти тест" +
                              "\n2. Посмотреть результаты" +
                              "\n3. Добавить свой вопрос" +
                              "\n4. Удалить вопрос" +
                              "\n5. Выйти из игры");

            int userInput;

            while (TryGetUserInput(out userInput) == false) { }

            switch (userInput)
            {
                case 1:
                    TestUser(user);
                    break;
                case 2:
                    PrintResults();
                    break;
                case 3:
                    AddQuestion();
                    break;
                case 4:
                    DeleteQuestion();
                    break;
                case 5:
                    ExitGame();
                    break;
                default:
                    PrintInvalidInputMessage();
                    break;
            }
        }
    }

    private void DeleteQuestion()
    {
        Console.Clear();

        var questions = new QuestionsStorage();

        if (questions.Count == 0)
        {
            Console.WriteLine("Вопросов нет. Добавьте вопрос, чтобы удалить его.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Список ворпосов:\n");
        questions.PrintQuestions();

        Console.Write("\nВведите номер вопроса, который хотите удалить:\n");

        int userInput;

        while (TryGetUserInput(out userInput) == false) { }

        if (userInput < 1 || userInput > questions.Count)
        {
            Console.WriteLine("Вопроса с таким индексом не существует. Попробуйте снова.");
            Console.ReadKey();
            return;
        }

        FileSystem.Delete<Question>(userInput - 1, FileNames.Questions);

        Console.WriteLine();
        Loading($"Удаляем вопрос номер {userInput}");

        Console.WriteLine($"\nВопрос номер {userInput} успешно удалён.\n");
        Console.ReadKey();
    }

    private void AddQuestion()
    {
        Console.Clear();

        Console.WriteLine("Введите вопрос, на который можно ответить целым числом.\n");

        string questionText = Console.ReadLine().Trim();

        Console.WriteLine("\nТеперь введите ответ на вопрос. Ответ должен быть целым числом.\n");

        int questionAnswer;

        while (TryGetUserInput(out questionAnswer) == false) { }

        var question = new Question(questionText, questionAnswer);

        FileSystem.Save(question, FileNames.Questions);

        Console.WriteLine("\nВопрос успешно добавлен");
        Console.ReadKey();
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
        catch (FormatException)
        {
            Console.WriteLine("Нужно ввести число!");
        }
        catch (OverflowException)
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
            user.ResetStats();

            Console.WriteLine($"{user.Name}, Вам нужно ответить на несколько вопросов. Начнём!\n");

            var questions = new QuestionsStorage();
            int questionsCount = questions.Count;

            int questionNumber = 1;

            while (questions.Count > 0)
            {
                var (question, rightAnswer) = questions.GetRandomQuestion();

                Console.WriteLine($"Вопрос №{questionNumber}: {question}\n");

                int userAnswer;

                while (TryGetUserInput(out userAnswer) == false) { }

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

            FileSystem.Save(user, FileNames.Users);

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

    private void PrintResults()
    {
        Console.Clear();

        const int FirstColumnWidth = 16;
        const int SecondColumnWidth = 25;
        const int ThirdColumnWidth = 10;

        string header = $"|| {"Имя пользователя",-FirstColumnWidth} || {"Кол-во правильных ответов",-SecondColumnWidth} || {"Диагноз",ThirdColumnWidth} ||";
        string separator = new string('=', header.Length);

        Console.WriteLine(separator);
        Console.WriteLine($"{header}");
        Console.WriteLine(separator);

        var users = new UsersStorage().Users;

        if (users.Count > 0)
        {
            foreach (var user in users)
            {
                var (userName, rightAnswersCount, diagnose) = user;

                if (userName.Length > FirstColumnWidth)
                {
                    userName = userName.Substring(0, FirstColumnWidth - 3) + "...";
                }

                Console.WriteLine($"|| {userName,-FirstColumnWidth} || {rightAnswersCount,-SecondColumnWidth} || {diagnose,ThirdColumnWidth} ||");
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
