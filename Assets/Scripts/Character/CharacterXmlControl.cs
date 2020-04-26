/*需要屏蔽的警告*/
#pragma warning disable
/*
 * Creator:ffm
 * Desc:角色配置文件
 * Time:2020/4/26 13:51:10
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;
using System.Xml;

public class CharacterXmlControl : XmlBase
{
	public struct StateInfo
	{
		public string m_Control;
		public List<KeyValuePair<string, bool>> m_Paramters;
	}

	public Dictionary<int, StateInfo> m_StateInfos;

	public CharacterXmlControl(string name) : base(name)
	{
		m_StateInfos = new Dictionary<int, StateInfo>();
		m_StateInfos.Clear();
	}

	public override bool LoadXml(XmlElement node)
	{
		if (!base.LoadXml(node))
			return false;

		foreach (XmlElement item in node.ChildNodes)
		{
			if (item.Name == "States")
			{
				foreach (XmlElement s in item.ChildNodes)
				{
					int id = int.Parse(s.GetAttribute("ID"));
					string control = s.GetAttribute("Control");
					StateInfo info = new StateInfo();
					info.m_Control = control;
					info.m_Paramters = new List<KeyValuePair<string, bool>>();
					foreach (XmlElement p in s.ChildNodes)
					{
						string key = p.GetAttribute("Key");
						bool value = bool.Parse(p.GetAttribute("Value"));
						info.m_Paramters.Add(new KeyValuePair<string, bool>(key, value));
					}

					m_StateInfos.Add(id, info);
				}
			}
		}

		return true;
	}
}
