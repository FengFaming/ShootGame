/*
 * Creator:ffm
 * Desc:UI管理器
 * Time:2019/12/27 15:00:50
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;

namespace Game.Engine
{
	public enum UILayer
	{
		None = 0,

		/// <summary>
		/// 最底层
		///		一般不会移动
		/// </summary>
		Bot = 1,

		/// <summary>
		/// 面板层
		/// </summary>
		Pnl = 2,

		/// <summary>
		/// 普通弹出框
		/// </summary>
		Dlg = 3,

		/// <summary>
		/// 提示层
		/// </summary>
		Tip = 4,

		/// <summary>
		/// 紧急弹框层
		/// </summary>
		Blk = 5,
	}

	public class UIManager : SingletonMonoClass<UIManager>
	{
		internal class LoadUIModel : IResObjectCallBack
		{
			private IUIModelControl m_Control;
			private UILayer m_Layer;
			private List<object> m_Arms;
			private Action<object[]> m_CallBack;

			public LoadUIModel(IUIModelControl control, UILayer layer, Action<object[]> cb, params object[] arms)
			{
				m_Control = control;
				m_CallBack = cb;
				m_Layer = layer;
				m_Arms = new List<object>();
				if (arms != null && arms.Length > 0)
				{
					m_Arms.AddRange(arms);
				}
			}

			/// <summary>
			/// 资源加载回调
			/// </summary>
			/// <param name="t"></param>
			public override void HandleLoadCallBack(object t)
			{
				if (m_CallBack != null)
				{
					m_Arms.Insert(0, m_Layer);
					m_Arms.Insert(0, t);
					m_Arms.Insert(0, m_Control);
					m_CallBack(m_Arms.ToArray());
				}
			}

			/// <summary>
			/// 加载回调优先级
			/// </summary>
			/// <returns></returns>
			public override int LoadCallbackPriority()
			{
				return 0;
			}
		}

		/// <summary>
		/// 所有正在显示的UI内容
		/// </summary>
		private Dictionary<UILayer, List<IUIModelControl>> m_AllShowUIModels;

		/// <summary>
		/// 显示的序列
		/// </summary>
		private List<KeyValuePair<UILayer, int>> m_ShowSequence;

		private Dictionary<UILayer, Transform> m_ParentDic;

		protected override void Awake()
		{
			base.Awake();
			m_AllShowUIModels = new Dictionary<UILayer, List<IUIModelControl>>();
			m_AllShowUIModels.Clear();

			m_ShowSequence = new List<KeyValuePair<UILayer, int>>();
			m_ShowSequence.Clear();

			m_ParentDic = new Dictionary<UILayer, Transform>();
			for (int index = 1; index <= this.gameObject.transform.childCount; index++)
			{
				int id = index - 1;
				Transform tf = this.gameObject.transform.GetChild(id);
				if (((UILayer)index).ToString() == tf.name)
				{
					m_ParentDic.Add((UILayer)index, tf);
				}
			}
		}

		/// <summary>
		/// 设置父亲节点
		/// </summary>
		/// <param name="layer"></param>
		/// <param name="go"></param>
		private void SetParent(UILayer layer, GameObject go)
		{
			go.transform.position = Vector3.zero;
			go.transform.eulerAngles = Vector3.zero;
			go.transform.SetParent(m_ParentDic[layer]);
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;
		}

		/// <summary>
		/// 加载回调
		/// </summary>
		/// <param name="arms"></param>
		private void LoadCalBack(params object[] arms)
		{
			IUIModelControl control = arms[0] as IUIModelControl;
			GameObject target = arms[1] as GameObject;
			UILayer layer = (UILayer)arms[2];
			List<object> ds = new List<object>();
			if (arms.Length > 3)
			{
				for (int index = 3; index < arms.Length; index++)
				{
					ds.Add(arms[index]);
				}
			}

			SetParent(layer, target);
			control.OpenSelf(target, layer, ds.ToArray());
			if (m_AllShowUIModels.ContainsKey(layer))
			{
				for (int index = 0; index < m_AllShowUIModels[layer].Count; index++)
				{
					if (m_AllShowUIModels[layer][index].m_IsOnlyID != index)
					{
						control.m_IsOnlyID = index;
						break;
					}
				}

				m_AllShowUIModels[layer].Add(control);
			}
			else
			{
				control.m_IsOnlyID = 0;
				m_AllShowUIModels.Add(layer, new List<IUIModelControl>() { control });
			}

			m_AllShowUIModels[layer].Sort((IUIModelControl c1, IUIModelControl c2) =>
			{
				return c1.m_IsOnlyID - c2.m_IsOnlyID;
			});

			m_ShowSequence.Insert(0, new KeyValuePair<UILayer, int>(layer, control.m_IsOnlyID));
		}

		/// <summary>
		/// 打开界面
		/// </summary>
		/// <param name="name">类名</param>
		/// <param name="layer">层级</param>
		/// <param name="arms">变长参数</param>
		public void OpenUI(string name, UILayer layer, params object[] arms)
		{
			IUIModelControl control = ReflexManager.Instance.CreateClass(name) as IUIModelControl;
			if (control != null)
			{
				if (!control.m_IsOnlyOne)
				{
					LoadUIModel lu = new LoadUIModel(control, layer, LoadCalBack, arms);
					ResObjectManager.Instance.LoadObject(control.m_ModelObjectPath, ResObjectType.UIPrefab, lu);
				}
				else
				{
					if (m_AllShowUIModels.ContainsKey(layer))
					{
						for (int index = 0; index < m_AllShowUIModels[layer].Count; index++)
						{
							if (m_AllShowUIModels[layer][index].GetType().Name == name)
							{
								return;
							}
						}
					}
					else
					{
						LoadUIModel lu = new LoadUIModel(control, layer, LoadCalBack, arms);
						ResObjectManager.Instance.LoadObject(control.m_ModelObjectPath, ResObjectType.UIPrefab, lu);
					}
				}
			}
		}

		/// <summary>
		/// 回收ui对象
		/// </summary>
		/// <param name="ui"></param>
		public void RecoveryUIModel(GameObject ui)
		{
			ui.SetActive(false);
			GameObject.Destroy(ui);
		}
	}
}
