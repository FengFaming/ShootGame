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
	public override void InitCharacter(GameCharacterCameraBase gameCharacterCameraBase = null, GameCharacterAttributeBase gameCharacterAttributeBase = null, GameCharacterAnimatorBase animatorBase = null)
	{
		base.InitCharacter(gameCharacterCameraBase, gameCharacterAttributeBase, animatorBase);
		BoxCollider bc = this.gameObject.GetComponentInChildren<BoxCollider>();
		BoxCollider bvc = this.gameObject.AddComponent<BoxCollider>();
		bvc.center = bc.center;
		bvc.size = bc.size;

		GameObject.DestroyImmediate(bc);

		this.gameObject.AddComponent<AnimatorBase>();

		GameMouseInputManager.Instance.SetMouseListen(EngineMessageHead.LISTEN_MOUSE_EVENT_FOR_INPUT_MANAGER, 5);
		MessageManger.Instance.AddMessageListener(EngineMessageHead.LISTEN_MOUSE_EVENT_FOR_INPUT_MANAGER,
			new IMessageBase(this.gameObject, false, ListenMouse));
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
