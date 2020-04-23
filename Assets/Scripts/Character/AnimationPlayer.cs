/*
 * Creator:ffm
 * Desc:动作场景玩家
 * Time:2020/4/14 8:45:50
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;

public class AnimationPlayer : GameCharacterBase
{
	private Rigidbody m_ControlRigidbody;

	public override void InitCharacter(GameCharacterCameraBase gameCharacterCameraBase = null, GameCharacterAttributeBase gameCharacterAttributeBase = null, GameCharacterAnimatorBase animatorBase = null)
	{
		base.InitCharacter(gameCharacterCameraBase, gameCharacterAttributeBase, animatorBase);
		BoxCollider bc = this.gameObject.GetComponentInChildren<BoxCollider>();
		BoxCollider bvc = this.gameObject.AddComponent<BoxCollider>();
		bvc.center = bc.center;
		bvc.size = bc.size;
		GameObject.DestroyImmediate(bc);

		Rigidbody rb = this.gameObject.GetComponentInChildren<Rigidbody>();
		Rigidbody n = this.gameObject.AddComponent<Rigidbody>();
		EngineTools.Instance.CopyRigidbody(rb, ref n);
		GameObject.DestroyImmediate(rb);

		m_ControlRigidbody = n;

		this.gameObject.AddComponent<AnimatorBase>();
		GameMouseInputManager.Instance.SetMouseListen(EngineMessageHead.LISTEN_MOUSE_EVENT_FOR_INPUT_MANAGER, 5);
		MessageManger.Instance.AddMessageListener(EngineMessageHead.LISTEN_MOUSE_EVENT_FOR_INPUT_MANAGER,
			new IMessageBase(this.gameObject, false, ListenMouse));

		MessageManger.Instance.AddMessageListener(EngineMessageHead.LISTEN_KEY_EVENT_FOR_INPUT_MANAGER + "-" + (int)KeyCode.A,
			new IMessageBase(this.gameObject, false, ListenKey));

		MessageManger.Instance.AddMessageListener(EngineMessageHead.LISTEN_KEY_EVENT_FOR_INPUT_MANAGER + "-" + (int)KeyCode.D,
			new IMessageBase(this.gameObject, false, ListenKey));

		MessageManger.Instance.AddMessageListener(EngineMessageHead.LISTEN_KEY_EVENT_FOR_INPUT_MANAGER + "-" + (int)KeyCode.W,
			new IMessageBase(this.gameObject, false, ListenKey));

		MessageManger.Instance.AddMessageListener(EngineMessageHead.LISTEN_KEY_EVENT_FOR_INPUT_MANAGER + "-" + (int)KeyCode.S,
			new IMessageBase(this.gameObject, false, ListenKey));
	}

	private void ListenKey(params object[] arms)
	{
		GameMouseInputManager.KeyInfo info = (GameMouseInputManager.KeyInfo)arms[0];
		Vector3 focale = Vector3.zero;
		if (info.m_KeyState == GameMouseInputManager.KeyState.KeyStay)
		{
			if (info.m_KeyCode == KeyCode.A)
			{
				focale.x = -2;
			}

			if (info.m_KeyCode == KeyCode.D)
			{
				focale.x = 2;
			}

			if (info.m_KeyCode == KeyCode.W)
			{
				focale.z = 2;
			}

			if (info.m_KeyCode == KeyCode.S)
			{
				focale.z = -2;
			}

			MoveControl(focale);
		}

		if (info.m_KeyState == GameMouseInputManager.KeyState.KeyUp)
		{
			if (focale == Vector3.zero)
			{
				if (m_ControlRigidbody != null)
				{
					m_ControlRigidbody.velocity = focale;
				}

				m_CharacterAnimator.ChangeParameter("IsWalk", false);
				m_CharacterAnimator.ChangeParameter("IsRun", false);
			}
		}
	}

	private void MoveControl(Vector3 sp)
	{
		GameObjectMoveControl move = this.gameObject.GetComponent<GameObjectMoveControl>();
		if (move == null)
		{
			move = this.gameObject.AddComponent<GameObjectMoveControl>();
		}

		Quaternion q = Quaternion.LookRotation(sp);
		move.SetRotation(q, 0.1f);

		if (m_ControlRigidbody != null)
		{
			m_ControlRigidbody.velocity = sp;
			m_CharacterAnimator.ChangeParameter("IsWalk", true);
			m_CharacterAnimator.ChangeParameter("IsRun", false);
		}
	}

	private void ListenMouse(params object[] arms)
	{
		GameMouseInputManager.ListenEvent le = (GameMouseInputManager.ListenEvent)arms[0];

		if (le.m_MouseType == GameMouseInputManager.MouseEventType.Mouse_0_Up)
		{
			Vector3 target = new Vector3(le.m_ScenePosition.x, this.gameObject.transform.position.y, le.m_ScenePosition.z);
			Vector3 f = target - this.gameObject.transform.position;
			Quaternion qt = Quaternion.LookRotation(f);
			//this.gameObject.transform.rotation = qt;
			GameObjectMoveControl move = this.gameObject.GetComponent<GameObjectMoveControl>();
			if (move == null)
			{
				move = this.gameObject.AddComponent<GameObjectMoveControl>();
			}

			m_CharacterAnimator.ChangeParameter("IsWalk", true);
			m_CharacterAnimator.ChangeParameter("IsRun", false);
			move.SetRotation(qt, 0.2f);
			move.SetMove(target, Vector3.zero, 2, EndMove);
		}
		else if (le.m_MouseType == GameMouseInputManager.MouseEventType.Mouse_1_Up)
		{
			Vector3 target = new Vector3(le.m_ScenePosition.x, this.gameObject.transform.position.y, le.m_ScenePosition.z);
			Vector3 f = target - this.gameObject.transform.position;
			Quaternion qt = Quaternion.LookRotation(f);
			//this.gameObject.transform.rotation = qt;
			GameObjectMoveControl move = this.gameObject.GetComponent<GameObjectMoveControl>();
			if (move == null)
			{
				move = this.gameObject.AddComponent<GameObjectMoveControl>();
			}

			m_CharacterAnimator.ChangeParameter("IsWalk", true);
			m_CharacterAnimator.ChangeParameter("IsRun", true);
			move.SetRotation(qt, 0.2f);
			move.SetMove(target, Vector3.zero, 1, EndMove);
		}
	}

	private void EndMove(bool end)
	{
		if (end)
		{
			m_CharacterAnimator.ChangeParameter("IsRun", false);
			m_CharacterAnimator.ChangeParameter("IsWalk", false);
		}
	}

	protected override bool Update()
	{
		if (!base.Update())
			return false;

		if (Input.GetKeyDown(KeyCode.A))
		{
			StartListenMouse();
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			StopListenMouse();
		}

		return true;
	}
}
