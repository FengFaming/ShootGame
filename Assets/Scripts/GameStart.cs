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
	public Vector2Int m_AddMessage;
	public Vector2Int m_RemoveMessage;
	public Vector2Int m_SendMessge;

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

		if (Input.GetKeyUp(KeyCode.Q))
		{
			for (int index = m_AddMessage.x; index < m_AddMessage.y; index++)
			{
				MessageManger.Instance.AddMessageListener(index.ToString(), this.gameObject, Ms);
			}
		}

		if (Input.GetKeyUp(KeyCode.W))
		{
			for (int index = m_RemoveMessage.x; index < m_RemoveMessage.y; index++)
			{
				MessageManger.Instance.RemoveMessageListener(index.ToString(), this.gameObject);
			}
		}

		if (Input.GetKeyUp(KeyCode.E))
		{
			MessageManger.Instance.SendMessage(m_SendMessge.x.ToString(), m_SendMessge.y);
		}

		if (Input.GetKeyUp(KeyCode.R))
		{
			MessageManger.Instance.RemoveMessageListener(this.gameObject);
			MessageManger.Instance.RearrangeMessage();
		}

		if (Input.GetKeyUp(KeyCode.T))
		{
			MessageManger.Instance.AddMessageListener(90.ToString(), Ms);
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
