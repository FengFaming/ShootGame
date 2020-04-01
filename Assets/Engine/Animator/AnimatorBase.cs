/*
 * Creator:ffm
 * Desc:动画状态机基类
 * Time:2020/1/8 15:02:18
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Engine;

namespace Game.Engine
{
#if UNITY_EDITOR
	using UnityEditor;

	public class MyAnimatorParameters
	{
		public string name { get; set; }
		public AnimatorControllerParameterType type { get; set; }
		public float defaultFloat { get; set; }
		public int defaultInt { get; set; }
		public bool defaultBool { get; set; }

		public MyAnimatorParameters() : this(default(AnimatorControllerParameter))
		{

		}

		public MyAnimatorParameters(AnimatorControllerParameter other)
		{
			name = other.name;
			type = other.type;
			defaultFloat = other.defaultFloat;
			defaultInt = other.defaultInt;
			defaultBool = other.defaultBool;
		}
	}
#endif
	/// <summary>
	/// 动画状态机基类
	/// </summary>
	public class AnimatorBase : ObjectBase
	{
		/// <summary>
		/// 动画帧事件
		/// </summary>
		public class AnimationEventInfo
		{
			/// <summary>
			/// 对应回调的名字
			/// </summary>
			public string m_EventName;

			/// <summary>
			/// 对应帧事件
			/// </summary>
			public float m_EventTime;

			/// <summary>
			/// 回调的参数
			/// </summary>
			public string m_EventValue;
		}

		/// <summary>
		/// 控制的对象
		/// </summary>
		protected Animator m_ControlTarget;

		/// <summary>
		/// 所有的状态内容
		/// </summary>
		public Dictionary<string, AnimationStateBase> m_AllStateDic;

#if UNITY_EDITOR
		/// <summary>
		/// 所有的控制条件
		/// </summary>
		public List<MyAnimatorParameters> m_AllParameters;
#endif

		private void Awake()
		{
			InitAnimator(this.gameObject.GetComponent<Animator>());
		}

		/// <summary>
		/// 初始化内容
		/// </summary>
		/// <param name="owner"></param>
		public void InitAnimator(Animator owner)
		{
			m_ControlTarget = owner;
			m_AllStateDic = new Dictionary<string, AnimationStateBase>();
			m_AllStateDic.Clear();
			if (m_ControlTarget != null)
			{
				AnimationClip[] clips = m_ControlTarget.runtimeAnimatorController.animationClips;
				for (int index = 0; index < clips.Length; index++)
				{
					AnimationStateBase ab = new AnimationStateBase();
					ab.StateName = clips[index].name;
					m_AllStateDic.Add(ab.StateName, ab);

					clips[index].events = default(AnimationEvent[]);
					string name = clips[index].name;
					AnimationEvent start = new AnimationEvent();
					start.time = 0;
					start.functionName = "StartAnimation";
					start.stringParameter = name;
					clips[index].AddEvent(start);

					AnimationEvent end = new AnimationEvent();
					end.time = clips[index].length;
					end.functionName = "EndAnimation";
					end.stringParameter = name;
					clips[index].AddEvent(end);
				}
			}

#if UNITY_EDITOR
			m_AllParameters = new List<MyAnimatorParameters>();
			m_AllParameters.Clear();
			AnimatorControllerParameter[] acps = m_ControlTarget.parameters;
			for (int index = 0; index < acps.Length; index++)
			{
				m_AllParameters.Add(new MyAnimatorParameters(acps[index]));
			}
#endif
		}

		/// <summary>
		/// 修改控制数据
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="type"></param>
		public virtual void ChangeParameter(string name, object value, AnimatorControllerParameterType type = AnimatorControllerParameterType.Bool)
		{
			switch (type)
			{
				case AnimatorControllerParameterType.Bool:
					m_ControlTarget.SetBool(name, (bool)value);
					break;
				case AnimatorControllerParameterType.Trigger:
					m_ControlTarget.SetBool(name, (bool)value);
					break;
				case AnimatorControllerParameterType.Float:
					m_ControlTarget.SetFloat(name, (float)value);
					break;
				case AnimatorControllerParameterType.Int:
					m_ControlTarget.SetInteger(name, (int)value);
					break;
			}

#if UNITY_EDITOR
			for (int index = 0; index < m_AllParameters.Count; index++)
			{
				if (m_AllParameters[index].name == name)
				{
					switch (m_AllParameters[index].type)
					{
						case AnimatorControllerParameterType.Bool:
							m_AllParameters[index].defaultBool = (bool)value;
							break;
						case AnimatorControllerParameterType.Trigger:
							m_AllParameters[index].defaultBool = (bool)value;
							break;
						case AnimatorControllerParameterType.Float:
							m_AllParameters[index].defaultFloat = (float)value;
							break;
						case AnimatorControllerParameterType.Int:
							m_AllParameters[index].defaultInt = (int)value;
							break;
					}
				}
			}

			if (Selection.activeGameObject != null)
			{
				///主动刷新选中物体
				EditorUtility.SetDirty(Selection.activeGameObject);
			}
#endif
		}

		/// <summary>
		/// 添加动画帧事件
		/// </summary>
		/// <param name="events"></param>
		public virtual void InitAnimationEvent(Dictionary<string, List<AnimationEventInfo>> events)
		{
			if (m_ControlTarget == null)
			{
				m_ControlTarget = this.gameObject.GetComponent<Animator>();
			}

			if (m_ControlTarget != null)
			{
				AnimationClip[] clips = m_ControlTarget.runtimeAnimatorController.animationClips;
				for (int index = 0; index < clips.Length; index++)
				{
					clips[index].events = default(AnimationEvent[]);
					string name = clips[index].name;
					AnimationEvent start = new AnimationEvent();
					start.time = 0;
					start.functionName = "StartAnimation";
					start.stringParameter = name;
					clips[index].AddEvent(start);

					AnimationEvent end = new AnimationEvent();
					end.time = clips[index].length;
					end.functionName = "EndAnimation";
					end.stringParameter = name;
					clips[index].AddEvent(end);

					if (events.ContainsKey(name))
					{
						for (int i = 0; i < events[name].Count; i++)
						{
							AnimationEvent aevent = new AnimationEvent();
							aevent.time = events[name][i].m_EventTime * clips[index].length;
							aevent.functionName = events[name][i].m_EventName;
							aevent.stringParameter = events[name][i].m_EventValue;
							clips[index].AddEvent(aevent);
						}
					}
				}
			}
		}

		public virtual void StartAnimation(string name)
		{
			if (m_AllStateDic.ContainsKey(name))
			{
				m_AllStateDic[name].EnterState();
			}
		}

		public virtual void EndAnimation(string name)
		{
			if (m_AllStateDic.ContainsKey(name))
			{
				m_AllStateDic[name].ExitState();
			}
		}
	}
}
