string[] questions = new string[5];
questions[0] = "Сколько будет: 2 + 2 * 2?";
questions[1] = "Бревно нужно распилить на 10 частей, сколько нужно сделать распилов?";
questions[2] = "На двух руках 10 пальцев. Сколько пальцев на 5 руках?";
questions[3] = "Укол делают каждые полчаса, сколько минут для трех уколов?";
questions[4] = "Пять свечей горело, две потухли. Сколько свечей осталось?";

int[] answers = new int[5];
answers[0] = 6;
answers[1] = 9;
answers[2] = 25;
answers[3] = 60;
answers[4] = 2;

int rightAnswersCount = 0;

for (int i = 0; i < questions.Length; i++)
{
    Console.WriteLine(questions[i]);

    int userAnswer = Convert.ToInt32(Console.ReadLine());

    int rightAnswer = answers[i];

    if (userAnswer == rightAnswer)
    {
        rightAnswersCount++;
    }
}

string[] results = new string[5];
results[0] = "";

Console.WriteLine("Вы ответили правильно на " + rightAnswersCount + " из " + questions.Length + " вопросов.");

