/*
 * Creator:ffm
 * Desc:第一个界面
 * Time:2020/4/6 16:00:51
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;

public class UIPnlFirstPanle : IUIModelControl
{
	public UIPnlFirstPanle() : base()
	{
		m_ModelObjectPath = "UIPnlFirstPanle";
		m_IsOnlyOne = true;
	}

	public override void OpenSelf(GameObject target)
	{
		base.OpenSelf(target);
		MessageManger.Instance.AddMessageListener(EngineMessageHead.CHANGE_SCENE_PRESS_VALUE, new IMessageBase(
			m_ControlTarget, false, Listen));
	}

	private void Listen(params object[] arms)
	{
		//Debug.Log(arms[0]);
	}

	public override void CloseSelf(bool manager = false)
	{
		base.CloseSelf(manager);
		MessageManger.Instance.RemoveMessageListener(EngineMessageHead.CHANGE_SCENE_PRESS_VALUE, m_ControlTarget);
	}
}
