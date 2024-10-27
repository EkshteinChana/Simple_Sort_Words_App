using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace InterviewExam
{
    public class SortWords
    {
        static void Main(string[] args)
        {
            var inputFilePath = ValidateInput(args);
            if (inputFilePath == null) return;

            var outputFilePath = Path.Combine(Path.GetDirectoryName(inputFilePath) ?? throw new InvalidOperationException("Unable to determine output directory."), "F2.txt");

            var ascending = GetSortOrder();
            if (ascending == null)
            {
                LogError("Invalid sort order. Use 'a' for ascending or 'd' for descending.");
                return;
            }

            try
            {
                var words = ReadWordsFromFile(inputFilePath);
                if (words == null)
                {
                    LogError("Failed to read input file.");
                    return;
                }

                var sortedWords = SortWordList(words, ascending.Value);
                WriteWordsToFile(sortedWords, outputFilePath);

                // Find and log the most frequent word from F1
                var mostFrequentWord = GetMostFrequentWord(words);
                Log($"The most frequent word in F1 is '{mostFrequentWord.Word}', count: {mostFrequentWord.Count}");
                Log($"The file F2 has been created at: {outputFilePath}");
            }
            catch (Exception ex)
            {
                LogError($"Unexpected error: {ex.Message}");
            }
        }

        // Validate input and return the input file path
        static string? ValidateInput(string[] args)
        {
            if (args.Length == 0)
            {
                LogError("Usage: Sortwords.exe \"path_to_F1.txt\"");
                return null;
            }

            var inputFilePath = args[0];

            if (!File.Exists(inputFilePath))
            {
                LogError("Input file not found.");
                return null;
            }

            if (Path.GetExtension(inputFilePath).ToLower() != ".txt")
            {
                LogError("Input file must be a .txt file.");
                return null;
            }

            return inputFilePath;
        }


        // Get the user's choice for sorting order
        static bool? GetSortOrder()
        {
            Log("Choose sort order (a for ascending, d for descending): ");
            var sortOrder = Console.ReadLine()?.ToLower();

            if (sortOrder == "a") return true;
            if (sortOrder == "d") return false;
            return null;
        }

        // Read words from the input file, ignoring numbers and converting to lower case
        public static List<string>? ReadWordsFromFile(string filePath)
        {
            try
            {
                var content = File.ReadAllText(filePath);
                var words = Regex.Split(content, @"\W+")
                                 .Where(word => !string.IsNullOrEmpty(word) && !int.TryParse(word, out _)) // Ignore numbers
                                 .Select(word => word.ToLower()) // Ensure all words are in lower case
                                 .ToList();
                return words;
            }
            catch (Exception ex)
            {
                LogError($"Error reading file: {ex.Message}");
            }
            return null;
        }

        // Sort the list of words either in ascending or descending order
        public static IEnumerable<string> SortWordList(List<string> words, bool ascending)
        {
            var uniqueWords = words.Distinct(StringComparer.OrdinalIgnoreCase);
            return ascending
                ? uniqueWords.OrderBy(word => word, StringComparer.OrdinalIgnoreCase)
                : uniqueWords.OrderByDescending(word => word, StringComparer.OrdinalIgnoreCase);
        }

        // Write the sorted words to the output file
        static void WriteWordsToFile(IEnumerable<string> words, string filePath)
        {
            try
            {
                File.WriteAllText(filePath, string.Join(", ", words));
            }
            catch (Exception ex)
            {
                LogError($"Error writing to file: {ex.Message}");
            }
        }

        // Get the most frequently occurring word in the list
        public static (string Word, int Count) GetMostFrequentWord(List<string> words)
        {
            var wordFrequency = words
                .GroupBy(word => word, StringComparer.OrdinalIgnoreCase)
                .Select(group => new { Word = group.Key, Count = group.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            return wordFrequency != null ? (wordFrequency.Word, wordFrequency.Count) : ("", 0);
        }

        // Log a message to the console
        static void Log(string message)
        {
            Console.WriteLine(message);
        }

        // Log an error message in red
        static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {message}");
            Console.ResetColor();
        }
    }
}
