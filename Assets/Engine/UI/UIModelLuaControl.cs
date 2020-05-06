/*需要屏蔽的警告*/
#pragma warning disable
/*
 * Creator:ffm
 * Desc:ui内容包含lua数据
 * Time:2020/5/6 15:16:20
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;
using XLua;
using System.IO;

namespace Game.Engine
{
	public class UIModelLuaControl : IUIModelControl
	{
		/// <summary>
		/// lua控制内容
		/// </summary>
		protected LuaEnv m_UILuaControl;

		protected LuaTable m_UILuaTable;

		/// <summary>
		/// lua名字
		/// </summary>
		protected string m_LuaName;

		protected Action m_LuaOpen;
		protected Action m_LuaUpdate;
		protected Action m_LuaDestroy;

		/// <summary>
		/// lua文件是否已经加载
		/// </summary>
		private bool m_IsLoadLua;

		public UIModelLuaControl() : base()
		{
			m_LuaName = string.Empty;
			m_IsLoadLua = false;
		}

		public override void CloseSelf(bool manager = false)
		{
			base.CloseSelf(manager);

			if (m_UILuaControl != null)
			{
				m_UILuaControl.Dispose();
				m_UILuaControl = null;
			}
		}

		/// <summary>
		/// 初始化自己
		/// </summary>
		/// <param name="layer"></param>
		/// <param name="arms"></param>
		public override void InitUIData(UILayer layer, params object[] arms)
		{
			base.InitUIData(layer, arms);

			if (!string.IsNullOrEmpty(m_LuaName))
			{
				m_UILuaControl = new LuaEnv();
			}

			m_UILuaTable = m_UILuaControl.NewTable();
			LuaTable meta = m_UILuaControl.NewTable();
			meta.Set("__index", m_UILuaControl.Global);
			m_UILuaTable.SetMetaTable(meta);
			meta.Dispose();
			m_UILuaTable.Set("self", this);

			string path = Application.dataPath + "/UseAB/Lua/" + m_LuaName + ".lua.txt";
			string ta = File.ReadAllText(path);
			m_UILuaControl.DoString(ta);

			Action luaAwake = m_UILuaTable.Get<Action>("init");
			m_UILuaTable.Get("open", out m_LuaOpen);
			m_UILuaTable.Get("update", out m_LuaUpdate);
			m_UILuaTable.Get("destroy", out m_LuaDestroy);

			if (luaAwake != null)
			{
				luaAwake();
			}
		}

		/// <summary>
		/// 打开自己
		/// </summary>
		/// <param name="target"></param>
		public override void OpenSelf(GameObject target)
		{
			base.OpenSelf(target);

			if (m_LuaOpen != null)
			{
				m_LuaOpen();
			}
		}
	}
}
