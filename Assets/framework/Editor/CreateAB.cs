/*
 *  create file time:1/11/2018 3:16:31 PM
 *  Describe:打包资源
* */

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Framework.Engine.Art;
using System.IO;

public class CreateAB : EditorWindow
{
    private static BuildTarget m_BuildTarget;
    private static string m_ABSourcePath;
    private static string m_ABSavePath;
    private static string m_ABShareName;

    [MenuItem("Framework/CreateAB &B")]
    private static void Create()
    {
        EditorWindow.GetWindowWithRect(typeof(CreateAB), new UnityEngine.Rect(0, 0, 800, 300));
        m_BuildTarget = BuildTarget.StandaloneWindows;
        m_ABSourcePath = string.Format("{0}/{1}", Application.dataPath, "ABFile");
        m_ABSavePath = string.Format("{0}/{1}", Application.streamingAssetsPath, "ABFile");
        m_ABShareName = "ShareAB";
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("PC平台", GUILayout.Width(100)))
        {
            m_BuildTarget = BuildTarget.StandaloneWindows;
        }

        if (GUILayout.Button("Android", GUILayout.Width(100)))
        {
            m_BuildTarget = BuildTarget.Android;
        }

        if (GUILayout.Button("IOS", GUILayout.Width(100)))
        {
            m_BuildTarget = BuildTarget.iOS;
        }

        GUILayout.EndHorizontal();

        GUILayout.Label(string.Format("Platform : {0}", m_BuildTarget.ToString()));

        m_ABSourcePath = EditorGUILayout.TextField("AB包的路径", m_ABSourcePath);
        m_ABSavePath = EditorGUILayout.TextField("AB包保存路径", m_ABSavePath);
        m_ABShareName = EditorGUILayout.TextField("共用资源包名称", m_ABShareName);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("设置ab包名字", GUILayout.Width(100)))
        {
            SetABName();
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("开始打包", GUILayout.Width(100)))
        {
            ClearSaveFile();

            BuildPipeline.BuildAssetBundles(m_ABSavePath, BuildAssetBundleOptions.ChunkBasedCompression, m_BuildTarget);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("提示", "打包完成", "OK");
        }
    }

    private void SetABName()
    {
        DirectoryInfo abInfo = new DirectoryInfo(m_ABSourcePath);
        if (!abInfo.Exists)
        {
            abInfo.Create();
        }

        foreach (var item in abInfo.GetDirectories())
        {
            SetABNameScend(item.FullName);
        }
    }

    private void SetABNameScend(string path)
    {
        EditorUtility.DisplayProgressBar(string.Format("设置[{0}]AB名称", path), "正在设置……", 0f);

        string abName = path.Substring(path.LastIndexOf("\\") + 1);
        Debug.Log(abName);

        FileInfo[] fs = (new DirectoryInfo(path)).GetFiles();
        int length = fs.Length;
        for (int index = 0; index < length; index++)
        {
            EditorUtility.DisplayProgressBar(string.Format("设置[{0}]AB名称", path), "正在设置……", (index + 1) / length * 1f);

            string file = fs[index].Name;
            if (!file.EndsWith(".meta"))
            {
                //string name = file.Substring(file.LastIndexOf("\\") + 1);
                //name = name.Substring(0, name.LastIndexOf("."));
                string filePath = fs[index].FullName.Replace("\\", "/");
                filePath = "Assets" + filePath.Substring(Application.dataPath.Length);
                AssetImporter importer = AssetImporter.GetAtPath(filePath);
                if (importer)
                {
                    importer.assetBundleName = abName;
                    //importer.SetAssetBundleNameAndVariant(abName,"ab");
                    SetDepends(filePath, abName);
                }
            }
        }

        EditorUtility.DisplayCancelableProgressBar("设置ab名称", "设置完成", 1f);
        EditorUtility.ClearProgressBar();
    }

    private void SetDepends(string path, string name)
    {
        string[] dependObjs = AssetDatabase.GetDependencies(path);
        foreach (var i in dependObjs)
        {
            if (!i.Equals(path) && !i.EndsWith(".cs"))
            {
                AssetImporter dp = AssetImporter.GetAtPath(i);
                if (dp != null)
                {
                    if (dp.assetBundleName.Equals(string.Empty))
                    {
                        dp.assetBundleName = name;
                        //dp.SetAssetBundleNameAndVariant(name, "ab");
                    }
                    else
                    {
                        if (!dp.assetBundleName.Equals(m_ABShareName) && !dp.assetBundleName.Equals(name))
                        {
                            dp.assetBundleName = m_ABShareName;
                            //dp.SetAssetBundleNameAndVariant(m_ABShareName, "ab");
                        }
                    }
                }

                SetDepends(i, name);
            }
        }
    }

    private void ClearSaveFile()
    {
        DirectoryInfo save = new DirectoryInfo(m_ABSavePath);
        if (!save.Exists)
        {
            save.Create();
        }

        FileInfo[] files = save.GetFiles();
        foreach (var item in files)
        {
            item.Delete();
        }
    }
}
