/*
 *  create file time:12/12/2017 10:18:37 AM
 *  Describe:UI数据
* */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Engine
{
    public class UIModuleData
    {
        public Type m_Type;
        public Type[] m_Linkeds;
        public bool m_HideOtherModules;
        public Type[] m_IgnoreMutexs;

        public UIModuleData(Type type, Type[] linkeds, bool hideotyermodules, Type[] ignoremutexs)
        {
            this.m_Type = type;
            this.m_Linkeds = linkeds;
            this.m_HideOtherModules = hideotyermodules;
            this.m_IgnoreMutexs = ignoremutexs;
        }

        public UIModuleData(Type type, Type[] linkeds, bool hideotyermodules) : this(type, linkeds, hideotyermodules, null) { }
        public UIModuleData(Type type, Type[] linkeds) : this(type, linkeds, false) { }
        public UIModuleData(Type type) : this(type, null) { }
    }

    public partial class UIModule : MonoBehaviour
    {
        [HideInInspector]
        public Type m_Type;

        [SerializeField]
        private UIModuleLayer m_Layer;
        [SerializeField]
        private bool m_IsOverLayer;

        private UIModuleLayer m_ShowLayer;
        public UIModuleLayer ShowLayer { get { return m_ShowLayer; } }

        private bool m_ShowOverLayer;
        public bool ShowOverLayer { get { return m_ShowOverLayer; } }

        /// <summary>
        /// 是否已经显示过
        /// </summary>
        private bool m_IsShowed;

        public bool OnShow(Type type, bool isOver, UIModuleLayer layer, params object[] arms)
        {
            if (!m_IsShowed)
            {
                m_Type = type;
                m_ShowLayer = layer == UIModuleLayer.None ? m_Layer : layer;
                m_ShowOverLayer = m_IsOverLayer & isOver;

                m_IsShowed = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CloseSelf()
        {
            UIManager.Instance.CloseModule(m_Type);
        }
    }
}
