using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MoreFun.Utils
{
    public class MathUtil
    {
        /// <summary>
        /// 按小数点后2数位四舍五入得到的带“%”的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetRoundPercentString(double input)
        {
            double result = Math.Round(input, 2);
            result = result * 100;
            string percentResult = result + "%";
            return percentResult;
        }

        public static string GetRoundString(double input, int roundNum)
        {
            double result = Math.Round(input, roundNum);
            string s = "".PadLeft(roundNum, '0');
            return result.ToString("0." + s);
        }
        public static bool Compare(float lOperand, float rOperand, CompareType compareType)
        {
            switch(compareType)
            {
                case CompareType.EQUAL:
                    if (Mathf.Abs(lOperand - rOperand) <= float.Epsilon)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case CompareType.NOT_EQUAL:
                    if (Mathf.Abs(lOperand - rOperand) > float.Epsilon)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case CompareType.LESS:
                    if (lOperand < rOperand)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case CompareType.LESS_OR_EQUAL:
                    if (lOperand <= rOperand)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case CompareType.GREATER:
                    if (lOperand > rOperand)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case CompareType.GREATER_OR_EQUAL:
                    if (lOperand >= rOperand)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return true;
            }
        }


        public static bool Compare(int lOperand, int rOperand, CompareType compareType)
        {
            switch (compareType)
            {
                case CompareType.EQUAL:
                    if (lOperand == rOperand)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case CompareType.NOT_EQUAL:
                    if (lOperand != rOperand)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case CompareType.LESS:
                    if (lOperand < rOperand)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case CompareType.LESS_OR_EQUAL:
                    if (lOperand <= rOperand)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case CompareType.GREATER:
                    if (lOperand > rOperand)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case CompareType.GREATER_OR_EQUAL:
                    if (lOperand >= rOperand)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return true;
            }
        }
    }

    public enum CompareType
    {
        EQUAL = 0,
        NOT_EQUAL = 1,
        LESS = 2,
        GREATER = 3,
        LESS_OR_EQUAL = 4,
        GREATER_OR_EQUAL = 5
    }
}
