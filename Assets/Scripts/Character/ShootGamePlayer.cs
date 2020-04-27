/*需要屏蔽的警告*/
#pragma warning disable
/*
 * Creator:ffm
 * Desc:射击游戏主角
 * Time:2020/4/27 15:38:16
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;

public class ShootGamePlayer : GameCharacterBase
{
	/// <summary>
	/// 上一次射击时间
	/// </summary>
	private float m_LastShootTime;

	/// <summary>
	/// 移动控制
	/// </summary>
	private GameObjectMoveControl m_MoveControl;

	public override void InitCharacter(GameCharacterCameraBase gameCharacterCameraBase = null,
										GameCharacterAttributeBase gameCharacterAttributeBase = null,
										GameCharacterAnimatorBase animatorBase = null,
										GameCharacterStateManager gameCharacterStateManager = null)
	{
		base.InitCharacter(gameCharacterCameraBase, gameCharacterAttributeBase, animatorBase, gameCharacterStateManager);
		m_CharacterStateManager.TryGotoState(0);
		m_LastShootTime = Time.time;

		MessageManger.Instance.AddMessageListener(EngineMessageHead.LISTEN_KEY_EVENT_FOR_INPUT_MANAGER + "-" + (int)KeyCode.A,
			new IMessageBase(this.gameObject, false, AddWASD));

		MessageManger.Instance.AddMessageListener(EngineMessageHead.LISTEN_KEY_EVENT_FOR_INPUT_MANAGER + "-" + (int)KeyCode.D,
			new IMessageBase(this.gameObject, false, AddWASD));

		MessageManger.Instance.AddMessageListener(EngineMessageHead.LISTEN_KEY_EVENT_FOR_INPUT_MANAGER + "-" + (int)KeyCode.W,
			new IMessageBase(this.gameObject, false, AddWASD));

		MessageManger.Instance.AddMessageListener(EngineMessageHead.LISTEN_KEY_EVENT_FOR_INPUT_MANAGER + "-" + (int)KeyCode.S,
			new IMessageBase(this.gameObject, false, AddWASD));

		MessageManger.Instance.AddMessageListener(EngineMessageHead.LISTEN_KEY_EVENT_FOR_INPUT_MANAGER + "-" + (int)KeyCode.Space,
			new IMessageBase(this.gameObject, false, AddSpace));

		m_MoveControl = this.gameObject.AddComponent<GameObjectMoveControl>();
	}

	private void OnMoveWithSpeed(Vector3 sp)
	{
		Vector3 target = this.gameObject.transform.position;
		target += sp * Time.deltaTime;
		target.x = Mathf.Clamp(target.x, -4, 4);
		target.y = Mathf.Clamp(target.y, -5, 5);
		Debug.Log(target);
		m_MoveControl.SetMove(target, sp);
	}

	private void OnShoot()
	{
		if (Time.time - m_LastShootTime > 10)
		{
			m_LastShootTime = Time.time;
			Debug.Log("shoot");
		}
	}

	private void AddSpace(params object[] arms)
	{
		GameMouseInputManager.KeyInfo info = (GameMouseInputManager.KeyInfo)arms[0];
		if (info.m_KeyState == GameMouseInputManager.KeyState.KeyStay)
		{
			OnShoot();
		}
	}

	private void AddWASD(params object[] arms)
	{
		GameMouseInputManager.KeyInfo info = (GameMouseInputManager.KeyInfo)arms[0];
		Vector3 focale = Vector3.zero;
		if (info.m_KeyState == GameMouseInputManager.KeyState.KeyStay)
		{
			if (info.m_KeyCode == KeyCode.A)
			{
				focale.x = -1;
			}

			if (info.m_KeyCode == KeyCode.D)
			{
				focale.x = 1;
			}

			if (info.m_KeyCode == KeyCode.W)
			{
				focale.y = 1;
			}

			if (info.m_KeyCode == KeyCode.S)
			{
				focale.y = -1;
			}

			OnMoveWithSpeed(focale);
		}
	}
}
