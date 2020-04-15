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

		GameMouseInputManager.Instance.SetMouseListen(EngineMessageHead.LISTEN_MOUSE_EVENT_FOR_INPUT_MANAGER, 5);
		MessageManger.Instance.AddMessageListener(EngineMessageHead.LISTEN_MOUSE_EVENT_FOR_INPUT_MANAGER,
			new IMessageBase(this.gameObject, false, ListenMouse));
	}

	private void ListenMouse(params object[] arms)
	{
		GameMouseInputManager.ListenEvent le = (GameMouseInputManager.ListenEvent)arms[0];

		if (le.m_MouseType == GameMouseInputManager.MouseEventType.Mouse_0_Up ||
			le.m_MouseType == GameMouseInputManager.MouseEventType.Mouse_0_Stay)
		{
			Vector3 target = new Vector3(le.m_ScenePosition.x, this.gameObject.transform.position.y, le.m_ScenePosition.z);
			Vector3 f = target - this.gameObject.transform.position;
			Quaternion qt = Quaternion.LookRotation(f);
			//Quaternion q = Quaternion.FromToRotation(Vector3.forward, f);
			//Vector3 n = q * Vector3.forward;
			//Vector3 worldUp = Vector3.up;
			//float dirDot = Vector3.Dot(n, worldUp);
			//Vector3 vproj = worldUp - n * dirDot;
			//vproj.Normalize();
			//float dotproj = Vector3.Dot(vproj, n);
			//float theta = Mathf.Acos(dotproj) * Mathf.Rad2Deg;
			//Quaternion qNew = Quaternion.AngleAxis(theta, n);
			//Quaternion qt = qNew * q;

			//float distance = Vector3.Distance(target, this.gameObject.transform.position);
			//Vector3 ft = this.gameObject.transform.forward * distance + this.gameObject.transform.position;
			//Vector3 tt = target - Vector3.Project(target, this.gameObject.transform.position);
			//Vector3 tf = ft - Vector3.Project(ft, this.gameObject.transform.position);
			//float angle = Vector3.Angle(tt, tf);
			//Vector3 eulerAngles = this.gameObject.transform.eulerAngles;
			//Debug.Log(eulerAngles);
			//eulerAngles.y += angle;
			//Debug.Log(eulerAngles);
			//this.gameObject.transform.eulerAngles = Vector3.Lerp(this.gameObject.transform.eulerAngles, eulerAngles, Time.deltaTime);
			//this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, f, Time.deltaTime);
			//Quaternion q = Quaternion.Euler(f);
			this.gameObject.transform.rotation = qt;// Quaternion.Lerp(this.gameObject.transform.rotation, qt, Time.deltaTime);
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
