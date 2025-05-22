

public static class FileSystem
{
    private static List<Question> _baseQuestions = new List<Question>()
    {
        new("Сколько будет: 2 + 2 * 2?", 6),
        new("Бревно нужно распилить на 10 частей, сколько нужно сделать распилов?", 9),
        new("На двух руках 10 пальцев. Сколько пальцев на 5 руках?", 25),
        new("Укол делают каждые полчаса, сколько минут для трех уколов?", 60),
        new("Пять свечей горело, две потухли. Сколько свечей осталось?", 2),
        new("Сколько месяцев в году имеют 28 дней?", 12),
        new("Сколько яиц можно съесть натощак? ", 1)
    };

    private static string GetPathToFile(FileNames filename)
    {
        string currentDirectoryPath = Directory.GetCurrentDirectory();
        string projectDirectoryPath = Path.Combine(currentDirectoryPath, @"..\..\..\");
        string fileName = $"{filename}.txt";
        string fileNamePath = Path.Combine(projectDirectoryPath, fileName);

        return fileNamePath;
    }

    public static List<T> Load<T>(FileNames fileName)
    {
        var data = new List<T>();
        var type = typeof(T);

        string pathToFile = GetPathToFile(fileName);

        bool isNotExist = File.Exists(pathToFile) == false;
        bool isEmpty = true;

        try
        {
            isEmpty = new FileInfo(pathToFile).Length == 0;
        }
        catch
        {
            isEmpty = true;
        }
        

        if (isNotExist || isEmpty)
        {
            using FileStream fs = File.Create(pathToFile);
            fs.Close();

            if (type == typeof(Question))
            {
                foreach (var question in _baseQuestions)
                {
                    Save(question, FileNames.Questions);
                    data.Add((T)(object)question);
                }

            }

            return data;
        }

        using (StreamReader sr = new StreamReader(pathToFile))
        {
            string line = sr.ReadLine();

            while (string.IsNullOrEmpty(line) == false)
            {
                var lineInfo = line.Split('#');

                if (type == typeof(User))
                {
                    if (lineInfo.Length < 3) continue;

                    string userName = lineInfo[0];
                    int rightAnswersCount = int.Parse(lineInfo[1]);
                    string diagnose = lineInfo[2];

                    data.Add((T)(object)new User(userName, rightAnswersCount)
                    {
                        Diagnosis = diagnose
                    });
                }

                if (type == typeof(Question))
                {
                    if (lineInfo.Length < 2) continue;

                    string questionText = lineInfo[0];
                    int rightAnswer = int.Parse(lineInfo[1]);
                    data.Add((T)(object)new Question(questionText, rightAnswer));
                }

                line = sr.ReadLine();
            }

            sr.Close();
        }

        return data;
    }

    public static void Save<T>(T data, FileNames filename)
    {
        string pathToFile = GetPathToFile(filename);

        using (StreamWriter writer = new StreamWriter(pathToFile, true, System.Text.Encoding.Default))
        {
            writer.WriteLine($"{data}");
            writer.Close();
        }
    }

    public static void Delete<T>(int index, FileNames fileName)
    {
        string pathToFile = GetPathToFile(fileName);

        var data = Load<T>(fileName).ToList();
        data.RemoveAt(index);

        File.WriteAllLines(pathToFile, data.Select(item => item.ToString()));
    }
}
