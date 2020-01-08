/*
 * Creator:ffm
 * Desc:动画状态机测试
 * Time:2020/1/8 14:22:31
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;

public class AnimatorTest : AnimatorBase
{
	int cout = 0;

	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Z))
		{
			ChangeParameter("IsWalk", !m_ControlTarget.GetBool("IsWalk"));
		}

		if (Input.GetKeyUp(KeyCode.X))
		{
			ChangeParameter("IsRun", !m_ControlTarget.GetBool("IsRun"));
		}

		if (Input.GetKeyUp(KeyCode.V))
		{
			ChangeParameter("Speed", cout++, AnimatorControllerParameterType.Int);
		}
	}
}
