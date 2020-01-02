/*
 * Creator:ffm
 * Desc:love界面
 * Time:2019/12/28 15:32:31
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;

public class Love : IUIModelControl
{
	public Love() : base()
	{
		m_ModelObjectPath = "UIPnlLove";
		m_IsOnlyOne = false;
	}

	public override List<string> GetLinksUI()
	{
		List<string> links = base.GetLinksUI();
		links.Add("UIPnlLove");
		return links;
	}
}
