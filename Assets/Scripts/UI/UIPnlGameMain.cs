/*
 * Creator:ffm
 * Desc:游戏主界面
 * Time:2020/4/11 9:56:15
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;
using UnityEngine.UI;

public class UIPnlGameMain : IUIModelControl
{
	public UIPnlGameMain() : base()
	{
		m_ModelObjectPath = "UIPnlGameMain";
		m_IsOnlyOne = true;
	}

	public override void OpenSelf(GameObject target)
	{
		base.OpenSelf(target);
		Button animation = m_ControlTarget.gameObject.transform.Find("animation").gameObject.GetComponent<Button>();
		Button shoot = m_ControlTarget.gameObject.transform.Find("shoot").gameObject.GetComponent<Button>();
		animation.onClick.AddListener(new UnityEngine.Events.UnityAction(() => { OnClickAnimation(1); }));
		shoot.onClick.AddListener(new UnityEngine.Events.UnityAction(() => { OnClickAnimation(2); }));
	}

	private void OnClickAnimation(int tage)
	{
		switch (tage)
		{
			case 1:
				GameSceneManager.Instance.ChangeScene(new AnimationScene("animationscene"));
				break;
		}
	}

	public override bool GetCloseOther(ref List<string> others)
	{
		return true;
	}
}
