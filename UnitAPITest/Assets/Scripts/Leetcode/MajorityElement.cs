using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
public class MajorityElement : MonoBehaviour
{
    void Start()
    {
        int majority = MajorityElementCancellation(new int[] { 1, 2, 1, 5, 1, 2, 3, 1 });
    }

    //法1：哈希表    Runtime: O(n), Space: O(n) — Hash table: Maintain a hash table of the counts of each element, then find the most common one.
    //法2：随机数    Average runtime: O(n), Worst case runtime: Infinity — Randomization: Randomly pick an element and check if it is the majority element. If it is not, do the random pick again until you find the majority element. As the probability to pick the majority element is greater than 1/2, the expected number of attempts is < 2.
    //法3：分治法    Runtime: O(n log n) — Divide and conquer: Divide the array into two halves, then find the majority element A in the first half and the majority element B in the second half. The global majority element must either be A or B. If A == B, then it automatically becomes the global majority element. If not, then both A and B are the candidates for the majority element, and it is suffice to check the count of occurrences for at most two candidates. The runtime complexity, T(n) = T(n/2) + 2n = O(n logn).
    //法4：相消法    Runtime: O(n) — Moore voting algorithm: We maintain a current candidate and a counter initialized to 0. As we iterate the array, we look at the current element x:
    //If the counter is 0, we set the current candidate to x and the counter to 1.
    //If the counter is not 0, we increment or decrement the counter based on whether x is the current candidate.
    //After one pass, the current candidate is the majority element. Runtime complexity = O(n).
    //法5：位运算    Runtime: O(n) — Bit manipulation: We would need 32 iterations, each calculating the number of 1's for the ith bit of all n numbers. Since a majority must exist, therefore, either count of 1's > count of 0's or vice versa (but can never be equal). The majority number’s ith bit must be the one bit that has the greater count.

    //下面是我自己的最普通，效率最低的算法：180 ms
    public int MajorityElementDic(int[] nums)
    {
        int num = 0;
        int majority = 0;
        int count;
        Dictionary<int, int> countResult = new Dictionary<int, int>();
        for (int i = 0; i < nums.Length; i++)
        {
            if (countResult.TryGetValue(nums[i], out count))
            {
                count++;
                countResult[nums[i]] = count;
            }
            else
            {
                countResult[nums[i]] = 1;
            }
        }
        foreach (var item in countResult.Keys)
        {
            if (countResult[item] > num)
            {
                num = countResult[item];
                majority = item;
            }
        }
        return majority;
    }

    /// <summary>
    /// 200ms  就是说嘛，有装箱拆箱，怎么会比dictionary快呢，真是的
    /// </summary>
    /// <param name="nums"></param>
    /// <returns></returns>
    public int MajorityElementHashTable(int[] nums)
    {
        int num = 0;
        int majority = 0;
        int count;
        Hashtable countResult = new Hashtable();
        
        for (int i = 0; i < nums.Length; i++)
        {
            if (countResult.ContainsKey(nums[i]))
            {
                count = (int)countResult[(int)nums[i]] + 1;
                countResult[nums[i]] = count;
            }
            else
            {
                countResult[nums[i]] = 1;
            }
        }
        foreach (var item in countResult.Keys)
        {
            if((int)countResult[(int)item] > num)
            {
                num = (int)countResult[item];
                majority = (int)item;
            }
        }
        return majority;
    }

    /// <summary>
    /// 相消法 168ms 效率超高  注意前提是：more than n/2 times才可以用这个方法
    /// </summary>
    /// <param name="nums"></param>
    /// <returns></returns>
    public int MajorityElementCancellation(int[] nums)
    {
        int majority = nums[0];
        int count = 0;
        
        for (int i = 0; i < nums.Length; i++)
        {
            if (count == 0 || majority == nums[i])
            {
                count++;
                majority = nums[i];
            }
            else
            {
                count--;
            }
        }
        return majority;
    }
}