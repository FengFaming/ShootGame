/*
 * Creator:ffm
 * Desc:love界面
 * Time:2019/12/28 14:59:28
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;
using XLua;
using UnityEngine.UI;

public class UIPnlLove : UIModelLuaControl
{
	public UIPnlLove() : base()
	{
		m_ModelObjectPath = "UIPnlLove";
		m_IsOnlyOne = true;
		m_LuaName = "MyTextLua";
	}

	public override void OpenSelf(GameObject target)
	{
		base.OpenSelf(target);
		Button t = target.transform.Find("Image").gameObject.GetComponent<Button>();
		Action action = m_UILuaTable.Get<Action>("click");
		t.onClick.AddListener(() => { action(); });
	}
}
