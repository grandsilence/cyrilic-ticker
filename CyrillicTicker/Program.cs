using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CyrillicTicker
{
    internal class Program
    {
        private static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("Ошибка: " + message);
            Console.ReadLine();
            Environment.Exit(-1);
        }

        private static void Main(string[] args)
        {
            Console.Title = "Кириллица для бегущей строки v1.0";

            if (args.Length == 0) 
                Error("Откройте файл в этой программе");

            string inputFile = args[0];

            try
            {
                Console.WriteLine($"Обработка: {inputFile}");

                string outputFile = FileConverter.Encode(inputFile);
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Сохранено: {outputFile}");

                const string process = "Msgeditor.exe";
                if (File.Exists(process))
                {
                    Console.Write($"{Environment.NewLine}Нажмите Пробел чтобы открыть программу в редакторе.");
                    var answer = Console.ReadKey(false);
                    if (answer.Key == ConsoleKey.Spacebar)
                        Process.Start(process, outputFile);
                }
                else
                {
                    Console.WriteLine($"{Environment.NewLine}Программа закроется сама через 5 секунд или закройте её сами.");
                    Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }
    }
}
