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
		public void DestroyScene(Action<float> action)
		{
			StartCoroutine(StartDestroyScene(action));
		}

		public void LoadScene(Action<float> action)
		{
			StartCoroutine("StartLoadScene", action);
		}

		private void CreateGameObject(int id)
		{
			GameObject go = new GameObject();
			go.transform.position = Vector3.zero;
			go.transform.eulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;
			go.name = id.ToString();
		}

		private IEnumerator StartLoadScene(Action<float> action)
		{
			yield return null;
			UIManager.Instance.OpenUI("UIPnlGameMain", UILayer.Pnl);
			yield return new WaitForEndOfFrame();
			yield return new WaitForFixedUpdate();

			for (int index = 0; index < 1000; index++)
			{
				CreateGameObject(index);
				if (action != null)
				{
					action((int)((index / 1000.0f) * 100));
				}

				yield return null;
			}

			if (action != null)
			{
				action(100);
			}
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
