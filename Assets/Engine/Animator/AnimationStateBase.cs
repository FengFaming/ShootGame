/*
 * Creator:ffm
 * Desc:动画状态基类
 * Time:2020/1/9 15:01:49
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;

namespace Game.Engine
{
	[System.Serializable]
	public class AnimationStateBase
	{
		/// <summary>
		/// 状态名字
		/// </summary>
		public string StateName;

		public virtual void EnterState()
		{

		}

		public virtual void ExitState()
		{

		}
	}
}
