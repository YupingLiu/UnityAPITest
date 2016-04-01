using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

namespace MoreFun
{
    [CustomEditor(typeof(MoreDebugRuntimeFilter))]
    public class MoreDebugRuntimeFilterEditor : Editor
    {
        public struct CacheItem
        {
            public Type itemType;
            public bool itemEnabled;
            
            public CacheItem(Type itemType, bool itemEnabled)
            {
                this.itemType = itemType;
                this.itemEnabled = itemEnabled;
            }
        }

        private List<CacheItem> m_cacheData = new List<CacheItem>();
        private bool m_caseSensitive;
        private string m_oldSearchValue;
        private string m_searchValue;

        private int m_currStartIndex = 0;
        private int itemPerPage = 10;

        public override void OnInspectorGUI()
        {
            MoreDebugRuntimeFilter myTarget = (MoreDebugRuntimeFilter)target;

            Dictionary<Type ,bool> data = myTarget.GetData();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Search class:");
            bool searchValueChanged = false;
            m_searchValue = EditorGUILayout.TextField(m_searchValue);
            if(m_searchValue != m_oldSearchValue)
            {
                searchValueChanged = true;
                m_oldSearchValue = m_searchValue;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Search case sensitive: ");
            m_caseSensitive = EditorGUILayout.Toggle(m_caseSensitive);
            EditorGUILayout.EndHorizontal();

            
            EditorGUILayout.BeginHorizontal();
            bool prevPage = GUILayout.Button("< prev");
            bool nextPage = GUILayout.Button("next >");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Enable log:");
            bool all = GUILayout.Button("All");
            bool none = GUILayout.Button("None");
            EditorGUILayout.EndHorizontal();

            if(null != data && 0 < data.Count)
            {

                string searchValue = m_searchValue;

                if(null != searchValue)
                {
                    if(false == m_caseSensitive)
                    {
                        searchValue = searchValue.ToLower();
                    }
                }

                if(false == searchValueChanged)
                {
                    if(prevPage)
                    {
                        m_currStartIndex -= itemPerPage;
                    }
                    if(nextPage)
                    {
                        m_currStartIndex += itemPerPage;
                    }
                }
                else
                {
                    m_currStartIndex = 0;
                }
                m_currStartIndex = Mathf.Clamp(m_currStartIndex, 0, data.Count - 1);

                
                m_cacheData.Clear();

                EditorGUILayout.BeginVertical();
                int iterateIndex = 0;
                foreach(KeyValuePair<Type, bool> onePair in data)
                {
                    Type dataKey = onePair.Key;
                    bool dataValue = onePair.Value;
                    
                    string keyName = dataKey.ToString();
                    if(false == m_caseSensitive)
                    {
                        keyName = keyName.ToLower();
                    }

                    if(searchValue == null || searchValue.Length == 0 ||
                       keyName.Contains(searchValue))
                    {
                        if(iterateIndex < m_currStartIndex + itemPerPage)
                        {
                            if(iterateIndex >= m_currStartIndex)
                            {
                                EditorGUILayout.BeginHorizontal();
                                bool oldValue = EditorGUILayout.Toggle(dataValue);
                                bool newValue = (oldValue || all) && !none;
                                m_cacheData.Add(new CacheItem(dataKey, newValue));
                                EditorGUILayout.LabelField(dataKey.ToString());
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                        else
                        {
                            break;
                        }

                        ++iterateIndex;
                    }
                }
                EditorGUILayout.EndVertical();

                for(int i = 0; i < m_cacheData.Count; ++i)
                {
                    if(m_cacheData[i].itemEnabled != myTarget.Check(m_cacheData[i].itemType))
                    {
                        myTarget.Set(m_cacheData[i].itemType, m_cacheData[i].itemEnabled);
                    }
                }
            }

        }
    }
}

