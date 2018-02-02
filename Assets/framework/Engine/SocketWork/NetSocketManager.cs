/*
 *  create file time:1/6/2018 11:44:26 AM
 *  Describe:用来创建连接以及控制连接的
* */

using System;
using System.Collections.Generic;
using Framework.Engine;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using UnityEngine;
using System.Text;

namespace Framework.Engine.NetWork
{
    public enum ClientStage
    {
        /// <summary>
        /// 未知
        /// </summary>
        UnKnown,

        /// <summary>
        /// 连接中
        /// </summary>
        Loading,

        /// <summary>
        /// 已连接
        /// </summary>
        Loaded,

        /// <summary>
        /// 断开
        /// </summary>
        Interrupt
    }

    public class NetSocketManager : BaseMonoSingleton<NetSocketManager>
    {
        private string m_Ipadress;
        private int m_Port;
        private byte[] m_ReceiveData;
        private Socket m_ClientSocket;
        private Thread m_ClientThread;
        private ClientStage m_Stage;

        public override bool Initilize()
        {
            if (!base.Initilize())
                return false;

            m_Ipadress = string.Empty;
            m_Port = 0;
            m_ReceiveData = new byte[2048];
            m_ClientSocket = null;
            m_ClientThread = null;
            m_Stage = ClientStage.UnKnown;

            return true;
        }

        public void StartNetSocket(string ip = "127.0.0.1", int port = 6789)
        {
            m_Ipadress = ip;
            m_Port = port;

            m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                m_Stage = ClientStage.Loading;
                m_ClientSocket.Connect(new IPEndPoint(IPAddress.Parse(m_Ipadress), m_Port));
                m_ClientThread = new Thread(ReceiveInfo);
                m_ClientThread.IsBackground = true;
                m_ClientThread.Start();
                m_Stage = ClientStage.Loaded;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void ReceiveInfo()
        {
            while (true)
            {
                int bytes = m_ClientSocket.Receive(m_ReceiveData);
                string s = Encoding.UTF8.GetString(m_ReceiveData, 0, bytes);
                string[] p = s.Split(' ');
                if (p.Length > 0)
                {
                    ResponseManager.Instance.BroctMessage(p);
                }
            }
        }

        public void UpLoadingMessage(string message)
        {
            byte[] m = Encoding.UTF8.GetBytes(message);
            if (m_Stage == ClientStage.Loaded)
            {
                m_ClientSocket.Send(m);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            CloseClient();
        }

        public void CloseClient()
        {
            Debug.Log("Close");
            if (m_ClientSocket != null)
            {
                m_ClientSocket.Close();
            }

            if (m_ClientThread != null)
            {
                m_ClientThread.Abort();
            }
        }
    }
}