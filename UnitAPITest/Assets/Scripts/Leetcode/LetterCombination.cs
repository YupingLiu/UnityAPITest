using System;
using System.Collections.Generic;
using UnityEngine;

public class LetterCombination : MonoBehaviour
{
    void Start()
    {
        LetterCombinations("2");
    }

    public IList<string> LetterCombinations(string digits)
    {
        List<string> digitsMappingLst = new List<string> { "", "", "abc", "def", "ghi", "jkl", "mno", "pqrs", "tuv", "wxyz" };
        List<string> results = new List<string>();
        if (!String.IsNullOrEmpty(digits))
        {
            Combine(0, digits.Length, digits, string.Empty, digitsMappingLst, results);
        }
        return results;
    }

    private void Combine(int currIndex, int maxIndex, string source, string tempResult, List<string> digitsMappingLst, List<string> results)
    {
        if (currIndex == maxIndex)
        {
            results.Add(tempResult);
            return;
        }

        int digit = (int)Char.GetNumericValue(source[currIndex]);
        string mapping = digitsMappingLst[digit];
        if (mapping.Length > 0)
        {
            for (int i = 0; i < mapping.Length; i++)
            {
                Combine(currIndex + 1, maxIndex, source, tempResult + mapping[i], digitsMappingLst, results);
            }
        }
        else
        {
            Combine(currIndex + 1, maxIndex, source, tempResult, digitsMappingLst, results);
        }
    }
}
