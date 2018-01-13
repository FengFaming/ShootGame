/*
 *  create file time:12/9/2017 3:12:16 PM
 *  Describe:进行消息传递，主要解决模块直接的耦合关系
* */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Engine
{
    public class MessageKey
    {
        public static readonly string SINGLETON_ADD_MANAGER = "SINGLETON_ADD_MANAGER";
        public static readonly string SINGLETON_REMOVE_MANAGER = "SINGLETON_REMOVE_MANAGER";
    }

    public class MessageManager : BaseMonoSingleton<MessageManager>
    {
        public delegate void OnSendMessageCallBack(params object[] arms);
        private Dictionary<string, List<OnSendMessageCallBack>> m_MessageDic;

        public override bool Initilize()
        {
            if (!base.Initilize())
            {
                GameObject go = new GameObject();
                go.transform.parent = null;
                go.transform.position = Vector3.zero;
                go.transform.eulerAngles = Vector3.zero;
                go.transform.localScale = Vector3.one;

                m_Instance = go.AddComponent<MessageManager>();
            }

            m_MessageDic = new Dictionary<string, List<OnSendMessageCallBack>>();
            m_MessageDic.Clear();

            return true;
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="key">消息头</param>
        /// <param name="callback">消息回调</param>
        /// <param name="forcedjoin">是否强制加入，默认不强制加入，用来控制多次添加同样的消息回调</param>
        public void AddMessage(string key, OnSendMessageCallBack callback, bool forcedjoin = false)
        {
            if (m_MessageDic.ContainsKey(key))
            {
                if (m_MessageDic[key] != null)
                {
                    if (m_MessageDic[key].Contains(callback))
                    {
                        Debug.LogWarning(string.Format("the call【key({0})fun({1})】 already existed.", key, callback));
                        if (forcedjoin)
                        {
                            m_MessageDic[key].Add(callback);
                        }
                    }
                    else
                    {
                        m_MessageDic[key].Add(callback);
                    }
                }
                else
                {
                    m_MessageDic[key] = new List<OnSendMessageCallBack>() { callback };
                }
            }
            else
            {
                m_MessageDic.Add(key, new List<OnSendMessageCallBack>() { callback });
            }
        }

        /// <summary>
        /// 移除消息
        /// </summary>
        /// <param name="key"></param>
        public void RemoveMessage(string key)
        {
            if (m_MessageDic.ContainsKey(key))
            {
                m_MessageDic[key].Clear();
                m_MessageDic[key] = null;
                m_MessageDic.Remove(key);
            }
        }

        /// <summary>
        /// 移除回调
        /// </summary>
        /// <param name="callback"></param>
        public void RemoveMessage(OnSendMessageCallBack callback)
        {
            Dictionary<string, List<int>> deletes = new Dictionary<string, List<int>>();
            foreach (var callbacks in m_MessageDic)
            {
                if (callbacks.Value != null)
                {
                    deletes.Add(callbacks.Key, new List<int>());
                    for (int index = 0; index < callbacks.Value.Count; index++)
                    {
                        if (callbacks.Value[index].Equals(callback))
                        {
                            deletes[callbacks.Key].Insert(0, index);
                        }
                    }
                }
            }

            foreach (var item in deletes)
            {
                item.Value.Sort((int a, int b) => { return b - a; });
                for (int index = 0; index < item.Value.Count; index++)
                {
                    m_MessageDic[item.Key].RemoveAt(item.Value[index]);
                }

                if (m_MessageDic[item.Key].Count <= 0)
                {
                    m_MessageDic.Remove(item.Key);
                }
            }
        }

        /// <summary>
        /// 移除某个消息当中的某个回调
        /// </summary>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        public void RemoveMessage(string key, OnSendMessageCallBack callback)
        {
            if (m_MessageDic.ContainsKey(key))
            {
                List<int> deletes = new List<int>();
                if (m_MessageDic[key] != null)
                {
                    if (m_MessageDic[key].Contains(callback))
                    {
                        for (int index = 0; index < m_MessageDic[key].Count; index++)
                        {
                            if (m_MessageDic[key][index].Equals(callback))
                            {
                                deletes.Insert(0, index);
                            }
                        }

                        deletes.Sort((int a, int b) => { return b - a; });
                        for (int index = 0; index < deletes.Count; index++)
                        {
                            m_MessageDic[key].RemoveAt(deletes[index]);
                        }

                        if (m_MessageDic[key].Count <= 0)
                        {
                            m_MessageDic.Remove(key);
                        }
                    }
                }
                else
                {
                    m_MessageDic.Remove(key);
                }
            }
        }

        /// <summary>
        /// 按照标签广播消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="arms"></param>
        public void SendMessage(string key, params object[] arms)
        {
            if (m_MessageDic.ContainsKey(key))
            {
                for (int index = 0; index < m_MessageDic[key].Count; index++)
                {
                    m_MessageDic[key][index](arms);
                }
            }
        }

        /// <summary>
        /// 添加回调的方式广播消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        /// <param name="arms"></param>
        public void SendMessage(string key, OnSendMessageCallBack callback, params object[] arms)
        {
            AddMessage(key, callback);
            SendMessage(key, arms);
        }
    }
}
