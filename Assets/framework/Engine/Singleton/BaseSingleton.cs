/*
 *  create file time:11/6/2017 10:08:58 AM
 *  Describe:非Mono类单例
* */

using System;
using System.Collections.Generic;

namespace Framework.Engine
{
    public class BaseSingleton : IBaseSingleton
    {
        private static BaseSingleton m_Instance;
        public static BaseSingleton Instance { get { return m_Instance; } }

        public BaseSingleton()
        {
            m_Instance = Activator.CreateInstance<BaseSingleton>();
            Initilize();
        }

        public virtual void Initilize()
        {

        }
    }

    public class BaseSingleton<T> : IBaseSingleton
    {
        private static T m_Instance;
        public static T Instance { get { return m_Instance; } }

        public BaseSingleton()
        {
            m_Instance = Activator.CreateInstance<T>();
            Initilize();
        }

        public BaseSingleton(T t)
        {
            m_Instance = t;
        }

        public virtual void Initilize()
        {

        }
    }
}
