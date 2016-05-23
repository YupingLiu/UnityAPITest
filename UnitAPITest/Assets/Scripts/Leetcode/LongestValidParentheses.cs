using MoreFun;
using System;
using System.Collections.Generic;
using UnityEngine;
public class LongestValidParentheses : MonoBehaviour
{
    public string testStr;
    public void Start()
    {
        MoreDebug.MoreLog(GetLongestValidParentheses(testStr));
    }

    public int GetLongestValidParentheses(string s)
    {
        int longestLength = 0;

        char[] source = s.ToCharArray();
        Stack<int> stack = new Stack<int>();
        // begin index  - end index
        List<KeyValuePair<int, int>> pairResultsLst = new List<KeyValuePair<int, int>>();
        List<int> combineResultsLst = new List<int>();
        int startIndex = -1;
        KeyValuePair<int, int> temp;

        for (int i = 0; i < source.Length; i++)
        {
            if ('(' == source[i])
            {
                stack.Push(i);
                startIndex = i;
            }
            else
            {
                if (stack.Count > 0)
                {
                    startIndex = stack.Peek();
                    stack.Pop();
                    temp = new KeyValuePair<int, int>(startIndex, i);
                    pairResultsLst.Add(temp);
                }
            }
        }

        pairResultsLst.Sort(SortIndex);

        // 组合
        if (pairResultsLst.Count > 0)
        {
            temp = pairResultsLst[0];

            if (pairResultsLst.Count > 1)
            {
                for (int i = 1; i < pairResultsLst.Count; i++)
                {
                    if (1 + temp.Value == pairResultsLst[i].Key)
                    {
                        temp = new KeyValuePair<int, int>(temp.Key, pairResultsLst[i].Value);
                    }
                    else if (1 + temp.Value < pairResultsLst[i].Key)
                    {
                        combineResultsLst.Add(temp.Value - temp.Key + 1);
                        temp = pairResultsLst[i];
                    }
                }
                // 收尾
                combineResultsLst.Add(temp.Value - temp.Key + 1);
            }
            else
            {
                longestLength = temp.Value - temp.Key + 1;
            }
        }
        for (int i = 0; i < combineResultsLst.Count; i++)
        {
            longestLength = Math.Max(combineResultsLst[i], longestLength);
        }

        return longestLength;
    }

    private int SortIndex(KeyValuePair<int, int> pair1, KeyValuePair<int, int> pair2)
    {
        return (pair1.Key).CompareTo(pair2.Key);
    }
}
