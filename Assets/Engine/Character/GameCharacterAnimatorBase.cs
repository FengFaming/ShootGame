/*
 * Creator:ffm
 * Desc:角色动作机控制类
 * Time:2020/4/13 16:58:54
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Engine
{
	public class GameCharacterAnimatorBase
	{
		/// <summary>
		/// 对象内容
		/// </summary>
		protected Animator m_OwnerAnimator;

		public GameCharacterAnimatorBase(Animator ator)
		{
			m_OwnerAnimator = ator;
			if (m_OwnerAnimator == null)
			{
				Debug.LogError("the ch is error.");
			}
		}

		/// <summary>
		/// Update更新
		/// </summary>
		public virtual void Update()
		{

		}
	}
}
