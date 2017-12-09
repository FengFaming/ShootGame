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
        public static BaseMonoSingleton Instance { get { return m_Instance; } }

        internal void Awake()
        {
            Initilize();
        }

        public virtual bool Initilize()
        {
            if (this.gameObject == null)
            {
                return false;
            }

            m_Instance = this;
            return true;
        }
    }

    public class BaseMonoSingleton<T> : MonoBehaviour, IBaseSingleton where T : MonoBehaviour
    {
        protected static T m_Instance;
        public static T Instance { get { return m_Instance; } }

        internal void Awake()
        {
            Initilize();
        }

        public virtual bool Initilize()
        {
            if (this.gameObject == null)
            {
                return false;
            }

            m_Instance = this as T;
            return true;
        }
    }
}
