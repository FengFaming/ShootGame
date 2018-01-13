/*
 *  create file time:12/12/2017 10:10:22 AM
 *  Describe:管理UI面板
* */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Engine
{
    public enum UIModuleLayer
    {
        None,

        Pnl,

        Dlg,

        Black,

        Pool,

        Top
    }

    public class UIManager : BaseMonoSingleton<UIManager>
    {
        /// <summary>
        /// 所有的UI面板数据
        /// </summary>
        private static Dictionary<Type, UIModuleData> m_UIDataDic;

        /// <summary>
        /// 正常面板、弹框
        /// </summary>
        private static Dictionary<Type, UIModule> m_UiModuleDic;
        private static Dictionary<Type, UIModule> m_UiBlockingDic;

        /// <summary>
        /// 当前正在显示的UI
        /// </summary>
        private static List<Type> m_ShowType;

        [SerializeField]
        private Transform m_TopView;

        [SerializeField]
        private Transform m_PoolView;

        [SerializeField]
        private Transform m_PnlView;

        [SerializeField]
        private Transform m_BlockView;

        [SerializeField]
        private Color m_CloseColor = new Color(255, 255, 255, 0);

        [SerializeField]
        private string m_PrefabPath = "UI/";

        private GameObject m_CloseView;
        private int m_CloseCount;

        public override bool Initilize()
        {
            if (!base.Initilize())
                return false;

            m_UIDataDic = new Dictionary<Type, UIModuleData>();
            m_UIDataDic.Clear();

            m_UiModuleDic = new Dictionary<Type, UIModule>();
            m_UiModuleDic.Clear();

            m_UiBlockingDic = new Dictionary<Type, UIModule>();
            m_UiBlockingDic.Clear();

            m_ShowType = new List<Type>();
            m_ShowType.Clear();

            GameObject go = new GameObject("CloseOnClick");
            go.transform.parent = m_BlockView.parent;

            /*
             * pnl/dlg
             * top
             * black
             * pool
             * */
            m_PnlView.SetAsFirstSibling();
            m_TopView.SetAsFirstSibling();
            go.transform.SetAsLastSibling();
            m_BlockView.SetAsLastSibling();
            m_PoolView.SetAsFirstSibling();

            go.layer = m_BlockView.gameObject.layer;
            RectTransform rect = go.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = Vector2.one * 0.5f;
            rect.sizeDelta = Vector2.zero;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            go.AddComponent<CanvasRenderer>();
            go.AddComponent<Image>();
            (go.GetComponent<Image>()).color = m_CloseColor;

            m_CloseView = go;

            m_CloseCount = 0;
            SetCloseOnClick(false);

            return true;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void SetCloseOnClick(bool close)
        {
            if (close)
            {
                m_CloseCount += 1;
                m_CloseView.SetActive(true);
            }
            else
            {
                m_CloseCount -= 1;
                if (m_CloseCount <= 0)
                {
                    m_CloseCount = 0;
                    m_CloseView.SetActive(false);
                }
            }
        }

        #region Inface
        public void AddModuleData(UIModuleData data)
        {
            if (!m_UIDataDic.ContainsKey(data.m_Type))
            {
                m_UIDataDic.Add(data.m_Type, data);
            }
        }

        public void AddModulesData(UIModuleData[] datas)
        {
            for (int index = 0; index < datas.Length; index++)
            {
                AddModuleData(datas[index]);
            }
        }

        public void ShowUI(Type type, params object[] arms)
        {
            ShowUI(type, UIModuleLayer.None, arms);
        }

        public void ShowUI(Type type, UIModuleLayer layer, params object[] arms)
        {
            ShowUI(type, false, layer);
        }

        public void ShowUI(Type type, bool isOverLayer, UIModuleLayer layer, params object[] arms)
        {
            UIModuleData data;
            if (!m_UIDataDic.TryGetValue(type, out data))
            {
                Debug.LogWarning(string.Format("the uimodule[{0}] hadn't add.", type.ToString()));
                return;
            }

            OpenLinkeds(data);

            UIModule module;
            Dictionary<Type, UIModule> showUIDic = layer == UIModuleLayer.Black ? m_UiBlockingDic : m_UiModuleDic;
            if (!showUIDic.TryGetValue(type, out module))
            {
                object i = Resources.Load(GetPrefabPath(type));
                if (i == null)
                {
                    Debug.LogError(string.Format("the uimodule[{0}] not in [{1}] file.", type.ToString(), m_PrefabPath));
                    return;
                }

                GameObject prefab = GameObject.Instantiate(Resources.Load(GetPrefabPath(type))) as GameObject;
                prefab.name = type.ToString();
                module = prefab.GetComponent<UIModule>();
                if (module == null)
                {
                    GameObject.Destroy(prefab);
                    Debug.LogError(string.Format("the prefab【{0}】 is not ui", GetPrefabPath(type)));
                    return;
                }
            }

            module.OnShow(type, isOverLayer, layer, arms);
            SetParent(module);
            CloseOrHideOtherModule(module, data);
        }
        #endregion

        #region Private
        private void OpenLinkeds(UIModuleData data)
        {
            if (data.m_Linkeds != null)
            {
                foreach (Type type in data.m_Linkeds)
                {
                    ShowUI(type);
                }
            }
        }

        private void CloseOrHideOtherModule(UIModule module, UIModuleData data)
        {
            List<Type> links = GetLinkTypes(data);
            List<Type> ignoers = GetIgnoers(data);

            if (module.ShowOverLayer)
            {
                for (int index = 0; index < m_ShowType.Count; index++)
                {
                    UIModule ui;
                    if (m_UiModuleDic.TryGetValue(m_ShowType[index], out ui))
                    {
                        if (!(links.Contains(ui.m_Type) || ignoers.Contains(ui.m_Type)))
                        {
                            ManagerCloseUI(ui);
                        }
                    }
                }
            }
        }

        private List<Type> GetLinkTypes(UIModuleData data)
        {
            List<Type> types = new List<Type>();
            if (data.m_Linkeds != null)
            {
                foreach (Type type in data.m_Linkeds)
                {
                    UIModuleData sunData;
                    if (m_UIDataDic.TryGetValue(type, out sunData))
                    {
                        types.Add(type);
                        GetLinkTypes(sunData);
                    }
                }
            }

            return types;
        }

        private List<Type> GetIgnoers(UIModuleData data)
        {
            List<Type> types = new List<Type>();
            if (data.m_IgnoreMutexs != null)
            {
                foreach (Type type in data.m_IgnoreMutexs)
                {
                    UIModuleData sunData;
                    if (m_UIDataDic.TryGetValue(type, out sunData))
                    {
                        types.Add(type);
                        GetIgnoers(sunData);
                    }
                }
            }

            return types;
        }

        private void ManagerCloseUI(UIModule module)
        {

        }

        private string GetPrefabPath(Type type)
        {
            string path = type.ToString();

            path = path.Substring(path.LastIndexOf("."));
            path = m_PrefabPath + path;

            return path;
        }

        private void SetParent(UIModule module)
        {
            switch (module.ShowLayer)
            {
                case UIModuleLayer.Black:
                    module.gameObject.transform.parent = m_BlockView;
                    module.gameObject.transform.SetAsLastSibling();
                    m_UiBlockingDic.Add(module.m_Type, module);
                    SetCloseOnClick(true);
                    m_ShowType.Insert(0, module.m_Type);
                    break;
                case UIModuleLayer.Dlg:
                case UIModuleLayer.Pnl:
                case UIModuleLayer.None:
                    module.gameObject.transform.parent = m_PnlView;
                    module.gameObject.transform.SetAsLastSibling();
                    m_UiModuleDic.Add(module.m_Type, module);
                    m_ShowType.Insert(0, module.m_Type);
                    break;
                case UIModuleLayer.Pool:
                    module.gameObject.transform.parent = m_PoolView;
                    module.gameObject.transform.SetAsLastSibling();
                    break;
                case UIModuleLayer.Top:
                    module.gameObject.transform.parent = m_TopView;
                    module.gameObject.transform.SetAsLastSibling();
                    m_UiModuleDic.Add(module.m_Type, module);
                    m_ShowType.Insert(0, module.m_Type);
                    break;
            }
        }
        #endregion
    }
}
