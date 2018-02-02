/*
 *  create file time:1/17/2018 8:59:37 AM
 *  Describe:UI面板
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Engine
{
    public partial class UIModule : MonoBehaviour
    {
        protected void Awake()
        {
            Initilize();
        }

        protected void OnDestroy()
        {
            DestroySelf();
        }

        protected virtual void Initilize()
        {
            m_IsShowed = false;
        }

        protected virtual void DestroySelf()
        {
            m_IsShowed = false;
        }
    }
}
