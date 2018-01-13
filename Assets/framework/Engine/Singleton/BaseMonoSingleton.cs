/*
 *  create file time:11/6/2017 10:22:23 AM
 *  Describe:Mono类单例基类
* */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Engine
{
    public class BaseMonoSingleton : MonoBehaviour, IBaseSingleton
    {
        protected static BaseMonoSingleton m_Instance;
        public static BaseMonoSingleton Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    GameObject go = new GameObject();
                    m_Instance = go.gameObject.AddComponent<BaseMonoSingleton>();
                    go.gameObject.transform.parent = null;
                    go.gameObject.transform.position = Vector3.zero;
                    go.gameObject.transform.eulerAngles = Vector3.zero;
                    go.gameObject.transform.localScale = Vector3.one;
                    go.name = go.gameObject.GetComponent<BaseMonoSingleton>().name;
                }

                return m_Instance;
            }
        }

        internal void Awake()
        {
            Initilize();
        }

        public virtual bool Initilize()
        {
            m_Instance = this;
            GameObject.DontDestroyOnLoad(m_Instance.gameObject);
            return true;
        }

        public virtual void OnDestroy()
        {

        }
    }

    public class BaseMonoSingleton<T> : MonoBehaviour, IBaseSingleton where T : MonoBehaviour
    {
        protected static T m_Instance;
        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    GameObject go = new GameObject();
                    m_Instance = go.gameObject.AddComponent<T>();
                    go.gameObject.transform.parent = null;
                    go.gameObject.transform.position = Vector3.zero;
                    go.gameObject.transform.eulerAngles = Vector3.zero;
                    go.gameObject.transform.localScale = Vector3.one;
                    go.name = go.gameObject.GetComponent<T>().name;
                }

                return m_Instance;
            }
        }

        internal void Awake()
        {
            Initilize();
        }

        public virtual bool Initilize()
        {
            m_Instance = this as T;
            GameObject.DontDestroyOnLoad(m_Instance.gameObject);
            return true;
        }

        public virtual void OnDestroy()
        {

        }
    }
}
