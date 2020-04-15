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
		MessageManger.Instance.AddMessageListener(EngineMessageHead.CHANGE_SCENE_MESSAGE,
						this.gameObject, OpenChangeScene);

		StartCoroutine("StartGame");
	}

	private void OpenChangeScene(params object[] arms)
	{
		if ((bool)arms[0])
		{
			UIManager.Instance.OpenUI("UIPnlFirstPanle", UILayer.Blk);
		}
		else
		{
			UIManager.Instance.RecoveryUIModel("UIPnlFirstPanle", UILayer.Blk);
		}
	}

	private IEnumerator StartGame()
	{
		yield return null;
		UIManager.Instance.OpenUI("UIPnlFirstPanle", UILayer.Pnl);
		yield return new WaitForSeconds(0.5f);

		UIManager.Instance.OpenUI("UIPnlGameStart", UILayer.Pnl);
	}

	private void Update()
	{
		//if (Input.GetKeyUp(KeyCode.A))
		//{
		//	UIManager.Instance.OpenUI("UIPnlFirstPanle", UILayer.Pnl);
		//}

		//if (Input.GetKeyUp(KeyCode.A))
		//{
		//	ResObjectCallBackBase cb = new ResObjectCallBackBase();
		//	cb.m_LoadType = ResObjectType.Configuration;
		//	cb.m_FinshFunction = Finsh;
		//	ResObjectManager.Instance.LoadObject("110Move", ResObjectType.Configuration, cb);
		//}

		//if (Input.GetKeyUp(KeyCode.A))
		//{
		//	ResObjectCallBackBase cb = new ResObjectCallBackBase();
		//	cb.m_LoadType = ResObjectType.GameObject;
		//	cb.m_FinshFunction = Finsh;

		//	ResObjectManager.Instance.LoadObject("c1", ResObjectType.GameObject, cb);
		//}

		//if (Input.GetKeyUp(KeyCode.B))
		//{
		//	Debug.Log(EngineTools.Instance.CheckIDCard(m_IDCard));
		//	Debug.Log(EngineTools.Instance.CheckIDCardSex(m_IDCard));
		//}

		if (Input.GetKeyUp(KeyCode.O))
		{
			for (int index = 0; index < m_UINames.Count; index++)
			{
				UIManager.Instance.OpenUI(m_UINames[index], m_Layer);
			}
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			if (Input.GetKeyUp(KeyCode.I))
			{
				ObjectPoolManager.Instance.InitPool("TEST", "TestObjectPool");
			}

			if (Input.GetKeyUp(KeyCode.A))
			{
				ObjectPoolControl oc = new ObjectPoolControl();
				ObjectPoolManager.Instance.AddObject("TEST", "CES", oc);
			}

			if (Input.GetKeyUp(KeyCode.R))
			{
				ObjectPoolManager.Instance.RemoveObject("TEST", "CES");
			}

			if (Input.GetKeyUp(KeyCode.B))
			{
				ObjectPoolControl oc = new ObjectPoolControl();
				ObjectPoolManager.Instance.RecoveryObject("TEST", "CES", oc);
			}

			if (Input.GetKeyUp(KeyCode.G))
			{
				ObjectPoolControl oc = ObjectPoolManager.Instance.GetCloneObject("TEST", "CES");
				Debug.Log(oc);
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
		Debug.Log(o);
	}
}
