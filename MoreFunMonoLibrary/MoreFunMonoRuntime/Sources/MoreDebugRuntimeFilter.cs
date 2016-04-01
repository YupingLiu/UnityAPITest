using System;
using UnityEngine;
using System.Collections.Generic;

namespace MoreFun
{
    public class MoreDebugRuntimeFilter : MonoBehaviour
    {
        private const string Pref_Key = "MoreDebugFilter_";


        private Dictionary<Type ,bool> m_dictRuntimeLogFilter = new Dictionary<Type, bool>();

        void Awake()
        {
        }

        public bool Check(Type type)
        {
            if(false == m_dictRuntimeLogFilter.ContainsKey(type))
            {
                bool newTypeEnabled = true;
                if(Application.isEditor)
                {
                    if(PlayerPrefs.HasKey(GetPrefKey(type)))
                    {
                        newTypeEnabled = (PlayerPrefs.GetInt(GetPrefKey(type)) != 0);
                    }
                }
                Set(type, newTypeEnabled);
            }

            return m_dictRuntimeLogFilter[type];
        }

        public Dictionary<Type ,bool> GetData()
        {
            return m_dictRuntimeLogFilter;
        }


        public void Set(Type type, bool value)
        {
            if(Application.isEditor)
            {
                PlayerPrefs.SetInt(GetPrefKey(type), (value ? 1 : 0));
                PlayerPrefs.Save();
            }

            m_dictRuntimeLogFilter[type] = value;
        }

        private string GetPrefKey(Type type)
        {
            return Pref_Key + type.ToString();
        }
    }
}

