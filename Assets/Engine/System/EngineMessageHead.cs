/*
 * Creator:ffm
 * Desc:框架消息头
 * Time:2020/4/11 14:48:01
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Engine
{
	public class EngineMessageHead
	{
		/// <summary>
		/// 场景加载
		///		true:false
		/// </summary>
		public static readonly string CHANGE_SCENE_MESSAGE = "CHANGE_SCENE_MESSAGE";

		/// <summary>
		/// 发送属性修改
		///		修改前的值:修改后的值
		/// </summary>
		public static readonly string CHANGE_CHARACTER_ATTRIBUTE_VALUE = "CHANGE_CHARACTER_ATTRIBUTE_VALUE{0}:{1}";

		/// <summary>
		/// 鼠标监听
		/// </summary>
		public static readonly string LISTEN_MOUSE_EVENT_FOR_INPUT_MANAGER = "LISTEN_MOUSE_EVENT_FOR_INPUT_MANAGER";
	}
}
