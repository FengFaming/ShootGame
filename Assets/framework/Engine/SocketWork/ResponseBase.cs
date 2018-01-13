/*
 *  create file time:1/6/2018 12:26:57 PM
 *  Describe:消息回馈基类
* */

using System;
using System.Collections.Generic;

namespace Framework.Engine.NetWork
{
    public class ResponseBase
    {
        protected string m_ProtocolTitle;
        public string ProtocolTitle { get { return m_ProtocolTitle; } }

        protected string[] m_ResponesMessage;

        public ResponseBase(string title)
        {
            m_ProtocolTitle = title;

            if (ResponseManager.Instance.IsSuccess())
            {
                ResponseManager.Instance.AddResponse(this);
            }
        }

        public void PressMessage(string message)
        {
            string ms = string.Format("{0} {1}", m_ProtocolTitle, message);
            NetSocketManager.Instance.UpLoadingMessage(ms);
        }

        public virtual void ResponseMessage(string[] messages)
        {
            m_ResponesMessage = messages;
        }

        public virtual void CloseResponse()
        {

        }
    }
}
