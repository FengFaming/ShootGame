/*
 * Creator:ffm
 * Desc:游戏场景管理器
 * Time:2020/4/11 10:50:34
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Engine
{
	public class GameSceneManager : SingletonMonoClass<GameSceneManager>
	{
		/// <summary>
		/// 是否正在切换场景当中
		/// </summary>
		private bool m_IsChangeScene;

		protected override void Awake()
		{
			base.Awake();
			m_IsChangeScene = false;
		}

		/// <summary>
		/// 切换场景
		/// </summary>
		/// <param name="id"></param>
		public void ChangeScene(int id)
		{
			if (m_IsChangeScene)
			{
				Debug.LogError("wait...Scene is changing.");
				return;
			}

			m_IsChangeScene = true;
			StopAllCoroutines();
			StartCoroutine("ChangeStart", id);
		}

		private IEnumerator ChangeStart(int id)
		{
			yield return null;
			UIManager.Instance.ClearAllUI();
			MessageManger.Instance.SendMessage(EngineMessageHead.CHANGE_SCENE_MESSAGE, true);
			yield return new WaitForEndOfFrame();
			yield return new WaitForFixedUpdate();

			AsyncOperation asy = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
			yield return asy;

			//把这个内容提到这个位置，是因为打开界面的时候不能清空资源
			ResObjectManager.Instance.ClearAll();
			yield return new WaitForEndOfFrame();
			yield return new WaitForFixedUpdate();

			AsyncOperation asyt = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(id);
			yield return asyt;

			yield return new WaitForEndOfFrame();
			yield return new WaitForFixedUpdate();

			UIManager.Instance.OpenUI("UIPnlGameMain", UILayer.Bot);
			MessageManger.Instance.SendMessage(EngineMessageHead.CHANGE_SCENE_MESSAGE, false);
			m_IsChangeScene = false;
		}
	}
}
