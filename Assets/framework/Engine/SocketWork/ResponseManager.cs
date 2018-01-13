/*
 *  create file time:1/6/2018 12:35:12 PM
 *  Describe:消息管理类
* */

using System;
using System.Collections.Generic;
using Framework.Engine;

namespace Framework.Engine.NetWork
{
    public class ResponseManager : BaseMonoSingleton<ResponseManager>
    {
        private Dictionary<string, ResponseBase> m_ResponseDic;

        public override bool Initilize()
        {
            if (!base.Initilize())
                return false;

            m_ResponseDic = new Dictionary<string, ResponseBase>();
            m_ResponseDic.Clear();

            return true;
        }

        public bool IsSuccess()
        {
            return Instance != null;
        }

        public void AddResponse(ResponseBase rs)
        {
            if (!m_ResponseDic.ContainsKey(rs.ProtocolTitle))
            {
                m_ResponseDic.Add(rs.ProtocolTitle, rs);
            }
        }

        public void RemoveResponse(string procotol)
        {
            if (m_ResponseDic.ContainsKey(procotol))
            {
                m_ResponseDic[procotol].CloseResponse();
                m_ResponseDic.Remove(procotol);
            }
        }

        public void BroctMessage(string[] message)
        {
            string title = message[0];
            if (m_ResponseDic.ContainsKey(title))
            {
                string[] targetMessage = null;
                if (message.Length > 1)
                {
                    targetMessage = new string[message.Length - 1];
                    Array.Copy(message, 1, targetMessage, 0, message.Length - 1);
                }

                m_ResponseDic[title].ResponseMessage(targetMessage);
            }
        }

        public T GetResponse<T>(string procotol) where T : ResponseBase
        {
            if (m_ResponseDic.ContainsKey(procotol))
            {
                return m_ResponseDic[procotol] as T;
            }

            return null;
        }
    }
}
