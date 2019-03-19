using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Lab6
{
    public class Code
    {
        private static char[] Alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
        'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
        'Y', 'Z' };

        private static double[] AlphabetFrequency = { 8.17, 1.49, 2.78, 4.25, 12.7, 2.23,
        2.02, 6.09, 6.97, 0.15, 0.77, 4.03, 2.41, 6.75, 7.51, 1.93, 0.1, 5.99, 6.33,
        9.06, 2.76, 0.98, 2.36, 0.15, 1.97, 0.05 };

        private static double IndexThreshold = 1.4;

        public static string Preparation(string Input)
        {
            Regex SpecialCharacters = new Regex(@"\W|[0-9]");
            return SpecialCharacters.Replace(Input, "");
        }

        public static string[] GetKeys(string Input)
        {
            List<string> Keys = new List<string> { };
            int[] PotentialKeyLengths = GetKeyLengths(Input);
            for (int i = 0; i < PotentialKeyLengths.Length; i++)
            {
                char[] PotentialKey = new char[PotentialKeyLengths[i]];
                for (int z = 0; z < PotentialKeyLengths[i]; z++)
                {
                    double MaxFrequencyCoef = 0;
                    int TrueShift = 0;
                    for (int Shift = 0; Shift < 26; Shift++)
                    {
                        double FrequencyCoef = 0;
                        double LetterCount = 0;
                        for (int j = z; j < Input.Length; j += PotentialKeyLengths[i])
                        {
                            if (/*Alphabet[Shift]*/(char)(Shift + 'A') == Input[j]) LetterCount++;
                        }
                        for (int l = 0; l < 26; l++)
                        {
                            FrequencyCoef += AlphabetFrequency[l] * LetterCount /
                                (Input.Length / PotentialKeyLengths[i]);
                        }
                        if (FrequencyCoef > MaxFrequencyCoef)
                        {
                            MaxFrequencyCoef = FrequencyCoef;
                            TrueShift = Shift;
                        }
                    }
                    PotentialKey[z] = (char)(TrueShift + 'A' - 4);
                }
                Keys.Add(new string(PotentialKey));
            }
            return Keys.ToArray();
        }

        public static int[] GetKeyLengths(string Input)
        {
            Input = Preparation(Input);
            int[] PotentialKeyLengths = new int[3];
            double CurrentIndex = GetIndexOfCoincidence(Input, 1);
            int ArrayThreshold = 0;
            for (int i = 2; i < Input.Length; i++)
            {
                double PreviousIndex = CurrentIndex;
                CurrentIndex = GetIndexOfCoincidence(Input, i);
                if ((CurrentIndex / PreviousIndex) > IndexThreshold)
                {
                    PotentialKeyLengths[ArrayThreshold] = i;
                    ArrayThreshold++;
                }
                if (ArrayThreshold >= PotentialKeyLengths.Length) break;
            }
            return PotentialKeyLengths;
        }

        public static double GetIndexOfCoincidence(string Input, int PotentialKeyLength)
        {
            double IndexOfCoincidence = 0;
            for (int i = 0; i < 26; i++)
            {
                int LetterCount = 0;
                for (int j = 1; j < Input.Length; j += PotentialKeyLength)
                {
                    if (Alphabet[i] == Input[j])
                    {
                        LetterCount++;
                    }
                }
                IndexOfCoincidence += (double)(LetterCount * (LetterCount - 1)) / 
                    ((Input.Length / PotentialKeyLength) * (Input.Length /
                    PotentialKeyLength - 1));
            }
            return IndexOfCoincidence;
        }

        public static string Decrypt(string Input, string Key)
        {
            char[] Output = new char[Input.Length];
            Input = Input.ToUpper();
            Key = Key.ToUpper();
            int Count = 0;
            for (int i = 0; i < Input.Length; i++)
            {
                if (Input[i] >= 'A' && Input[i] <= 'Z')
                {
                    Output[i] = (char)((Input[i] - Key[Count % Key.Length] + 26) % 26 + 'A');
                    Count++;
                }
                else
                {
                    Output[i] = Input[i];
                }
            }
            return new string(Output);
        }
    }
}
