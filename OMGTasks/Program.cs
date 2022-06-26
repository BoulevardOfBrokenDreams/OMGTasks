using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskExam
{
    internal class TaskSolver
    {
        public static void Main(string[] args)
        {
            TestGetWordSubWords();
            TestFindPath();
            TestFindPathPlus();
            TestFormatPrettyCoins();
            TestFormatPrettyCoinsPlus();
            FindMaxRect();

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

        #region methods for solving Индекс слова

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
            var stepMap = MakeStepMatrix(gridMap);
            var reversedGridMap = ReverseMatrix(gridMap);
            var steps = new Queue<Point>();
            steps.Enqueue(new Point(sX, sY, 0));

            while (steps.Count > 0)
            {
                MakeStep(reversedGridMap, ref stepMap, ref steps);
            }

            if(stepMap[eY][eX] > energyAmount || stepMap[eY][eX] == 0)
            {
                return -1;
            }

            return stepMap[eY][eX];
        }

        #region Methods and classes for solving Луноход

        private class Point
        {
            public int x;
            public int y;
            public int stepNumber;

            public Point(int x, int y, int stepNumber)
            {
                this.x = x;
                this.y = y;
                this.stepNumber = stepNumber;
            }
        }

        private static int[][] MakeStepMatrix(int[][] gridMap)
        {
            int[][] stepMatrix = new int[gridMap.Length][];
            int matrixWidth = gridMap[0].Length;

            for(int i = 0; i < stepMatrix.Length; i++)
            {
                stepMatrix[i] = new int[matrixWidth];
            }

            return stepMatrix;
        }

        private static int[][] ReverseMatrix(int[][] gridMap)
        {
            var matrix = new int[gridMap.Length][];
            
            for(int i = gridMap.Length - 1; i >= 0; i--)
            {
                matrix[gridMap.Length - 1 - i] = gridMap[i];
            }

            return matrix;
        }

        private static void MakeStep(int[][] gridMap, ref int[][] stepMap, ref Queue<Point> steps)
        {
            var mapHeight = gridMap.Length;
            var mapWidth = gridMap[0].Length;
            var tempStep = steps.Dequeue();

            if (tempStep.x + 1 < mapWidth && gridMap[tempStep.y][tempStep.x + 1] != 0  
                && (stepMap[tempStep.y][tempStep.x + 1] > tempStep.stepNumber || stepMap[tempStep.y][tempStep.x + 1] == 0))
            {
                stepMap[tempStep.y][tempStep.x + 1] = tempStep.stepNumber + 1;
                steps.Enqueue(new Point(tempStep.x + 1, tempStep.y, tempStep.stepNumber + 1));
            }

            if (tempStep.x - 1 > 0 && gridMap[tempStep.y][tempStep.x - 1] != 0
                && (stepMap[tempStep.y][tempStep.x - 1] > tempStep.stepNumber || stepMap[tempStep.y][tempStep.x - 1] == 0))
            {
                stepMap[tempStep.y][tempStep.x - 1] = tempStep.stepNumber + 1;
                steps.Enqueue(new Point(tempStep.x - 1, tempStep.y, tempStep.stepNumber + 1));
            }

            if (tempStep.y + 1 < mapHeight && gridMap[tempStep.y + 1][tempStep.x] != 0
                && (stepMap[tempStep.y + 1][tempStep.x] > tempStep.stepNumber || stepMap[tempStep.y + 1][tempStep.x] == 0))
            {
                stepMap[tempStep.y + 1][tempStep.x] = tempStep.stepNumber + 1;
                steps.Enqueue(new Point(tempStep.x, tempStep.y + 1, tempStep.stepNumber + 1));
            }

            if (tempStep.y - 1 > 0 && gridMap[tempStep.y - 1][tempStep.x] != 0
                && (stepMap[tempStep.y - 1][tempStep.x] > tempStep.stepNumber || stepMap[tempStep.y - 1][tempStep.x] == 0))
            {
                stepMap[tempStep.y - 1][tempStep.x] = tempStep.stepNumber + 1;
                steps.Enqueue(new Point(tempStep.x, tempStep.y - 1, tempStep.stepNumber + 1));
            }
        }

        #endregion

        /// задание 3) Монетки
        // в задании нигде не сказано про округление => функция не округляет
        public static string FormatPrettyCoins(long value, char separator)
        {
            var stringValue = value.ToString();
            switch (CheckValue(value))
            {
                case 'M':
                    return MakePrettyCoin(stringValue.Substring(0, stringValue.Length - 6), separator, 'M');
                case 'K':
                    return MakePrettyCoin(stringValue.Substring(0, stringValue.Length - 3), separator, 'K');
                default:
                    return MakePrettyCoin(stringValue, separator);
            }

            return string.Empty;
        }

        #region Methods for solving Монетки

        private static char CheckValue(long value)
        {
            if(value > 10000000) { return 'M'; }
            if(value > 100000) { return 'K'; }
            return 'L';
        }

        private static string MakePrettyCoin(string shortValue, char separator, char letter)
        {
            var valueLen = shortValue.Length;
            if(valueLen <= 3)
            {
                return shortValue + letter.ToString();
            }
            else
            {
                return shortValue.Substring(0, valueLen % 3) + separator.ToString() + shortValue.Substring(valueLen % 3, 3) + letter.ToString();
            }
        }

        private static string MakePrettyCoin(string shortValue, char separator)
        {
            var valueLen = shortValue.Length;
            if (valueLen <= 3)
            {
                return shortValue;
            }
            else
            {
                return shortValue.Substring(0, valueLen - 3) + separator.ToString() + shortValue.Substring(valueLen - 3, 3);
            }
        }

        #endregion

        /// задание 4) Самый большой прямоугольник на гистограмме
        public static int FindMaxRect(List<int> heights)
        {
            int minHeight = 0, maxHeight = 0, maxRect = 0;

            ExtremumSearch(heights, ref minHeight, ref maxHeight);

            var tempRect = 0;
            for(int i = minHeight; i <= maxHeight; i++)
            {
                tempRect = RectSearch(heights, i);

                if (tempRect > maxRect)
                {
                    maxRect = tempRect;
                }
            }

            return maxRect;
        }

        #region Methods for solving Самый большой прямоугольник на гистограмме

        private static void ExtremumSearch(List<int> heights, ref int minHeight, ref int maxHeight)
        {
            minHeight = heights[0];
            maxHeight = heights[0];

            for(int i = 0; i < heights.Count; i++)
            {
                if(heights[i] < minHeight)
                {
                    minHeight = heights[i];
                }
                if(heights[i] > maxHeight)
                {
                    maxHeight = heights[i];
                }
            }
        }

        private static int RectSearch(List<int> heights, int tempHeight)
        {
            int bestRect = 0;
            var tempRect = 0;
            for (int i = 0; i < heights.Count; i++)
            {
                if(heights[i] >= tempHeight)
                {
                    tempRect += tempHeight;
                }

                if(tempRect > bestRect)
                {
                    bestRect = tempRect;
                }

                if (heights[i] < tempHeight)
                {
                    tempRect = 0;
                }
            }

            return bestRect;
        }

        #endregion

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

        private static void TestFindPathPlus()
        {
            int[][] gridA =
            {
                new[] {1, 1, 1, 0, 1, 1, 1},
                new[] {1, 1, 1, 0, 1, 0, 1},
                new[] {1, 1, 1, 0, 0, 0, 1},
                new[] {1, 1, 1, 1, 1, 1, 1},
                new[] {1, 1, 1, 1, 1, 1, 1},
                new[] {1, 1, 1, 1, 1, 1, 1},
            };

            AssertEqual(FindPath(gridA, 0, 0, 4, 4, 14), 14);
            AssertEqual(FindPath(gridA, 0, 0, 4, 4, 13), -1);
        }

        private static void TestFormatPrettyCoins()
        {
            AssertEqual(FormatPrettyCoins(10, ' '), "10");
            AssertEqual(FormatPrettyCoins(1233, ' '), "1 233");
            AssertEqual(FormatPrettyCoins(1717310, ' '), "1 717K");
            AssertEqual(FormatPrettyCoins(7172343310, ' '), "7 172M");
        }

        private static void TestFormatPrettyCoinsPlus()
        {
            AssertEqual(FormatPrettyCoins(0, '_'), "0");
            AssertEqual(FormatPrettyCoins(10, '_'), "10");
            AssertEqual(FormatPrettyCoins(100, '_'), "100");
            AssertEqual(FormatPrettyCoins(1000, '_'), "1_000");
            AssertEqual(FormatPrettyCoins(10000, '_'), "10_000");
            AssertEqual(FormatPrettyCoins(100000, '_'), "100_000");
            AssertEqual(FormatPrettyCoins(100001, '_'), "100K");
            AssertEqual(FormatPrettyCoins(1000000, '_'), "1_000K");
            AssertEqual(FormatPrettyCoins(10000000, '_'), "10_000K");
            AssertEqual(FormatPrettyCoins(10000001, '_'), "10M");
            AssertEqual(FormatPrettyCoins(100000000, '_'), "100M");
            AssertEqual(FormatPrettyCoins(1000000000, '_'), "1_000M");
            AssertEqual(FormatPrettyCoins(10000000000, '_'), "10_000M");
        }

        private static void FindMaxRect()
        {
            AssertEqual(FindMaxRect(new List<int> { 1, 2, 3, 4, 4, 4, 5, 4, 6 }), 24);
            AssertEqual(FindMaxRect(new List<int> { 1, 2, 3, 5, 5, 4, 2, 4, 6 }), 16);
            AssertEqual(FindMaxRect(new List<int> { 8, 9, 3, 5, 5, 2, 3, 4, 6, 1, 6 }), 18);

            //plus
            AssertEqual(FindMaxRect(new List<int> { 1, 2, 3, 3, 3, 1, 3, 3, 3, 3 }), 12);
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