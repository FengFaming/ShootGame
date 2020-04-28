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

	private bool m_IsShoot;

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
		m_IsShoot = false;
	}

	private void OnMoveWithSpeed(Vector3 sp)
	{
		Vector3 target = this.gameObject.transform.position;
		target += sp * Time.deltaTime;
		if (target.x >= -4 && target.x <= 4 && target.y >= -5 && target.y <= 3.5f)
		{
			m_MoveControl.SetMove(target, sp);
		}
	}

	private void OnShoot()
	{
		m_IsShoot = !m_IsShoot;
	}

	protected override bool Update()
	{
		if (!base.Update())
		{
			return false;
		}

		if (m_IsShoot)
		{
			if (Time.time - m_LastShootTime > 0.001f)
			{
				m_LastShootTime = Time.time;
				ShootGameObjectControl spc = ObjectPoolManager.Instance.GetCloneObject("ShootGamePoolControl", "Sphere") as ShootGameObjectControl;
				ShootControl sc = spc.m_Target.AddComponent<ShootControl>();
				sc.m_Target = spc;
				sc.m_PoolName = "ShootGamePoolControl";
				Vector3 start = this.gameObject.transform.position + new Vector3(0, 0.3f, -1);
				sc.InitMove(start, start + new Vector3(0, 8, 0), 3f);
			}
		}

		return true;
	}

	private void AddSpace(params object[] arms)
	{
		GameMouseInputManager.KeyInfo info = (GameMouseInputManager.KeyInfo)arms[0];
		if (info.m_KeyState == GameMouseInputManager.KeyState.KeyUp)
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
