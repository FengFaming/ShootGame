/*
 *  create file time:1/6/2018 12:49:08 PM
 *  Describe:登陆回调
* */

using System;
using System.Collections.Generic;
using Framework.Engine.NetWork;
using UnityEngine;

public class LoadResponse : ResponseBase
{
    public LoadResponse(string potocol) : base(potocol) { }

    public override void ResponseMessage(string[] messages)
    {
        base.ResponseMessage(messages);

        string log = string.Empty;
        foreach (string s in m_ResponesMessage)
        {
            log += s + " ";
        }

        Debug.Log(log);
    }
}
