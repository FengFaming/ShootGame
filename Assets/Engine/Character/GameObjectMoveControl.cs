/*
 * Creator:ffm
 * Desc:use update move
 * Time:2020/4/16 14:07:47
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Engine
{
	/// <summary>
	/// ʹ��update�����ƶ�����
	/// </summary>
	public class GameObjectMoveControl : ObjectBase
	{
		/// <summary>
		/// �ƶ�����
		/// </summary>
		private enum MoveType
		{
			/// <summary>
			/// ��Ŀ���
			/// </summary>
			Target,

			/// <summary>
			/// ��ʱ��
			/// </summary>
			Time,
		}

		/// <summary>
		/// �ƶ��յ�
		/// </summary>
		private Vector3 m_MoveTarget;

		/// <summary>
		/// �����ٶ�
		/// </summary>
		private Vector3 m_ForwardSpeed;

		/// <summary>
		/// �ƶ�ʱ��
		/// </summary>
		private float m_MoveTime;

		/// <summary>
		/// �ƶ������ص�
		/// </summary>
		private Action<bool> m_MoveEnd;

		/// <summary>
		/// �ƶ�����
		/// </summary>
		private MoveType m_MoveType;

		/// <summary>
		/// �Ƿ���Կ�ʼ�ƶ�
		/// </summary>
		private bool m_StartMove;

		/// <summary>
		/// �µ���ת����
		/// </summary>
		private Quaternion m_NewQuaternion;

		/// <summary>
		/// ��תʱ��
		/// </summary>
		private float m_RotationTime;

		/// <summary>
		/// �Ƿ�ʼ��ת
		/// </summary>
		private bool m_HasRotation;

		private void Awake()
		{
			m_MoveTarget = this.gameObject.transform.position;
			m_ForwardSpeed = Vector3.zero;
			m_MoveTime = 0f;
			m_MoveEnd = null;
			m_MoveType = MoveType.Target;
			m_StartMove = false;

			m_NewQuaternion = this.gameObject.transform.rotation;
			m_RotationTime = 0f;
			m_HasRotation = false;
		}

		/// <summary>
		/// ������ת
		/// </summary>
		/// <param name="rotation"></param>
		/// <param name="time"></param>
		public void SetRotation(Quaternion rotation, float time)
		{
			m_NewQuaternion = rotation;
			m_RotationTime = time;
			m_HasRotation = true;
		}

		/// <summary>
		/// �����ƶ�
		/// </summary>
		/// <param name="target"></param>
		/// <param name="forwardSpeed"></param>
		/// <param name="moveTime"></param>
		/// <param name="end"></param>
		public void SetMove(Vector3 target, Vector3 forwardSpeed, float moveTime = -1f, Action<bool> end = null)
		{
			m_StartMove = false;
			m_MoveTarget = target;
			m_ForwardSpeed = forwardSpeed;
			m_MoveTime = moveTime;
			m_MoveEnd = end;

			if (forwardSpeed != Vector3.zero && moveTime > 0)
			{
				m_MoveType = MoveType.Time;
			}

			if (m_MoveType == MoveType.Target)
			{
				if (m_MoveTime > 0)
				{
					float distance = Vector3.Distance(this.gameObject.transform.position, m_MoveTarget);
					float s = distance / m_MoveTime;
					Vector3 f = Vector3.Normalize(m_MoveTarget - this.gameObject.transform.position);
					m_ForwardSpeed = f * s;
				}
			}

			m_StartMove = true;
		}

		private void Update()
		{
			if (m_HasRotation)
			{
				this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation, m_NewQuaternion, m_RotationTime);
			}

			if (m_StartMove)
			{
				if (m_MoveType == MoveType.Target)
				{
					Vector3 t = m_ForwardSpeed * Time.deltaTime + this.gameObject.transform.position;
					this.gameObject.transform.position = t;

					if (Vector3.Normalize(m_MoveTarget - this.gameObject.transform.position) !=
							Vector3.Normalize(m_ForwardSpeed))
					{
						CalExit(true);
					}
				}
				else if (m_MoveType == MoveType.Time)
				{
					Vector3 t = m_ForwardSpeed * Time.deltaTime + this.gameObject.transform.position;
					this.gameObject.transform.position = t;
					m_MoveTime -= Time.deltaTime;
					if (m_MoveTime < 0)
					{
						CalExit(true);
					}
				}
			}
		}

		private void OnDestroy()
		{
			m_StartMove = false;
			m_ForwardSpeed = Vector3.zero;
			m_MoveTarget = this.gameObject.transform.position;
			m_MoveTime = -1f;
			m_MoveType = MoveType.Target;
			m_MoveEnd = null;
		}

		private void CalExit(bool end)
		{
			m_StartMove = false;
			m_ForwardSpeed = Vector3.zero;
			m_MoveTarget = this.gameObject.transform.position;
			m_MoveTime = -1f;
			m_MoveType = MoveType.Target;
			Action<bool> e = m_MoveEnd;
			m_MoveEnd = null;

			if (e != null)
			{
				e(end);
			}
		}
	}
}
