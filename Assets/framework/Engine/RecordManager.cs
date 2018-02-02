/*
 *  create file time:1/18/2018 4:22:34 PM
 *  Describe:录音管理
* */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework.Engine
{
    public struct VolumeInfo
    {
        public float m_MinVolume;
        public float m_MaxVolume;
        public float m_MidVolume;
        public float m_UnorderedMid;
        public float m_MeanVolume;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Min:{0} Max:{1} Mid:{2} UMid:{3} Mea:{4}",
                                m_MinVolume.ToString("F3"),
                                m_MaxVolume.ToString("F3"),
                                m_MidVolume.ToString("F3"),
                                m_UnorderedMid.ToString("F3"),
                                m_MeanVolume.ToString("F3"));
        }
    }

    [RequireComponent(typeof(AudioSource))]
    public class RecordManager : BaseMonoSingleton<RecordManager>
    {
        private AudioSource m_ASNode;
        private AudioClip m_ACNode;

        private bool m_IsSuccess;

        /// <summary>
        /// 采样率
        /// </summary>
        private const int m_SamplingRate = 44100;

        [Tooltip("录音时长")]
        [SerializeField]
        private int m_RecordTime = 1000;

        public override bool Initilize()
        {
            if (!base.Initilize())
                return false;

            m_ASNode = this.gameObject.GetComponent<AudioSource>();
            if (Microphone.devices.Length <= 0)
            {
                m_IsSuccess = false;
                Debug.LogError("can not find audio device.");
            }
            else
            {
                m_IsSuccess = true;
            }

            m_ASNode.loop = true;

            return true;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            StopRecord();
        }

        public void StartRecord()
        {
            if (m_IsSuccess)
            {
                Debug.Log("Start");
                StopRecord();

                m_ACNode = Microphone.Start(null, false, m_RecordTime, m_SamplingRate);
                while (Microphone.GetPosition(null) <= 0) { }
                m_ASNode.clip = m_ACNode;
                m_ASNode.Play();
            }
        }

        public void StopRecord()
        {
            if (m_IsSuccess)
            {
                Microphone.End(null);
                m_ASNode.Stop();
                //m_ACNode = null;
            }
        }

        public void PlayClip()
        {
            StopRecord();
            Debug.Log("Play");
            m_ASNode.Play();
        }

        public VolumeInfo GetVolume()
        {
            if (m_IsSuccess)
            {
                VolumeInfo info = new VolumeInfo();

                if (Microphone.IsRecording(null))
                {
                    int num = 128;
                    float[] waveData = new float[num];
                    int micPosition = Microphone.GetPosition(null) - (num + 1);
                    if (micPosition < 0)
                    {
                        return info;
                    }

                    m_ACNode.GetData(waveData, micPosition);
                    info.m_UnorderedMid = waveData[waveData.Length / 2];
                    float[] waveData2 = waveData.Where(x => x != 0).ToArray();
                    Array.Sort(waveData2);
                    if (waveData2.Length > 0)
                    {
                        info.m_MinVolume = waveData2[0];
                        info.m_MaxVolume = waveData2[waveData2.Length - 1];
                        info.m_MidVolume = waveData2[waveData2.Length / 2];

                        float sum = 0;
                        for (int index = 0; index < waveData2.Length; index++)
                        {
                            sum += Math.Abs(waveData2[index]);
                        }

                        info.m_MeanVolume = sum / waveData2.Length;
                    }
                }

                //if (m_ACNode.length >= m_RecordTime)
                //{
                //    StartRecord();
                //}

                return info;
            }

            return default(VolumeInfo);
        }
    }
}
