using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskExam
{
    internal class TaskSolver
    {
        public static void Main(string[] args)
        {
            //TestGetWordSubWords();
            TestFindPath();
            //TestFormatPrettyCoins();
            //FindMaxRect();

            Console.WriteLine("All Test completed!");
        }

        /// задание 1) Индекс слова
        public static List<int> GetWordSubWords(List<string> words, List<string> wordDictionary)
        {
            List<int> result = new List<int>(words.Count - 1);

            for(int i = 0; i < words.Count; i++)
            {
                words[i] = new string(Quicksort(words[i].ToArray<char>(), 0, words[i].Length - 1));
            }

            for(int i = 0; i < wordDictionary.Count; i++)
            {
                wordDictionary[i] = new string(Quicksort(wordDictionary[i].ToArray<char>(), 0, wordDictionary[i].Length - 1));
            }

            for(int i = 0; i < words.Count; i++)
            {
                result.Add(CountDerivedWord(words[i], ref wordDictionary));
            }

            return result;
        }

        #region methods for solving индекс слова

        private static int Partition(char[] array, int start, int end)
        {
            int marker = start; 
            for (int i = start; i < end; i++)
            {
                if (array[i] < array[end]) 
                {
                    (array[marker], array[i]) = (array[i], array[marker]);
                    marker += 1;
                }
            }
            
            (array[marker], array[end]) = (array[end], array[marker]);
            return marker;
        }

        private static char[] Quicksort(char[] array, int start, int end)
        {
            if (start >= end)
                return null;

            int pivot = Partition(array, start, end);
            Quicksort(array, start, pivot - 1);
            Quicksort(array, pivot + 1, end);

            return array;
        }

        private static int CountDerivedWord(string word, ref List<string> wordDictionary)
        {
            int IndexInDictionary = 0;
            for(int i = 0; i < wordDictionary.Count; i++)
            {
                if (word.Length < wordDictionary[i].Length)
                {
                    continue;
                }

                if(word == wordDictionary[i])
                {
                    IndexInDictionary++;
                    continue;
                }
                
                for(int tempWordIndex = 0, DictionaryWordIndex = 0; tempWordIndex < word.Length && DictionaryWordIndex < wordDictionary[i].Length;)
                {
                    if (word[tempWordIndex] == wordDictionary[i][DictionaryWordIndex])
                    {
                        DictionaryWordIndex++;
                        tempWordIndex++;
                    }
                    else
                    {
                        tempWordIndex++;
                    }

                    if(DictionaryWordIndex == wordDictionary[i].Length)
                    {
                        IndexInDictionary++;
                        break;
                    }
                }
            }

            return IndexInDictionary;
        }

        #endregion

        /// задание 2) Луноход
        public static int FindPath(int[][] gridMap, int sX, int sY, int eX, int eY, int energyAmount)
        {
            //код алгоритма
            return -1;
        }

        /// задание 3) Монетки
        public static string FormatPrettyCoins(long value, char separator)
        {
            //код алгоритма
            return string.Empty;
        }

        /// задание 4) Самый большой прямоугольник на гистограмме
        public static int FindMaxRect(List<int> heights)
        {
            //algorithm
            return 0;
        }

        /// Тесты (можно/нужно добавлять свои тесты) 

        private static void TestGetWordSubWords()
        {
            var wordsList = new List<string>
            {
                "кот", "ток", "око", "мимо", "гром", "ром",
                "рог", "морг", "огр", "мор", "порог"
            };

            AssertSequenceEqual(GetWordSubWords(new List<string> { "кот" }, wordsList), new[] { 2 });
            AssertSequenceEqual(GetWordSubWords(new List<string> { "мама" }, wordsList), new[] { 0 });
            AssertSequenceEqual(GetWordSubWords(new List<string> { "погром", "гром" }, wordsList), new[] { 7, 6 });
        }

        private static void TestFindPath()
        {
            int[][] gridA =
            {
                new[] {1, 1, 1, 0, 1, 1},
                new[] {1, 1, 1, 0, 1, 1},
                new[] {1, 1, 1, 0, 0, 0},
                new[] {1, 1, 1, 1, 1, 1},
                new[] {1, 1, 1, 1, 1, 1},
                new[] {1, 1, 1, 1, 1, 1},
            };

            AssertEqual(FindPath(gridA, 0, 0, 2, 2, 5), 4);
            AssertEqual(FindPath(gridA, 0, 0, 5, 5, 30), -1);
            AssertEqual(FindPath(gridA, 0, 0, 0, 5, 3), -1);
        }

        private static void TestFormatPrettyCoins()
        {
            AssertEqual(FormatPrettyCoins(10, ' '), "10");
            AssertEqual(FormatPrettyCoins(1233, ' '), "1 233");
            AssertEqual(FormatPrettyCoins(1717310, ' '), "1 717K");
            AssertEqual(FormatPrettyCoins(7172343310, ' '), "7 172M");
        }

        private static void FindMaxRect()
        {
            AssertEqual(FindMaxRect(new List<int> { 1, 2, 3, 4, 4, 4, 5, 4, 6 }), 24);
            AssertEqual(FindMaxRect(new List<int> { 1, 2, 3, 5, 5, 4, 2, 4, 6 }), 16);
            AssertEqual(FindMaxRect(new List<int> { 8, 9, 3, 5, 5, 2, 3, 4, 6, 1, 6 }), 18);
        }

        /// Тестирующая система, лучше не трогать этот код

        private static void Assert(bool value)
        {
            if (value)
            {
                return;
            }

            throw new Exception("Assertion failed");
        }

        private static void AssertEqual(object value, object expectedValue)
        {
            if (value.Equals(expectedValue))
            {
                return;
            }

            throw new Exception($"Assertion failed expected = {expectedValue} actual = {value}");
        }

        private static void AssertSequenceEqual<T>(IEnumerable<T> value, IEnumerable<T> expectedValue)
        {
            if (ReferenceEquals(value, expectedValue))
            {
                return;
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (expectedValue is null)
            {
                throw new ArgumentNullException(nameof(expectedValue));
            }

            var valueList = value.ToList();
            var expectedValueList = expectedValue.ToList();

            if (valueList.Count != expectedValueList.Count)
            {
                throw new Exception($"Assertion failed expected count = {expectedValueList.Count} actual count = {valueList.Count}");
            }

            for (var i = 0; i < valueList.Count; i++)
            {
                if (!valueList[i].Equals(expectedValueList[i]))
                {
                    throw new Exception($"Assertion failed expected value at {i} = {expectedValueList[i]} actual = {valueList[i]}");
                }
            }
        }

    }

}