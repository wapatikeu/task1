using System;
using System.Linq;
using System.Threading;

class Program
{
    static char[] availableLetters;
    static string[] usedWords = Array.Empty<string>();
    static string initialWord;

    static void Main(string[] args)
    {
        PlayGame();
    }

    static string InputWord()
    {
        while (true)
        {
            Console.WriteLine("Введите начальное слово (8-30 букв):");
            string word = Console.ReadLine()?.Trim().ToLower();

            if (word.Length < 8 || word.Length > 30)
            {
                Console.WriteLine("Слово должно быть от 8 до 30 символов!");
                continue;
            }

            if (!word.All(char.IsLetter))
            {
                Console.WriteLine("Слово должно содержать только буквы!");
                continue;
            }

            return word;
        }
    }

    static void PlayGame()
    {
        initialWord = InputWord();
        availableLetters = initialWord.ToCharArray();
        bool gameOver = false;
        int currentPlayer = 1;
        var timer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);

        Console.WriteLine($"\nНачальное слово: {initialWord}");

        while (!gameOver)
        {
            Console.WriteLine($"\nИгрок {currentPlayer}, ваш ход. Введите слово:");

            timer.Change(30000, Timeout.Infinite);
            string input = Console.ReadLine()?.Trim().ToLower();
            timer.Change(Timeout.Infinite, Timeout.Infinite);

            if (string.IsNullOrEmpty(input))
            {
                gameOver = true;
                Console.WriteLine($"Игрок {currentPlayer} не ввел слово и проиграл!");
                break;
            }

            if (ValidateWord(input))
            {
                usedWords = usedWords.Append(input).ToArray();
                SubtractLetters(input);
                currentPlayer = currentPlayer == 1 ? 2 : 1;
            }
            else
            {
                gameOver = true;
                Console.WriteLine($"Игрок {currentPlayer} ввел недопустимое слово и проиграл!");
            }
        }
    }

    static bool ValidateWord(string word)
    {
        if (word == initialWord)
        {
            Console.WriteLine("Нельзя использовать начальное слово!");
            return false;
        }

        if (usedWords.Contains(word))
        {
            Console.WriteLine("Это слово уже было использовано!");
            return false;
        }

        var tempLetters = availableLetters.ToArray();
       foreach (char c in word)
        {
            int index = Array.IndexOf(tempLetters, c);
            if (index == -1)
            {
                Console.WriteLine($"Буква '{c}' недоступна!");
                return false;
            }
            tempLetters[index] = '\0';
        }

        return true;
    }

    static void SubtractLetters(string word)
    {
        foreach (char c in word)
        {
            int index = Array.IndexOf(availableLetters, c);
            if (index != -1)
            {
                availableLetters[index] = '\0';
            }
        }
    }

    static void TimerCallback(object state)
    {
        Console.WriteLine("\nВремя вышло! Нажмите Enter для продолжения...");
    }
}