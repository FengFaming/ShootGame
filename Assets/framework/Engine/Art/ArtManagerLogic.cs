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

            m_IsLoad = true;
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

            //AssetBundle manifest = AssetBundle.LoadFromFile(string.Format("{0}/{1}", Application.streamingAssetsPath, "ABFile/shareab"));
            //if (manifest != null)
            //{
            //    m_ShareabAB = manifest;
            //    AssetBundleManifest m = manifest.LoadAsset<AssetBundleManifest>("shareabManifest");
            //}

            //StartCoroutine(LoadSareab());
            Debug.Log(m_ShareabAB);
            //StartCoroutine(LoadUIIcon());
            //StartCoroutine(LoadSprite());
            StartCoroutine(LoadAvitar());
        }

        private void StartLoadAvitar()
        {
            if (m_IsLoad)
            {
                StartCoroutine(LoadAvitar());
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
            m_IsLoad = false;

            AssetBundleCreateRequest avitar = AssetBundle.LoadFromFileAsync(GetABPath(3));
            yield return avitar;
            while (!avitar.isDone)
            {
                yield return null;
            }

            if (avitar != null)
            {
                m_UIIconAB = avitar.assetBundle;
            }

            m_IsLoad = true;
            yield return m_IsLoad;
        }

        private IEnumerator LoadSareab()
        {
            m_IsLoad = false;

            AssetBundleCreateRequest avitar = AssetBundle.LoadFromFileAsync(GetABPath(4));
            yield return avitar;
            while (!avitar.isDone)
            {
                yield return null;
            }

            if (avitar != null)
            {
                m_ShareabAB = avitar.assetBundle;
            }

            m_IsLoad = true;
            yield return m_IsLoad;
        }

        private IEnumerator LoadAvitar()
        {
            m_IsLoad = false;

            AssetBundleCreateRequest avitar = AssetBundle.LoadFromFileAsync(GetABPath(1));
            yield return avitar;
            while (!avitar.isDone)
            {
                yield return null;
            }

            if (avitar != null)
            {
                m_AvitarAB = avitar.assetBundle;
            }

            m_IsLoad = true;
            yield return m_IsLoad;
        }

        private IEnumerator LoadSprite()
        {
            m_IsLoad = false;

            AssetBundleCreateRequest avitar = AssetBundle.LoadFromFileAsync(GetABPath(2));
            yield return avitar;
            while (!avitar.isDone)
            {
                yield return null;
            }

            if (avitar != null)
            {
                m_SpriteAB = avitar.assetBundle;
            }

            m_IsLoad = true;
            yield return m_IsLoad;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (m_AvitarAB != null)
            {
                m_AvitarAB.Unload(true);
            }
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