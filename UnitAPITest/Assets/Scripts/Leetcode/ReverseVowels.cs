using UnityEngine;
using System.Collections;
using MoreFun;
using System.Collections.Generic;
public class ReverseVowels : MonoBehaviour {

	// Use this for initialization
	void Start () {
        reverseVowels("are you kidding");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private string reverseVowels(string s)
    {
        List<char> allVowels = new List<char> {'A','E','I','O','U','a','e','i','o','u'};

        char[] arr = s.ToCharArray();

        int i = 0;
        int j = s.Length - 1;

        while (i < j)
        {
            if (!allVowels.Contains(arr[i]))
            {
                i++;
                continue;
            }

            if (!allVowels.Contains(arr[j]))
            {
                j--;
                continue;
            }

            char t = arr[i];
            arr[i] = arr[j];
            arr[j] = t;

            i++;
            j--;
        }

        return new string(arr);
    }
}
