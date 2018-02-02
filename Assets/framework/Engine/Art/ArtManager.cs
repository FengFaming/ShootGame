/*
 *  create file time:1/11/2018 3:01:44 PM
 *  Describe:用来管理资源的
* */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.Engine;

namespace Framework.Engine.Art
{
    /// <summary>
    /// 主要是对外的接口
    /// </summary>
    public partial class ArtManager : BaseMonoSingleton<ArtManager>
    {
        public Sprite LoadIcon(string name)
        {
            if (m_UIIconAB != null)
            {
                GameObject icons = m_UIIconAB.LoadAsset<GameObject>(name);
                Sprite sp = (icons.GetComponent<Image>()).sprite;
                return sp;
            }
            else
            {
                StartLoadUIIcon();

                return LoadIcon(name);
            }
        }

        public GameObject LoadAvitar(string name)
        {
            if (m_AvitarAB != null)
            {
                GameObject icons = m_AvitarAB.LoadAsset<GameObject>(name + ".prefab");
                return icons;
            }
            else
            {
                StartLoadAvitar();
                return null;
            }
        }

        public Sprite LoadSprite(string name)
        {
            if (m_SpriteAB != null)
            {
                GameObject icons = m_SpriteAB.LoadAsset<GameObject>(name);
                Sprite sp = (icons.GetComponent<Image>()).sprite;
                return sp;
            }
            else
            {
                StartLoadSprite();

                return LoadSprite(name);
            }
        }
    }
}

