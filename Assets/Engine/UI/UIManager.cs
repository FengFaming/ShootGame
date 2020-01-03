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
			private Action<object, IUIModelControl, List<string>, IUIModelControl> m_CallBack;

			public List<string> m_Others;
			public IUIModelControl m_OtherControl;

			public LoadUIModel(IUIModelControl control,
				Action<object, IUIModelControl, List<string>, IUIModelControl> cb)
			{
				m_Control = control;
				m_CallBack = cb;
				m_Others = new List<string>();
				m_Others.Clear();
				m_OtherControl = null;
			}

			/// <summary>
			/// 资源加载回调
			/// </summary>
			/// <param name="t"></param>
			public override void HandleLoadCallBack(object t)
			{
				if (m_CallBack != null)
				{
					m_CallBack(t, m_Control, m_Others, m_OtherControl);
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
		/// 需要加载的队列
		/// </summary>
		internal class OpenUIData
		{
			public string m_Name;
			public UILayer m_Layer;
			public List<object> m_Arms;

			public OpenUIData(string name, UILayer layer, params object[] arms)
			{
				m_Name = name;
				m_Layer = layer;
				m_Arms = new List<object>();
				if (arms != null)
				{
					m_Arms.AddRange(arms);
				}
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

		/// <summary>
		/// 父节点
		/// </summary>
		private Dictionary<UILayer, Transform> m_ParentDic;

		/// <summary>
		/// 需要打开的界面队列
		/// </summary>
		private List<OpenUIData> m_NeedOpenUIs;

		/// <summary>
		/// 已经有正在打开的界面
		/// </summary>
		private bool m_HasOpen;

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

			m_NeedOpenUIs = new List<OpenUIData>();
			m_NeedOpenUIs.Clear();
			m_HasOpen = false;
		}

		/// <summary>
		/// 打开界面
		/// </summary>
		/// <param name="name">类名</param>
		/// <param name="layer">层级</param>
		/// <param name="arms">变长参数</param>
		public void OpenUI(string name, UILayer layer, params object[] arms)
		{
			OpenUIData data = new OpenUIData(name, layer, arms);
			m_NeedOpenUIs.Add(data);
			OpenUI();
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

		#region 打开相关
		/// <summary>
		/// 内部打开数据
		/// </summary>
		private void OpenUI()
		{
			if (!m_HasOpen && m_NeedOpenUIs.Count > 0)
			{
				m_HasOpen = true;
				OpenUIData data = m_NeedOpenUIs[0];
				string name = data.m_Name;
				UILayer layer = data.m_Layer;
				IUIModelControl control = ReflexManager.Instance.CreateClass(name) as IUIModelControl;
				if (control != null)
				{
					if (control.m_IsOnlyOne)
					{
						if (m_AllShowUIModels.ContainsKey(layer))
						{
							for (int index = 0; index < m_AllShowUIModels[layer].Count; index++)
							{
								if (m_AllShowUIModels[layer][index].GetType().Name == name)
								{
									control = m_AllShowUIModels[layer][index];
									m_AllShowUIModels[layer].RemoveAt(index);
									break;
								}
							}
						}
					}

					control.InitUIData(layer, data.m_Arms.ToArray());
					OpenSelfUI(control);
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
			go.transform.SetParent(m_ParentDic[layer]);
			go.transform.SetAsLastSibling();

			RectTransform tf = go.GetComponent<RectTransform>();
			tf.localPosition = Vector3.zero;
			tf.localEulerAngles = Vector3.zero;
			tf.localScale = Vector3.one;
			tf.sizeDelta = Vector2.zero;
			tf.anchorMin = Vector2.zero;
			tf.anchorMax = Vector2.one;
			tf.pivot = Vector2.one * 0.5f;
		}

		/// <summary>
		/// 加载回调
		/// </summary>
		/// <param name="t"></param>
		/// <param name="control"></param>
		private void LoadCalBack(object t, IUIModelControl control,
			List<string> others, IUIModelControl other)
		{
			GameObject target = t as GameObject;
			SetParent(control.Layer, target);
			if (m_AllShowUIModels.ContainsKey(control.Layer))
			{
				for (int index = 0; index < m_AllShowUIModels[control.Layer].Count; index++)
				{
					if (m_AllShowUIModels[control.Layer][index].m_IsOnlyID != index)
					{
						control.m_IsOnlyID = index;
						break;
					}
				}

				m_AllShowUIModels[control.Layer].Add(control);
			}
			else
			{
				control.m_IsOnlyID = 0;
				m_AllShowUIModels.Add(control.Layer, new List<IUIModelControl>() { control });
			}

			m_AllShowUIModels[control.Layer].Sort((IUIModelControl c1, IUIModelControl c2) =>
			{
				return c1.m_IsOnlyID - c2.m_IsOnlyID;
			});

			m_ShowSequence.Insert(0, new KeyValuePair<UILayer, int>(control.Layer, control.m_IsOnlyID));
			control.OpenSelf(target);
			OpenOtherUI(others, control.Layer, other);
		}

		/// <summary>
		/// 打开其他关联数据
		/// </summary>
		/// <param name="others"></param>
		private void OpenOtherUI(List<string> others, UILayer layer, IUIModelControl self)
		{
			if (others != null && others.Count > 0)
			{
				string name = others[0];
				others.RemoveAt(0);
				IUIModelControl control = ReflexManager.Instance.CreateClass(name) as IUIModelControl;
				if (control == null)
				{
					OpenOtherUI(others, layer, self);
				}
				else
				{
					if (control.m_IsOnlyOne)
					{
						if (m_AllShowUIModels.ContainsKey(layer))
						{
							for (int index = 0; index < m_AllShowUIModels[layer].Count; index++)
							{
								if (m_AllShowUIModels[layer][index].GetType().Name == name)
								{
									control = m_AllShowUIModels[layer][index];
									m_AllShowUIModels[layer].RemoveAt(index);
									break;
								}
							}
						}
					}

					control.InitUIData(layer);
					if (control.ControlTarget == null)
					{
						LoadUIModel lu = new LoadUIModel(control, LoadCalBack);
						lu.m_OtherControl = self;
						lu.m_Others = others;
						ResObjectManager.Instance.LoadObject(control.m_ModelObjectPath,
							ResObjectType.UIPrefab, lu);
					}
					else
					{
						LoadCalBack(control.ControlTarget, control, others, self);
					}
				}
			}
			else
			{
				if (self != null)
				{
					if (self.ControlTarget == null)
					{
						LoadUIModel lu = new LoadUIModel(self, LoadCalBack);
						ResObjectManager.Instance.LoadObject(self.m_ModelObjectPath,
							ResObjectType.UIPrefab, lu);
					}
					else
					{
						LoadCalBack(self.ControlTarget, self, null, null);
					}
				}
				else
				{
					//打开完毕，进行其他的操作
					Debug.Log("open end.");
					OpenUIData data = m_NeedOpenUIs[0];
					m_NeedOpenUIs.RemoveAt(0);
					m_HasOpen = false;
					if (m_NeedOpenUIs.Count > 0)
					{
						OpenUI();
					}
				}
			}
		}

		/// <summary>
		/// 对象数据
		/// </summary>
		/// <param name="control"></param>
		private void OpenSelfUI(IUIModelControl control)
		{
			List<string> parents = control.GetLinksUI();
			if (parents.Count > 0)
			{
				OpenOtherUI(parents, control.Layer, control);
			}
			else
			{
				if (control.ControlTarget != null)
				{
					LoadCalBack(control.ControlTarget, control, null, null);
				}
				else
				{
					LoadUIModel lu = new LoadUIModel(control, LoadCalBack);
					ResObjectManager.Instance.LoadObject(control.m_ModelObjectPath,
						ResObjectType.UIPrefab, lu);
				}
			}
		}
		#endregion
	}
}
