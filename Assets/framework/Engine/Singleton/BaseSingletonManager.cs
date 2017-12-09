/*
 *  create file time:12/9/2017 3:05:01 PM
 *  Describe:专门用来管理非Mono类的单例
* */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Engine
{
    public class BaseSingletonManager : BaseMonoSingleton
    {
        private List<BaseSingleton> m_AllBaseSingletons;

        public override bool Initilize()
        {
            if (!base.Initilize())
            {
                GameObject go = new GameObject();
                go.transform.parent = null;
                go.transform.position = Vector3.zero;
                go.transform.eulerAngles = Vector3.zero;
                go.transform.localScale = Vector3.one;

                m_Instance = go.AddComponent<BaseSingletonManager>();
            }

            m_AllBaseSingletons = new List<BaseSingleton>();
            m_AllBaseSingletons.Clear();

            return true;
        }

        public void AddSingleton(BaseSingleton instance)
        {

        }
    }
}
