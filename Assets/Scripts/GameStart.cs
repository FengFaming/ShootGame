/*
 * Creator:ffm
 * Desc:项目启动类
 * Time:2019/11/11 15:25:55
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;

public class GameStart : ObjectBase
{
	private void Start()
	{
		ResObjectManager.Instance.InitResManager("AB");
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.A))
		{
			ResObjectCallBackBase cb = new ResObjectCallBackBase();
			cb.m_LoadType = ResObjectType.GameObject;
			cb.m_FinshFunction = Finsh;

			ResObjectManager.Instance.LoadObject("c1", ResObjectType.GameObject, cb);
		}
	}

	private void Finsh(object o)
	{

	}
}
