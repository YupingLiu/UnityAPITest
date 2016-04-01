using UnityEngine;
using System.Diagnostics;
using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

namespace MoreFun
{
    public enum MoreLogLevel
    {
        Info = 1,
        Log,
        Warning,
        Error,
        Assert,
        None = 256
    }

    /// <summary>
    /// More debug.
    /// <para>Log message with type info, method info and date time info.</para>
    /// 
    /// <code>
    /// using MoreFun;
    /// 
    /// public class YourClass
    /// {
    ///     public void MemberFunction()
    ///     {
    ///         // functions with "More" will log only when conditional "MoreDebug" is predefined
    ///         this.MoreLog();
    ///         this.MoreLog("message");
    ///         this.MoreLog("m1", "m2", "m3");
    ///         this.MoreLogFormat("value={0}, value2={1}", 1, 3);
    /// 
    ///         // functions with "Must" will always log
    ///         // DO NOT call MustLog at high frequency!
    ///         this.MustLog();
    ///         // and other fuctions like above...
    /// 
    ///         this.MoreLogWarning();
    ///         // and other fuctions like above...
    /// 
    ///         this.MoreLogError();
    ///         // and other fuctions like above...
    ///     }
    /// 
    /// 
    ///     public static void StaticFunction()
    ///     {
    ///         // functions with "More" will log only when conditional "MoreDebug" is predefined
    ///         MoreDebug.MoreStaticLog<YourClass>();
    ///         MoreDebug.MoreStaticLog<YourClass>("message");
    ///         MoreDebug.MoreStaticLog<YourClass>("m1", "m2", "m3");
    ///         MoreDebug.MoreStaticLogFormat<YourClass>("value={0}, value2={1}", 1, 3);
    /// 
    ///         // functions with "Must" will always log
    ///         // DO NOT call MustLog at high frequency!
    ///         MoreDebug.MustStaticLog<YourClass>();
    ///         // and other fuctions like above...
    /// 
    ///         MoreDebug.MoreStaticLogWarning<YourClass>();
    ///         // and other fuctions like above...
    /// 
    ///         MoreDebug.MoreStaticLogError<YourClass>();
    ///         // and other fuctions like above...
    ///     }
    /// }
    /// </code>
    /// </summary>
    public static partial class MoreDebug 
    {

        private static MoreLogLevel ms_moreLogLevel = MoreLogLevel.Log;
        public static MoreLogLevel moreLogLevel
        {
            set{ ms_moreLogLevel = value; }
            get{ return ms_moreLogLevel; }
        }

        private static bool ms_enableStack = true;
        public static bool enableStack { set { ms_enableStack = value; } get{ return ms_enableStack; } }
        
        private static bool ms_enableDateTime = true;
        public static bool enableDateTime { set; get; }

        private static bool ms_enableRuntimeFilter = false;
        private static MoreDebugRuntimeFilter ms_runtimeFilterObject;

        private static bool CheckMoreLogLevel(MoreLogLevel level)
        {
            bool ret = false;
            if((int)level >= (int)moreLogLevel)
            {
                ret = true;
            }

            return ret;
        }


        private static StringBuilder ms_sbLog = new StringBuilder();
        private const string Dot = ".";
        private const string Parentheses = "()";
        private const string CommaSpace = ", ";
        private const string Null = "null";

        public static void Initialize(bool enableRuntimeFilter)
        {
            ms_enableRuntimeFilter = enableRuntimeFilter;

            if(ms_enableRuntimeFilter)
            {
                if(null == ms_runtimeFilterObject)
                {
                    GameObject go = new GameObject();
                    go.name = "MoreDebugRuntimeFilter";
                    GameObject.DontDestroyOnLoad(go);
                    ms_runtimeFilterObject = go.AddComponent<MoreDebugRuntimeFilter>();
                }
            }
        }

        /// <summary>
        /// Get formatted message with additional info: type info, method info and date time info.
        /// </summary>
        /// <returns>The more log.</returns>
        /// <param name="caller">Caller.</param>
        /// <param name="stackFrame">Stack frame.</param>
        /// <param name="messages">Messages.</param>
        public static string GetMoreLog(object caller, int stackFrame, params object[] messages)
        {
            if(0 < ms_sbLog.Length)
            {
                ms_sbLog.Length = 0;
            }

            if(null != messages && 0 < messages.Length)
            {
                object oneMessage;
                for(int i = 0; i < messages.Length - 1; ++i)
                {
                    oneMessage = messages[i];
                    if(null != oneMessage)
                    {
                        ms_sbLog.Append(oneMessage);
                    }
                    else
                    {
                        ms_sbLog.Append(Null);
                    }
                    ms_sbLog.Append(CommaSpace);
                }

                /// add the last one without CommaSpace at the end
                oneMessage = messages[messages.Length - 1];
                if(null != oneMessage)
                {
                    ms_sbLog.Append(oneMessage);
                }
                else
                {
                    ms_sbLog.Append(Null);
                }
            }

            return GetMoreLog(caller, stackFrame + 1, ms_sbLog.ToString());
        }

        private static string GetMoreLog(object caller, int stackFrame, object message)
        {
            if(ms_enableRuntimeFilter)
            {
                if(null != ms_runtimeFilterObject && ms_runtimeFilterObject.Check(caller.GetType()) == false)
                {
                    return null;
                }

            }


            if(0 < ms_sbLog.Length)
            {
                ms_sbLog.Length = 0;
            }
            if(null != message)
            {
                string strMessage = message.ToString();
                if(false == string.IsNullOrEmpty(strMessage))
                {
                    ms_sbLog.AppendLine(strMessage);
                }
            }


            ms_sbLog.Append(caller);

            if(ms_enableStack)
            {
                string methodName = null;

                StackTrace st = null;
                StackFrame sf = null;
                st = new StackTrace(true);
                if(null != st && st.FrameCount >= stackFrame)
                {
                    sf = st.GetFrame(stackFrame);
                    if(null != sf)
                    {
                        MethodBase mb = sf.GetMethod();
                        if(null != mb)
                        {
                            methodName = mb.Name;
                        }
                    }
                }

                ms_sbLog.Append(Dot);
                ms_sbLog.Append(methodName);
                ms_sbLog.AppendLine(Parentheses);
            }

            if(ms_enableDateTime)
            {
                ms_sbLog.Append(GetDateTimeInfo());
            }

            return ms_sbLog.ToString();
        }

        private const string DateTimeFormat = "o";
        private static string GetDateTimeInfo()
        {
            DateTime time = DateTime.Now;
            return time.ToString(DateTimeFormat);
        }
    }



    public static partial class MoreDebug 
    {
        private static void MoreLogRaw(MoreLogLevel logLevel, bool mustLog, object caller)
        {
            MoreLogRaw(logLevel, mustLog, GetMoreLog(caller, 3, null));
        }

        private static void MoreLogRaw(MoreLogLevel logLevel, bool mustLog, object caller, object message)
        {
            MoreLogRaw(logLevel, mustLog, GetMoreLog(caller, 3, message));
        }

        private static void MoreLogRaw(MoreLogLevel logLevel, bool mustLog, object caller, params object[] message)
        {
            MoreLogRaw(logLevel, mustLog, GetMoreLog(caller, 3, message));
        }

        private static void MoreLogRawFormat(MoreLogLevel logLevel, bool mustLog, object caller, string format, params object[] args)
        {
            MoreLogRaw(logLevel, mustLog, GetMoreLog(caller, 3, string.Format(format, args)));
        }

        private static void MoreLogRaw(MoreLogLevel logLevel, bool mustLog, string s)
        {
            if(mustLog || CheckMoreLogLevel(logLevel))
            {
                if(s != null)
                {
                    switch (logLevel)
                    {
                        case MoreLogLevel.Info:
                        case MoreLogLevel.Log:
                            UnityEngine.Debug.Log(s);
                            break;
                        case MoreLogLevel.Warning:
                            UnityEngine.Debug.LogWarning(s);
                            break;
                        case MoreLogLevel.Error:
                        case MoreLogLevel.Assert:
                            UnityEngine.Debug.LogError(s);
                            break;
                        default:
                            break;
                    }

                }
            }
        }


        #region Info
        [Conditional("MoreDebug")]
        public static void MoreLogInfo(this object caller) { MoreLogRaw(MoreLogLevel.Info, false, caller); }
        [Conditional("MoreDebug")]
        public static void MoreStaticLogInfo<T>() { MoreLogRaw(MoreLogLevel.Info, false, typeof(T)); }
        [Conditional("MoreDebug")]
        public static void MoreLogInfo(this object caller, object message) { MoreLogRaw(MoreLogLevel.Info, false, caller, message); }
        [Conditional("MoreDebug")]
        public static void MoreStaticLogInfo<T>(object message) { MoreLogRaw(MoreLogLevel.Info, false, typeof(T), message); }
        [Conditional("MoreDebug")]
        public static void MoreLogInfo(this object caller, params object[] message) { MoreLogRaw(MoreLogLevel.Info, false, caller, message); }
        [Conditional("MoreDebug")]
        public static void MoreLogInfoFormat(this object caller, string format, params object[] args) { MoreLogRawFormat(MoreLogLevel.Info, false, caller, format, args); }
        [Conditional("MoreDebug")]
        public static void MoreStaticLogInfo<T>(params object[]  message) { MoreLogRaw(MoreLogLevel.Info, false, typeof(T), message); }
        [Conditional("MoreDebug")]
        public static void MoreStaticLogInfoFormat<T>(string format, params object[] args){ MoreLogRawFormat(MoreLogLevel.Info, false, typeof(T), format, args); } 
        #endregion


        #region Log
        [Conditional("MoreDebug")]
        public static void MoreLog(this object caller) { MoreLogRaw(MoreLogLevel.Log, false, caller); }
        public static void MustLog(this object caller) { MoreLogRaw(MoreLogLevel.Log, true, caller); }
        [Conditional("MoreDebug")]
        public static void MoreStaticLog<T>() { MoreLogRaw(MoreLogLevel.Log, false, typeof(T)); }
        public static void MustStaticLog<T>() { MoreLogRaw(MoreLogLevel.Log, true, typeof(T)); }
        [Conditional("MoreDebug")]
        public static void MoreLog(this object caller, object message) { MoreLogRaw(MoreLogLevel.Log, false, caller, message); }
        public static void MustLog(this object caller, object message) { MoreLogRaw(MoreLogLevel.Log, true, caller, message); }
        [Conditional("MoreDebug")]
        public static void MoreStaticLog<T>(object message) { MoreLogRaw(MoreLogLevel.Log, false, typeof(T), message); }
        public static void MustStaticLog<T>(object message) { MoreLogRaw(MoreLogLevel.Log, true, typeof(T), message); }
        [Conditional("MoreDebug")]
        public static void MoreLog(this object caller, params object[] message) { MoreLogRaw(MoreLogLevel.Log, false, caller, message); }
        public static void MustLog(this object caller, params object[] message) { MoreLogRaw(MoreLogLevel.Log, true, caller, message); }
        [Conditional("MoreDebug")]
        public static void MoreLogFormat(this object caller, string format, params object[] args) { MoreLogRawFormat(MoreLogLevel.Log, false, caller, format, args); } 
        public static void MustLogFormat(this object caller, string format, params object[] args) { MoreLogRawFormat(MoreLogLevel.Log, true, caller, format, args); } 
        [Conditional("MoreDebug")]
        public static void MoreStaticLog<T>(params object[]  message) { MoreLogRaw(MoreLogLevel.Log, false, typeof(T), message); }
        public static void MustStaticLog<T>(params object[]  message) { MoreLogRaw(MoreLogLevel.Log, true, typeof(T), message); }
        [Conditional("MoreDebug")]
        public static void MoreStaticLogFormat<T>(string format, params object[] args) { MoreLogRawFormat(MoreLogLevel.Log, false, typeof(T), format, args); } 
        public static void MustStaticLogFormat<T>(string format, params object[] args) { MoreLogRawFormat(MoreLogLevel.Log, true, typeof(T), format, args); } 
        #endregion

        
        #region Warning
        [Conditional("MoreDebug")]
        public static void MoreLogWarning(this object caller) { MoreLogRaw(MoreLogLevel.Warning, false, caller); }
        public static void MustLogWarning(this object caller) { MoreLogRaw(MoreLogLevel.Warning, true, caller); }
        [Conditional("MoreDebug")]
        public static void MoreStaticLogWarning<T>() { MoreLogRaw(MoreLogLevel.Warning, false, typeof(T)); }
        public static void MustStaticLogWarning<T>() { MoreLogRaw(MoreLogLevel.Warning, true, typeof(T)); }
        [Conditional("MoreDebug")]
        public static void MoreLogWarning(this object caller, object message) { MoreLogRaw(MoreLogLevel.Warning, false, caller, message); }
        public static void MustLogWarning(this object caller, object message) { MoreLogRaw(MoreLogLevel.Warning, true, caller, message); }
        [Conditional("MoreDebug")]
        public static void MoreStaticLogWarning<T>(object message) { MoreLogRaw(MoreLogLevel.Warning, false, typeof(T), message); }
        public static void MustStaticLogWarning<T>(object message) { MoreLogRaw(MoreLogLevel.Warning, true, typeof(T), message); }
        [Conditional("MoreDebug")]
        public static void MoreLogWarning(this object caller, params object[] message) { MoreLogRaw(MoreLogLevel.Warning, false, caller, message); }
        public static void MustLogWarning(this object caller, params object[] message) { MoreLogRaw(MoreLogLevel.Warning, true, caller, message); }
        [Conditional("MoreDebug")]
        public static void MoreLogWarningFormat(this object caller, string format, params object[] args) { MoreLogRawFormat(MoreLogLevel.Warning, false, caller, format, args); } 
        public static void MustLogWarningFormat(this object caller, string format, params object[] args) { MoreLogRawFormat(MoreLogLevel.Warning, true, caller, format, args); } 
        [Conditional("MoreDebug")]
        public static void MoreStaticLogWarning<T>(params object[]  message) { MoreLogRaw(MoreLogLevel.Warning, false, typeof(T), message); }
        public static void MustStaticLogWarning<T>(params object[]  message) { MoreLogRaw(MoreLogLevel.Warning, true, typeof(T), message); }
        [Conditional("MoreDebug")]
        public static void MoreStaticLogWarningFormat<T>(string format, params object[] args) { MoreLogRawFormat(MoreLogLevel.Warning, false, typeof(T), format, args); } 
        public static void MustStaticLogWarningFormat<T>(string format, params object[] args) { MoreLogRawFormat(MoreLogLevel.Warning, true, typeof(T), format, args); } 
        #endregion
        
        #region LogError
        [Conditional("MoreDebug")]
        public static void MoreLogError(this object caller) { MoreLogRaw(MoreLogLevel.Error, false, caller); }
        public static void MustLogError(this object caller) { MoreLogRaw(MoreLogLevel.Error, true, caller); }
        [Conditional("MoreDebug")]
        public static void MoreStaticLogError<T>() { MoreLogRaw(MoreLogLevel.Error, false, typeof(T)); }
        public static void MustStaticLogError<T>() { MoreLogRaw(MoreLogLevel.Error, true, typeof(T)); }
        [Conditional("MoreDebug")]
        public static void MoreLogError(this object caller, object message) { MoreLogRaw(MoreLogLevel.Error, false, caller, message); }
        public static void MustLogError(this object caller, object message) { MoreLogRaw(MoreLogLevel.Error, true, caller, message); }
        [Conditional("MoreDebug")]
        public static void MoreStaticLogError<T>(object message) { MoreLogRaw(MoreLogLevel.Error, false, typeof(T), message); }
        public static void MustStaticLogError<T>(object message) { MoreLogRaw(MoreLogLevel.Error, true, typeof(T), message); }
        [Conditional("MoreDebug")]
        public static void MoreLogError(this object caller, params object[] message) { MoreLogRaw(MoreLogLevel.Error, false, caller, message); }
        public static void MustLogError(this object caller, params object[] message) { MoreLogRaw(MoreLogLevel.Error, true, caller, message); }
        [Conditional("MoreDebug")]
        public static void MoreLogErrorFormat(this object caller, string format, params object[] args) { MoreLogRawFormat(MoreLogLevel.Error, false, caller, format, args); } 
        public static void MustLogErrorFormat(this object caller, string format, params object[] args) { MoreLogRawFormat(MoreLogLevel.Error, true, caller, format, args); } 
        [Conditional("MoreDebug")]
        public static void MoreStaticLogError<T>(params object[]  message) { MoreLogRaw(MoreLogLevel.Error, false, typeof(T), message); }
        public static void MustStaticLogError<T>(params object[]  message) { MoreLogRaw(MoreLogLevel.Error, true, typeof(T), message); }
        [Conditional("MoreDebug")]
        public static void MoreStaticLogErrorFormat<T>(string format, params object[] args) { MoreLogRawFormat(MoreLogLevel.Error, false, typeof(T), format, args); } 
        public static void MustStaticLogErrorFormat<T>(string format, params object[] args) { MoreLogRawFormat(MoreLogLevel.Error, true, typeof(T), format, args); } 
        #endregion
    }




    public static partial class MoreDebug 
    {
        private const string AssertionFailed = "Assertion failed!";

        #region Assert extension method
        [Conditional("MoreDebug")]
        public static void MoreAssertAreEqual(this object caller, object expected, object actual)
        {
            if(!expected.Equals(actual))
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreEqual(this object caller, object expected, object actual, string message)
        {
            if(!expected.Equals(actual))
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, message);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreNotEqual(this object caller, object expected, object actual)
        {
            if(expected.Equals(actual))
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreNotEqual(this object caller, object expected, object actual, string message)
        {
            if(expected.Equals(actual))
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, message);
            }
        }



        [Conditional("MoreDebug")]
        public static void MoreAssertIsFalse(this object caller, bool condition)
        {
            if(condition)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertIsFalse(this object caller, bool condition, string message)
        {
            if(condition)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, message);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertIsTrue(this object caller, bool condition)
        {
            if(!condition)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertIsTrue(this object caller, bool condition, string message)
        {
            if(!condition)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, message);
            }
        }





        [Conditional("MoreDebug")]
        public static void MoreAssertIsNotNull(this object caller, object value)
        {
            if(null == value)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertIsNotNull(this object caller, object value, string message)
        {
            if(null == value)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, message);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertIsNull(this object caller, object value)
        {
            if(null != value)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertIsNull(this object caller, object value, string message)
        {
            if(null != value)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, message);
            }
        }


        [Conditional("MoreDebug")]
        public static void MoreAssertAreApproximatelyEqual(this object caller, float expected, float actual)
        {
            if(Mathf.Abs(expected - actual) > 0.00001f)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreApproximatelyEqual(this object caller, float expected, float actual, string message)
        {
            if(Mathf.Abs(expected - actual) > 0.00001f)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, message);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreApproximatelyEqual(this object caller, float expected, float actual, float tolerance, string message)
        {
            if(Mathf.Abs(expected - actual) > tolerance)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, message);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreApproximatelyEqual(this object caller, float expected, float actual, float tolerance)
        {
            if(Mathf.Abs(expected - actual) > tolerance)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, AssertionFailed);
            }
        }


        [Conditional("MoreDebug")]
        public static void MoreAssertAreNotApproximatelyEqual(this object caller, float expected, float actual)
        {
            if(Mathf.Abs(expected - actual) < 0.00001f)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreNotApproximatelyEqual(this object caller, float expected, float actual, string message)
        {
            if(Mathf.Abs(expected - actual) < 0.00001f)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, message);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreNotApproximatelyEqual(this object caller, float expected, float actual, float tolerance, string message)
        {
            if(Mathf.Abs(expected - actual) < tolerance)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, message);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreNotApproximatelyEqual(this object caller, float expected, float actual, float tolerance)
        {
            if(Mathf.Abs(expected - actual) < tolerance)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, caller, AssertionFailed);
            }
        }
        #endregion


        #region Assert static method
        [Conditional("MoreDebug")]
        public static void MoreAssertAreEqual<T>(object expected, object actual)
        {
            if(!expected.Equals(actual))
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreEqual<T>(object expected, object actual, string message)
        {
            if(!expected.Equals(actual))
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), message);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreNotEqual<T>(object expected, object actual)
        {
            if(expected.Equals(actual))
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreNotEqual<T>(object expected, object actual, string message)
        {
            if(expected.Equals(actual))
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), message);
            }
        }



        [Conditional("MoreDebug")]
        public static void MoreAssertIsFalse<T>(bool condition)
        {
            if(condition)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertIsFalse<T>(bool condition, string message)
        {
            if(condition)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), message);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertIsTrue<T>(bool condition)
        {
            if(!condition)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertIsTrue<T>(bool condition, string message)
        {
            if(!condition)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), message);
            }
        }





        [Conditional("MoreDebug")]
        public static void MoreAssertIsNotNull<T>(object value)
        {
            if(null == value)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertIsNotNull<T>(object value, string message)
        {
            if(null == value)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), message);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertIsNull<T>(object value)
        {
            if(null != value)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertIsNull<T>(object value, string message)
        {
            if(null != value)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), message);
            }
        }


        [Conditional("MoreDebug")]
        public static void MoreAssertAreApproximatelyEqual<T>(float expected, float actual)
        {
            if(Mathf.Abs(expected - actual) > 0.00001f)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreApproximatelyEqual<T>(float expected, float actual, string message)
        {
            if(Mathf.Abs(expected - actual) > 0.00001f)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), message);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreApproximatelyEqual<T>(float expected, float actual, float tolerance, string message)
        {
            if(Mathf.Abs(expected - actual) > tolerance)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), message);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreApproximatelyEqual<T>(float expected, float actual, float tolerance)
        {
            if(Mathf.Abs(expected - actual) > tolerance)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), AssertionFailed);
            }
        }


        [Conditional("MoreDebug")]
        public static void MoreAssertAreNotApproximatelyEqual<T>(float expected, float actual)
        {
            if(Mathf.Abs(expected - actual) < 0.00001f)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), AssertionFailed);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreNotApproximatelyEqual<T>(float expected, float actual, string message)
        {
            if(Mathf.Abs(expected - actual) < 0.00001f)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), message);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreNotApproximatelyEqual<T>(float expected, float actual, float tolerance, string message)
        {
            if(Mathf.Abs(expected - actual) < tolerance)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), message);
            }
        }
        [Conditional("MoreDebug")]
        public static void MoreAssertAreNotApproximatelyEqual<T>(float expected, float actual, float tolerance)
        {
            if(Mathf.Abs(expected - actual) < tolerance)
            {
                MoreLogRaw(MoreLogLevel.Assert, false, typeof(T), AssertionFailed);
            }
        }
        #endregion
    }

}

