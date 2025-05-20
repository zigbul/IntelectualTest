int arraySize = 5;

string[] questions = new string[arraySize];
questions[0] = "Сколько будет: 2 + 2 * 2?";
questions[1] = "Бревно нужно распилить на 10 частей, сколько нужно сделать распилов?";
questions[2] = "На двух руках 10 пальцев. Сколько пальцев на 5 руках?";
questions[3] = "Укол делают каждые полчаса, сколько минут для трех уколов?";
questions[4] = "Пять свечей горело, две потухли. Сколько свечей осталось?";

int[] answers = new int[arraySize];
answers[0] = 6;
answers[1] = 9;
answers[2] = 25;
answers[3] = 60;
answers[4] = 2;

int rightAnswersCount = 0;

Random random = new Random();

for (int i = 0; i < questions.Length; i++)
{
    int randomIndex = random.Next(0, arraySize);
    int questionNumber = i + 1;

    Console.WriteLine($"Вопрос №{questionNumber}:\n{questions[randomIndex]}\n");

    int userAnswer = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine();

    int rightAnswer = answers[randomIndex];

    if (userAnswer == rightAnswer)
    {
        rightAnswersCount++;
    }
}

string[] diagnoses = new string[arraySize + 1];
diagnoses[0] = "Идиот";
diagnoses[1] = "Кретин";
diagnoses[2] = "Дурак";
diagnoses[3] = "Нормальный";
diagnoses[4] = "Талант";
diagnoses[5] = "Гений";

Console.WriteLine($"Вы ответили правильно на {rightAnswersCount} из {questions.Length} вопросов.");
Console.WriteLine($"Поздравляю, Вы - {diagnoses[rightAnswersCount]}!");