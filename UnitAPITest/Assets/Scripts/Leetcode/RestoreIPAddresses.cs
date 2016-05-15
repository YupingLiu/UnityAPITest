using MoreFun;
using System;
using System.Collections.Generic;
using UnityEngine;
public class RestoreIPAddresses: MonoBehaviour 
{
    public void Start()
    {
        List<string> results = RestoreIpAddresses("25512324") as List<string>;
        for (int i = 0; i < results.Count; i++)
        {
            MoreDebug.MoreLog(results[i]);
        }
    }

    public IList<string> RestoreIpAddresses(string s) {
        List<string> results = new List<string>();

        if (null != s && s.Length >= 4 && s.Length <= 12)
	    {
		     SplitString(0, 1, "", results, s);
	    }

        return results;
    }

    private void SplitString(int startIndex, int segmentIndex, string item, List<string> results, string source)
    {
        if (startIndex >= source.Length)
	    {
		    return;
	    }
        // IP地址是4段
        if (4 == segmentIndex)
	    {
		    string subStr = source.Substring(startIndex);
            if (IsValidSegment(subStr))
            {
                item += subStr;
                results.Add(item);
            }
            return;
	    }
        // 最长只能是三位数
        for (int i = 1; i < 4 && ((startIndex + i) < source.Length); i++)
		{
            string subStr = source.Substring(startIndex, i);
            if (IsValidSegment(subStr))
            {
                // 回溯法
                SplitString(startIndex + i, segmentIndex + 1, item + subStr + '.', results, source);
	        }
		}
    }

    private bool IsValidSegment(string str)
    {
        if (str[0] == '0' && str.Length > 1)
	    {
            return false;
	    }
        int num = 0;
        if (Int32.TryParse(str, out num))
        {
            if (num < 0 || num > 255)
	        {
		        return false;
	        }
        }
        return true;
    }
}
