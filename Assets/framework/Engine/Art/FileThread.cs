/*
 *  create file time:1/13/2018 10:28:12 AM
 *  Describe:客户端保存文件流
* */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Framework.Engine.Art
{
    public class FileData
    {
        public string m_FileName;
        public string m_FileData;

        public FileData(string name, string data)
        {
            m_FileName = name;
            m_FileData = data;
        }

        public override bool Equals(object obj)
        {
            return (obj as FileData).m_FileName.Equals(this.m_FileName);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class FileThread : BaseMonoSingleton<FileThread>
    {
        private static Queue<FileData> m_ThreadData;
        private Thread m_FileThread;
        private string m_SaveFile;

        public override bool Initilize()
        {
            if (!base.Initilize())
                return false;

            m_ThreadData = new Queue<FileData>();
            m_ThreadData.Clear();

            //m_FileThread = new Thread(ThreadFunction);
            //m_FileThread.IsBackground = true;
            //m_FileThread.Start();

            StartCoroutine("ThreadFunction");

            m_SaveFile = Application.persistentDataPath;
            Debug.Log(m_SaveFile);

            return true;
        }

        public void SaveFile(FileData data)
        {
            if (!m_ThreadData.Contains(data))
            {
                Debug.Log("63");
                m_ThreadData.Enqueue(data);
            }
        }

        private IEnumerator ThreadFunction()
        {
            while (true)
            {
                if (m_ThreadData.Count > 0)
                {
                    FileData data = m_ThreadData.Dequeue();
                    Debug.Log(data);
                    if (data != null)
                    {
                        Debug.Log("75");
                        string path = string.Format("{0}/{1}", m_SaveFile, data.m_FileName);
                        if (File.Exists(@path))
                        {
                            File.Delete(@path);
                            yield return null;
                        }

                        FileStream fs = File.Open(@path, FileMode.OpenOrCreate);
                        yield return fs;

                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(data.m_FileData);

                        yield return sw;
                        sw.Close();
                        fs.Close();

                        Debug.Log("save close");
                        yield return null;
                    }
                }
                else
                {
                    yield return null;
                }
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            //m_FileThread.Abort();
            StopCoroutine("ThreadFunction");
            m_ThreadData.Clear();
        }
    }
}
