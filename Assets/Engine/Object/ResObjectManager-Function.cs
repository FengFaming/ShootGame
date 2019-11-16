/*
 * Creator:ffm
 * Desc:游戏对象加载管理类对外接口
 * Time:2019/11/11 14:21:40
* */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Engine
{
	public partial class ResObjectManager : SingletonMonoClass<ResObjectManager>
	{
		/// <summary>
		/// 初始化或者说启用这个管理类
		/// </summary>
		/// <param name="manifest"></param>
		public void InitResManager(string manifest)
		{
			m_ManifestABName = manifest;
			WWW mani = new WWW(GetABPath(m_ManifestABName));
			if (mani != null && string.IsNullOrEmpty(mani.error))
			{
				m_Manifest = mani.assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

				ABInfo info = new ABInfo();
				info.m_ABName = m_ManifestABName;
				info.m_IsDontClear = true;
				info.m_LastUseTime = (long)Time.time;
				info.m_TargetAB = mani.assetBundle;
				info.m_UseCount = 0;

				m_AllABInfoDic.Add(info.m_ABName, info);
				m_IsInitSuccess = true;
			}
		}

		/// <summary>
		/// 加载资源
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="cb"></param>
		public void LoadObject(string name, ResObjectType type, IResObjectCallBack cb)
		{
			LoadResObjectInfo info = new LoadResObjectInfo();
			info.m_LoadName = name;
			info.m_LoadType = type;
			info.m_LoadCB.Add(cb);

			if (m_NeedLoadInfos.Contains(info))
			{
				for (int index = 0; index < m_NeedLoadInfos.Count; index++)
				{
					if (m_NeedLoadInfos[index].Equals(info))
					{
						m_NeedLoadInfos[index].m_LoadCB.Add(cb);
						break;
					}
				}
			}
			else
			{
				m_NeedLoadInfos.Add(info);
			}

			if (!m_IsLoad)
			{
				StartCoroutine("LoadYiedFunction");
			}
		}
	}
}
