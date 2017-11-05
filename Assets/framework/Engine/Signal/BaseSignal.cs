/*
 *  create file time:11/5/2017 8:52:44 PM
 *  Describe:project signal base
* */

using System;
using System.Collections.Generic;

namespace Framework.Engine
{
    public class BaseSignal : IBaseSignal
    {
        /// <summary>
        /// 循环的信号集
        /// </summary>
        private event Action<IBaseSignal, object[]> m_BaseListener = delegate { };

        /// <summary>
        /// 一次性的信号集
        /// </summary>
        private event Action<IBaseSignal, object[]> m_OnceListener = delegate { };

        /// <summary>
        /// 信号调度
        /// </summary>
        /// <param name="args"></param>
        public void Dispath(object[] args)
        {
            m_BaseListener(this, args);
            m_OnceListener(this, args);
            m_OnceListener = delegate { };
        }

        /// <summary>
        /// 增加信号
        /// </summary>
        /// <param name="callback"></param>
        public void AddListener(Action<IBaseSignal, object[]> callback)
        {
            foreach (Delegate del in m_BaseListener.GetInvocationList())
            {
                Action<IBaseSignal, object[]> action = (Action<IBaseSignal, object[]>)del;
                if (callback.Equals(action))
                {
                    return;
                }
            }

            m_BaseListener += callback;
        }

        /// <summary>
        /// 添加一次性信号
        /// </summary>
        /// <param name="callback"></param>
        public void AddOnce(Action<IBaseSignal, object[]> callback)
        {
            foreach (Delegate del in m_OnceListener.GetInvocationList())
            {
                Action<IBaseSignal, object[]> action = (Action<IBaseSignal, object[]>)del;
                if (callback.Equals(action))
                {
                    return;
                }
            }

            m_OnceListener += callback;
        }

        /// <summary>
        /// 移除信号
        /// </summary>
        /// <param name="callback"></param>
        public void RemoveListener(Action<IBaseSignal, object[]> callback)
        {
            bool hasAction = false;
            foreach (Delegate del in m_BaseListener.GetInvocationList())
            {
                Action<IBaseSignal, object[]> action = (Action<IBaseSignal, object[]>)del;
                if (callback.Equals(action))
                {
                    hasAction = true;
                    break;
                }
            }

            if (hasAction)
            {
                m_BaseListener -= callback;
            }
        }

        /// <summary>
        /// 获取信号量
        /// </summary>
        /// <returns></returns>
        public virtual List<Type> GetTypes()
        {
            return new List<Type>();
        }
    }
}
