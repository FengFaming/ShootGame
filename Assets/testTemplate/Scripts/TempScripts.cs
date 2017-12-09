/*
 *  create file time:11/6/2017 10:53:01 AM
 *  Describe:测试场景开始
* */

using System;
using System.Collections.Generic;
using UnityEngine;
using Framework.Engine;

public class TempScripts : BaseMonoSingleton<TempScripts>
{
    private Signal m_Action;
    private Signal<int, float, float, float> m_Action2;

    public override bool Initilize()
    {
        base.Initilize();

        m_Action = new Signal();
        m_Action.Dispath();

        m_Action.AddListener(() => { Debug.Log("action 1"); });
        m_Action.AddListener(() => { Debug.Log("action 2"); });
        m_Action.AddListener(() => { Debug.Log("action 3"); });

        m_Action2 = new Signal<int, float, float, float>();
        m_Action2.AddListener((int a, float b, float c, float d) => { Debug.Log(string.Format("{0} = {1} = {2} = {3}", a, b, c, d)); });
        m_Action2.AddListener((int a, float b, float c, float d) => { Debug.Log(string.Format("{0} = {1} = {2} = {3}", a + 1, b, c, d)); });
        m_Action2.AddListener((int a, float b, float c, float d) => { Debug.Log(string.Format("{0} = {1} = {2} = {3}", a + 2, b, c, d)); });
        m_Action2.AddListener((int a, float b, float c, float d) => { Debug.Log(string.Format("{0} = {1} = {2} = {3}", a + 3, b, c, d)); });

        return true;
    }

    private void Test(params object[] arms)
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_Action.Dispath();

            m_Action2.Dispath(1, 2, 3, 4);
        }
    }
}
