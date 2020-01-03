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
	public string m_IDCard = "44022219910116033X";
	public List<string> m_UINames;
	public UILayer m_Layer;

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

		if (Input.GetKeyUp(KeyCode.B))
		{
			Debug.Log(EngineTools.Instance.CheckIDCard(m_IDCard));
			Debug.Log(EngineTools.Instance.CheckIDCardSex(m_IDCard));
		}

		if (Input.GetKeyUp(KeyCode.O))
		{
			for (int index = 0; index < m_UINames.Count; index++)
			{
				UIManager.Instance.OpenUI(m_UINames[index], m_Layer);
			}
		}
	}

	private void Ms(params object[] arms)
	{
		if (arms != null)
		{
			Debug.Log(arms.Length);
		}
	}

	private void Finsh(object o)
	{

	}
}
