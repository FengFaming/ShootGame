/*
 *  create file time:11/5/2017 9:48:25 PM
 *  Describe:项目当中的信号文件
* */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Engine
{
    public class Signal : BaseSignal
    {
        private event Action m_Linstener = delegate { };
        private event Action m_OnceListener = delegate { };

        public void Dispath()
        {
            m_Linstener();
            m_OnceListener();
            m_OnceListener = delegate { };
            base.Dispath(null);
        }

        public override List<Type> GetTypes()
        {
            return new List<Type>();
        }

        public void AddListener(Action callback)
        {
            if (!m_Linstener.GetInvocationList().Contains(callback))
            {
                m_Linstener += callback;
            }
        }

        public void AddOnce(Action callback)
        {
            if (!m_OnceListener.GetInvocationList().Contains(callback))
            {
                m_OnceListener += callback;
            }
        }

        public void RemoveListener(Action callback)
        {
            if (m_Linstener.GetInvocationList().Contains(callback))
            {
                m_Linstener -= callback;
            }
        }
    }

    public class Signal<T> : BaseSignal
    {
        private event Action<T> m_Listener = delegate { };
        private event Action<T> m_OnceListener = delegate { };

        public override List<Type> GetTypes()
        {
            List<Type> types = new List<Type>();
            types.Add(typeof(T));
            return types;
        }

        public void Dispath(T type)
        {
            m_Listener(type);
            m_OnceListener(type);
            m_OnceListener = delegate { };
            object[] outv = { type };
            base.Dispath(outv);
        }

        public void AddListener(Action<T> callback)
        {
            if (!m_Listener.GetInvocationList().Contains(callback))
            {
                m_Listener += callback;
            }
        }

        public void AddOnce(Action<T> callback)
        {
            if (!m_OnceListener.GetInvocationList().Contains(callback))
            {
                m_OnceListener += callback;
            }
        }

        public void RemoveListener(Action<T> callback)
        {
            if (!m_Listener.GetInvocationList().Contains(callback))
            {
                m_Listener -= callback;
            }
        }
    }

    public class Signal<T, U> : BaseSignal
    {
        private event Action<T, U> m_Listener = delegate { };
        private event Action<T, U> m_Once = delegate { };

        public override List<Type> GetTypes()
        {
            List<Type> types = new List<Type>();
            types.Add(typeof(T));
            types.Add(typeof(U));
            return types;
        }

        public void Dispath(T type, U uype)
        {
            m_Listener(type, uype);
            m_Once(type, uype);
            m_Once = delegate { };
            object[] outv = { type, uype };
            base.Dispath(outv);
        }

        public void AddListener(Action<T, U> callback)
        {
            if (!m_Listener.GetInvocationList().Contains(callback))
            {
                m_Listener += callback;
            }
        }

        public void AddOnce(Action<T, U> callback)
        {
            if (!m_Once.GetInvocationList().Contains(callback))
            {
                m_Once += callback;
            }
        }

        public void RemoveListener(Action<T, U> callback)
        {
            if (m_Listener.GetInvocationList().Contains(callback))
            {
                m_Listener -= callback;
            }
        }
    }

    public class Signal<T, U, V> : BaseSignal
    {
        private event Action<T, U, V> m_Listener = delegate { };
        private event Action<T, U, V> m_Once = delegate { };

        public override List<Type> GetTypes()
        {
            List<Type> types = new List<Type>();
            types.Add(typeof(T));
            types.Add(typeof(U));
            types.Add(typeof(V));
            return types;
        }

        public void Dispath(T type, U uype, V vype)
        {
            m_Listener(type, uype, vype);
            m_Once(type, uype, vype);
            m_Once = delegate { };
            object[] outv = { type, uype, vype };
            base.Dispath(outv);
        }

        public void AddListener(Action<T, U, V> callback)
        {
            if (!m_Listener.GetInvocationList().Contains(callback))
            {
                m_Listener += callback;
            }
        }

        public void AddOnce(Action<T, U, V> callback)
        {
            if (!m_Once.GetInvocationList().Contains(callback))
            {
                m_Once += callback;
            }
        }

        public void RemoveListener(Action<T, U, V> callback)
        {
            if (m_Listener.GetInvocationList().Contains(callback))
            {
                m_Listener -= callback;
            }
        }
    }

    public class Signal<T, U, V, W> : BaseSignal
    {
        private event Action<T, U, V, W> m_Listener = delegate { };
        private event Action<T, U, V, W> m_Once = delegate { };

        public override List<Type> GetTypes()
        {
            List<Type> types = new List<Type>();
            types.Add(typeof(T));
            types.Add(typeof(U));
            types.Add(typeof(V));
            types.Add(typeof(W));
            return types;
        }

        public void Dispath(T type, U uype, V vype, W wype)
        {
            m_Listener(type, uype, vype, wype);
            m_Once(type, uype, vype, wype);
            m_Once = delegate { };
            object[] outv = { type, uype, vype, wype };
            base.Dispath(outv);
        }

        public void AddListener(Action<T, U, V, W> callback)
        {
            if (!m_Listener.GetInvocationList().Contains(callback))
            {
                m_Listener += callback;
            }
        }

        public void AddOnce(Action<T, U, V, W> callback)
        {
            if (!m_Once.GetInvocationList().Contains(callback))
            {
                m_Once += callback;
            }
        }

        public void RemoveListener(Action<T, U, V, W> callback)
        {
            if (m_Listener.GetInvocationList().Contains(callback))
            {
                m_Listener -= callback;
            }
        }
    }
}
