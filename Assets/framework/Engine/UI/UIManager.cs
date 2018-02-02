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
        /// <summary>
        /// 紧急弹框
        /// </summary>
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
        private Color m_CloseColor;

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

            m_UIDataDic.Clear();
            m_UiBlockingDic.Clear();
            m_UiModuleDic.Clear();
            m_ShowType.Clear();
        }

        #region Inface
        #region 添加数据
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
        #endregion

        #region 显示UI
        public void ShowUI(Type type, params object[] arms)
        {
            ShowUIWithLayer(type, UIModuleLayer.None, arms);
        }

        public void ShowUIWithLayer(Type type, UIModuleLayer layer, params object[] arms)
        {
            ShowUIWithLayer(type, false, layer, arms);
        }

        public void ShowUIWithLayer(Type type, bool isOverLayer, UIModuleLayer layer, params object[] arms)
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
            RectTransform t = module.gameObject.GetComponent<RectTransform>();
            t.localPosition = Vector3.zero;
            t.sizeDelta = Vector2.zero;
            t.localScale = Vector3.one;

            CloseOrHideOtherModule(module, data);
        }
        #endregion

        /// <summary>
        /// 获取UI面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public T GetModule<T>(Type type) where T : UIModule
        {
            T t = default(T);
            if (m_ShowType.Contains(type))
            {
                if (m_UiModuleDic.ContainsKey(type))
                {
                    t = m_UiModuleDic[type] as T;
                }
                else if (m_UiBlockingDic.ContainsKey(type))
                {
                    t = m_UiBlockingDic[type] as T;
                }
            }

            return t;
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <param name="type"></param>
        public void CloseModule(Type type)
        {
            UIModuleData data;
            if (m_UIDataDic.TryGetValue(type, out data))
            {
                List<Type> closes = new List<Type>();
                closes.Add(type);
                closes.AddRange(GetLinkType(type));

                //List<UIModule> modules = new List<UIModule>();
                for (int index = 0; index < closes.Count; index++)
                {
                    UIModule module;
                    if (m_UiBlockingDic.TryGetValue(closes[index], out module))
                    {
                        //modules.Add(module);
                        m_UiBlockingDic.Remove(closes[index]);
                    }
                    else if (m_UiModuleDic.TryGetValue(closes[index], out module))
                    {
                        //modules.Add(module);
                        m_UiModuleDic.Remove(closes[index]);
                    }

                    if (m_ShowType.Contains(closes[index]))
                    {
                        m_ShowType.Remove(closes[index]);
                    }

                    if (module != null)
                    {
                        ///如果是紧急面板,那么关闭遮罩
                        if (module.ShowLayer == UIModuleLayer.Black)
                        {
                            SetCloseOnClick(false);
                        }

                        GameObject.Destroy(module.gameObject);
                    }
                    else
                    {
                        Debug.LogWarning(string.Format("the module type【{0}】 is not showing.", closes[index]));
                    }
                }

                ///因为关闭面板，所以重新打开界面
                ShowBack();
            }
            else
            {
                Debug.LogWarning(string.Format("the type【{0}】 is not module data.", type));
            }
        }
        #endregion

        #region Private
        private void SetCloseOnClick(bool show)
        {
            if (show)
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

        #region 关闭相关的
        private List<Type> GetLinkType(Type type)
        {
            List<Type> tps = new List<Type>();

            for (int index = 0; index < m_ShowType.Count; index++)
            {
                UIModuleData data;
                if (m_UIDataDic.TryGetValue(m_ShowType[index], out data))
                {
                    if (data.m_Linkeds != null)
                    {
                        foreach (Type item in data.m_Linkeds)
                        {
                            if (item.Equals(type))
                            {
                                tps.Add(data.m_Type);
                                break;
                            }
                        }
                    }
                }
            }

            return tps;
        }

        private void ShowBack()
        {
            for (int index = 0; index < m_ShowType.Count; index++)
            {
                UIModule module;
                if (m_UiBlockingDic.TryGetValue(m_ShowType[index], out module))
                {

                }
                else if (m_UiModuleDic.TryGetValue(m_ShowType[index], out module))
                {

                }

                if (module != null)
                {
                    ShowUI(m_ShowType[index]);
                    if ((module.ShowLayer & (UIModuleLayer.Black | UIModuleLayer.Pnl)) != 0)
                    {
                        break;
                    }
                }
            }
        }
        #endregion

        #region 打开相关的
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
            links.Add(data.m_Type);

            List<Type> ignoers = GetIgnoers(data);

            List<UIModule> closes = new List<UIModule>();
            if (module.ShowOverLayer)
            {
                for (int index = 0; index < m_ShowType.Count; index++)
                {
                    UIModule ui;
                    if (m_UiModuleDic.TryGetValue(m_ShowType[index], out ui))
                    {
                        if (!(links.Contains(ui.m_Type) || ignoers.Contains(ui.m_Type)))
                        {
                            if (!closes.Contains(ui))
                            {
                                closes.Add(ui);
                            }
                        }
                    }
                    else if (m_UiBlockingDic.TryGetValue(m_ShowType[index], out ui))
                    {
                        if (!(links.Contains(ui.m_Type) || ignoers.Contains(ui.m_Type)))
                        {
                            if (!closes.Contains(ui))
                            {
                                closes.Add(ui);
                            }
                        }
                    }
                }
            }

            if (closes.Count > 0)
            {
                for (int index = 0; index < closes.Count; index++)
                {
                    ManagerCloseUI(closes[index]);
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
            //if (m_UiModuleDic.ContainsKey(module.m_Type))
            //{
            //    m_UiModuleDic.Remove(module.m_Type);
            //}
            //else if (m_UiBlockingDic.ContainsKey(module.m_Type))
            //{
            //    m_UiBlockingDic.Remove(module.m_Type);
            //    SetCloseOnClick(false);
            //}

            //if (m_ShowType.Contains(module.m_Type))
            //{
            //    m_ShowType.Remove(module.m_Type);
            //}

            if (module != null)
            {
                module.CloseSelf();
            }
            //GameObject.Destroy(module.gameObject);
        }

        private string GetPrefabPath(Type type)
        {
            string path = type.ToString();

            if (path.IndexOf(".") >= 0)
            {
                path = path.Substring(path.LastIndexOf("."));
            }

            path = m_PrefabPath + path;

            return path;
        }

        private void SetParent(UIModule module)
        {
            switch (module.ShowLayer)
            {
                case UIModuleLayer.Black:
                    module.gameObject.transform.SetParent(m_BlockView);
                    module.gameObject.transform.SetAsLastSibling();
                    if (m_UiBlockingDic.ContainsKey(module.m_Type))
                    {
                        m_UiBlockingDic[module.m_Type] = module;
                    }
                    else
                    {
                        m_UiBlockingDic.Add(module.m_Type, module);
                    }

                    SetCloseOnClick(true);
                    break;
                case UIModuleLayer.Dlg:
                case UIModuleLayer.Pnl:
                case UIModuleLayer.None:
                    module.gameObject.transform.SetParent(m_PnlView);
                    module.gameObject.transform.SetAsLastSibling();
                    if (!m_UiModuleDic.ContainsKey(module.m_Type))
                    {
                        m_UiModuleDic.Add(module.m_Type, module);
                    }
                    else
                    {
                        m_UiModuleDic[module.m_Type] = module;
                    }
                    break;
                case UIModuleLayer.Pool:
                    module.gameObject.transform.SetParent(m_PoolView);
                    module.gameObject.transform.SetAsLastSibling();
                    break;
                case UIModuleLayer.Top:
                    module.gameObject.transform.SetParent(m_TopView);
                    module.gameObject.transform.SetAsLastSibling();
                    if (m_UiModuleDic.ContainsKey(module.m_Type))
                    {
                        m_UiModuleDic[module.m_Type] = module;
                    }
                    else
                    {
                        m_UiModuleDic.Add(module.m_Type, module);
                    }
                    break;
            }

            if (module.ShowLayer != UIModuleLayer.Pool)
            {
                if (m_ShowType.Contains(module.m_Type))
                {
                    m_ShowType.Remove(module.m_Type);
                }

                m_ShowType.Insert(0, module.m_Type);
            }
        }
        #endregion
        #endregion
    }
}
