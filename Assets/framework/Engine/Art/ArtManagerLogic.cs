/*
 *  create file time:1/11/2018 3:05:25 PM
 *  Describe:用来管理资源的，主要是内部逻辑处理，比如加载ab包等
* */

using System;
using System.Collections.Generic;
using UnityEngine;
using Framework.Engine;
using System.Collections;

namespace Framework.Engine.Art
{
    /// <summary>
    /// 这个类里面主要是供内部调用的内容
    /// </summary>
    public partial class ArtManager : BaseMonoSingleton<ArtManager>
    {
        private AssetBundle m_AvitarAB;
        private AssetBundle m_SpriteAB;
        private AssetBundle m_UIIconAB;
        private AssetBundle m_ShareabAB;

        private bool m_IsLoad;

        public override bool Initilize()
        {
            if (!base.Initilize())
                return false;

            m_IsLoad = false;
            StartLoadSareab();

            return true;
        }

        private void StartLoadSareab()
        {
            //m_IsLoad = true;
            //if (m_IsLoad)
            //{
            //    StartCoroutine("LoadSareab");
            //}

            AssetBundle manifest = AssetBundle.LoadFromFile(string.Format("{0}/{1}", Application.streamingAssetsPath, "ABFile/shareab"));
            if (manifest != null)
            {
                m_ShareabAB = manifest;
                Debug.Log(m_ShareabAB);
                AssetBundleManifest m = manifest.LoadAsset<AssetBundleManifest>("shareabManifest");
                Debug.Log(m);
                //string[] depends = m.GetAllDependencies();
            }
        }

        private void StartLoadAvitar()
        {
            //m_IsLoad = true;
            //if (m_IsLoad)
            //{
            //    StartCoroutine("LoadAvitar");
            //}

            AssetBundle avitar = AssetBundle.LoadFromFile(GetABPath(1));
            if (avitar != null)
            {
                //AssetBundleManifest mainfest = avitar.LoadAsset<AssetBundleManifest>("avitar");
                m_AvitarAB = avitar;
            }
        }

        private void StartLoadSprite()
        {
            m_IsLoad = true;
            if (m_IsLoad)
            {
                StartCoroutine("LoadSprite");
            }
        }

        private void StartLoadUIIcon()
        {
            //m_UIIconAB = AssetBundle.LoadFromFile(GetABPath(3));
            m_IsLoad = true;
            if (m_IsLoad)
            {
                StartCoroutine("LoadUIIcon");
            }
        }

        private IEnumerator LoadUIIcon()
        {
            //m_UIIconAB = AssetBundle.LoadFromFile(GetABPath(3));
            m_IsLoad = false;
            WWW www = new WWW(GetABPath(3));
            yield return www;

            m_UIIconAB = www.assetBundle;
            m_IsLoad = true;
            yield return m_IsLoad;
        }

        private IEnumerator LoadSareab()
        {
            m_IsLoad = false;
            //WWW www = new WWW(GetABPath(4));
            WWW www = WWW.LoadFromCacheOrDownload(GetABPath(4), 0);
            yield return www;

            m_ShareabAB = www.assetBundle;
            m_IsLoad = true;
            yield return m_IsLoad;
        }

        private IEnumerator LoadAvitar()
        {
            m_IsLoad = false;
            WWW www = WWW.LoadFromCacheOrDownload(GetABPath(1), 0);
            yield return www;

            m_AvitarAB = www.assetBundle;
            m_IsLoad = true;
            yield return m_IsLoad;
        }

        private IEnumerator LoadSprite()
        {
            m_IsLoad = false;
            WWW www = new WWW(GetABPath(2));
            yield return www;

            m_SpriteAB = www.assetBundle;
            m_IsLoad = true;
            yield return m_IsLoad;
        }

        private string GetABPath(int type)
        {
            string path = string.Format("{0}/{1}", Application.streamingAssetsPath, "ABFile/");
            if (type == 1)
            {
                path += "avitar";
            }

            if (type == 2)
            {
                path += "sprite";
            }

            if (type == 3)
            {
                path += "uiiocn";
            }

            if (type == 4)
            {
                path += "shareab";
            }

            //path = "file//" + path;
            return path;
        }
    }
}