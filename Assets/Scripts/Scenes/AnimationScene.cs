/*
 * Creator:ffm
 * Desc:第一个游戏场景
 * Time:2020/4/11 17:45:38
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;

public class AnimationScene : IScene
{
	public class AnimationSceneControl : ObjectBase
	{
		private Action<float> m_LoadAction;

		public void DestroyScene(Action<float> action)
		{
			StartCoroutine(StartDestroyScene(action));
		}

		public void LoadScene(Action<float> action)
		{
			m_LoadAction = action;

			StartCoroutine("StartLoadScene");
		}

		private void CreateGameObject(object target)
		{
			GameObject go = target as GameObject;
			if (go != null)
			{
				go.transform.position = new Vector3(0, -1, 0);
				go.transform.eulerAngles = Vector3.zero;
				go.transform.localScale = Vector3.one;
			}

			if (m_LoadAction != null)
			{
				m_LoadAction(100);
			}

			go.AddComponent<ElephentControl>();
		}

		private void CreateCharacter()
		{
			GameObject go = new GameObject();
			go.name = "Player";
			go.transform.position = Vector3.zero;
			go.transform.rotation = Quaternion.Euler(Vector3.zero);
			go.transform.localScale = Vector3.one;
			AnimationPlayer ch = go.AddComponent<AnimationPlayer>();
			ch.StartInitCharacter("elephant", GetCharacter);
		}

		private void GetCharacter(object t)
		{
			m_LoadAction(100);
			AnimationPlayer ch = t as AnimationPlayer;
			ch.InitCharacter();
			ch.SetCameraTra(new Vector3(0, 2, -10), Vector3.zero, Vector3.one);
		}

		private IEnumerator StartLoadScene()
		{
			yield return null;
			CreateCharacter();
			//ResObjectCallBackBase cb = new ResObjectCallBackBase();
			//cb.m_LoadType = ResObjectType.GameObject;
			//cb.m_FinshFunction = CreateGameObject;
			//ResObjectManager.Instance.LoadObject("elephant", ResObjectType.GameObject, cb);
		}

		private IEnumerator StartDestroyScene(Action<float> action)
		{
			yield return null;
		}
	}

	private AnimationSceneControl m_LoadControl;

	public AnimationScene(string name) : base(name)
	{

	}

	public override void InitScene()
	{
		///为什么要在这里创建，因为场景切换的过程当中会删除掉一部分
		GameObject scene = new GameObject();
		scene.name = m_SceneName;
		scene.gameObject.transform.position = Vector3.zero;
		scene.gameObject.transform.eulerAngles = Vector3.zero;
		scene.gameObject.transform.localScale = Vector3.one;
		m_LoadControl = scene.gameObject.AddComponent<AnimationSceneControl>();
	}

	public override void LoadScene(Action<float> action)
	{
		if (m_LoadControl != null)
		{
			m_LoadControl.LoadScene(action);
		}
	}
}
