var questionsAndAnswers = new List<(string question, int answer)>()
{
    ("Сколько будет: 2 + 2 * 2?", 6),
    ("Бревно нужно распилить на 10 частей, сколько нужно сделать распилов?", 9),
    ("На двух руках 10 пальцев. Сколько пальцев на 5 руках?", 25),
    ("Укол делают каждые полчаса, сколько минут для трех уколов?", 60),
    ("Пять свечей горело, две потухли. Сколько свечей осталось?", 2)
};

int questionsCount = questionsAndAnswers.Count;
int rightAnswersCount = 0;
int questionNumber = 1;

Random random = new Random();

while (questionsAndAnswers.Count > 0)
{
    int randomIndex = random.Next(0, questionsAndAnswers.Count);
    
    var (question, answer) = questionsAndAnswers[randomIndex];

    Console.WriteLine($"Вопрос №{questionNumber}:\n{question}\n");

    int userAnswer = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine();

    int rightAnswer = answer;

    if (userAnswer == rightAnswer)
    {
        rightAnswersCount++;
    }

    questionNumber++;
    questionsAndAnswers.RemoveAt(randomIndex);
}

string[] diagnoses = new string[6];
diagnoses[0] = "Идиот";
diagnoses[1] = "Кретин";
diagnoses[2] = "Дурак";
diagnoses[3] = "Нормальный";
diagnoses[4] = "Талант";
diagnoses[5] = "Гений";

Console.WriteLine($"Вы ответили правильно на {rightAnswersCount} из {questionsCount} вопросов.");
Console.WriteLine($"Поздравляю, Вы - {diagnoses[rightAnswersCount]}!");