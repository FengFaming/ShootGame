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

		private int m_LoadCont;

		public void DestroyScene(Action<float> action)
		{
			StartCoroutine(StartDestroyScene(action));
		}

		public void LoadScene(Action<float> action)
		{
			m_LoadAction = action;
			m_LoadCont = 0;

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
			m_LoadCont++;
			m_LoadAction(m_LoadCont / 7 * 100);
			AnimationPlayer ch = t as AnimationPlayer;
			ch.InitCharacter();
			ch.SetCameraTra(new Vector3(0, 2, -10), Vector3.zero, Vector3.one);
		}

		private void Nomalize(GameObject go, Vector3 position, Vector3 rotation, Vector3 scale)
		{
			go.gameObject.transform.position = position;
			go.gameObject.transform.rotation = Quaternion.Euler(rotation);
			go.gameObject.transform.localScale = scale;
		}

		private void GetGrass(object t)
		{
			GameObject go = t as GameObject;
			GameObject parent = new GameObject();
			parent.name = "garss";
			Nomalize(parent, Vector3.zero, Vector3.zero, Vector3.one);
			EngineTools.Instance.CreateRect(parent.transform, go, 3, 5);
			m_LoadCont++;
			m_LoadAction(m_LoadCont / 7 * 100);
		}

		private void CreateGrass()
		{
			ResObjectCallBackBase cb = new ResObjectCallBackBase();
			cb.m_FinshFunction = GetGrass;
			cb.m_LoadType = ResObjectType.GameObject;
			ResObjectManager.Instance.LoadObject("grass", ResObjectType.GameObject, cb);
		}

		private IEnumerator StartLoadScene()
		{
			yield return null;
			CreateCharacter();
			yield return null;
			for (int index = 0; index < 6; index++)
			{
				CreateGrass();
				yield return new WaitForEndOfFrame();
				yield return new WaitForFixedUpdate();
			}
		}

		private IEnumerator StartDestroyScene(Action<float> action)
		{
			yield return null;
			m_LoadCont = 0;
			action(100);
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
