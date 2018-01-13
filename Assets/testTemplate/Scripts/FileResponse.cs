/*
 *  create file time:1/13/2018 10:03:07 AM
 *  Describe:测试文件传送
* */

using System;
using System.Collections.Generic;
using Framework.Engine.NetWork;
using UnityEngine;
using Framework.Engine.Art;

public class FileResponse : ResponseBase
{
    public FileResponse(string potocol) : base(potocol) { }

    public override void ResponseMessage(string[] messages)
    {
        base.ResponseMessage(messages);

        string log = string.Empty;
        foreach (string s in m_ResponesMessage)
        {
            log += s + " ";
        }

        FileThread.Instance.SaveFile(new FileData(m_ResponesMessage[0], m_ResponesMessage[1]));

        Debug.Log(log);
    }
}
