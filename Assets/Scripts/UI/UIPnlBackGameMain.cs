/*需要屏蔽的警告*/
#pragma warning disable
/*
 * Creator:ffm
 * Desc:返回游戏主界面
 * Time:2020/4/27 8:57:36
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;
using UnityEngine.UI;

public class UIPnlBackGameMain : IUIModelControl
{
	public UIPnlBackGameMain() : base()
	{
		m_ModelObjectPath = "UIPnlBackGameMain";
		m_IsOnlyOne = true;
	}

	public override void OpenSelf(GameObject target)
	{
		base.OpenSelf(target);
		Button bt = m_ControlTarget.gameObject.transform.Find("Button").gameObject.GetComponent<Button>();
		bt.onClick.AddListener(GoBackGameMain);
	}

	private void GoBackGameMain()
	{
		GameSceneManager.Instance.ChangeScene(new GameMainScene());
	}
}
