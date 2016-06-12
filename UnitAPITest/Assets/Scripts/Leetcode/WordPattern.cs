using System;
using System.Collections.Generic;
using UnityEngine;

public class WordPattern : MonoBehaviour
{
    public string pattern;
    public string str;

    void Start()
    {
        CheckPatter(pattern, str);
    }

    private bool CheckPatter(string pattern, string str)
    {
        char[] sperators = new char[] { ' ' };
        string[] splitResults = str.Split(sperators, StringSplitOptions.RemoveEmptyEntries);
        char[] patternArray = pattern.ToCharArray();
        Dictionary<char, string> resultPattern = new Dictionary<char, string>();
        List<string> uniqueStrLst = new List<string>();

        if (patternArray.Length != splitResults.Length)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < patternArray.Length; i++)
            {
                string result = string.Empty;
                if (resultPattern.TryGetValue(patternArray[i], out result))
                {
                    if (result != splitResults[i])
                    {
                        return false;
                    }
                }
                else
                {
                    resultPattern[patternArray[i]] = splitResults[i];
                    if (uniqueStrLst.Contains(splitResults[i]))
	                {
		                return false;
	                }
                    else
                    {
                        uniqueStrLst.Add(splitResults[i]);
                    }
                }
            }
        }
        return true;
    }

}