using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Test : MonoBehaviour
{
    private Material m_M;
    public int m_WaveWidth;
    public int m_WaveHeight;

    private float[,] m_WaveA;
    private float[,] m_WaveB;
    private Color[] m_ColorBuffer;

    private Texture2D m_TexUV;

    private bool m_IsRun;
    private int m_SleepTime;

    private Thread m_Thread;

    private void Awake()
    {
        Debug.Log("Awake");
        m_M = this.GetComponent<Renderer>().material;

        m_IsRun = true;
    }

    private void Start()
    {
        Debug.Log("Start");

        m_WaveA = new float[m_WaveWidth, m_WaveHeight];
        m_WaveB = new float[m_WaveWidth, m_WaveHeight];
        m_ColorBuffer = new Color[m_WaveWidth * m_WaveHeight];
        m_TexUV = new Texture2D(m_WaveWidth, m_WaveHeight);

        m_M.SetTexture("_WaveTex", m_TexUV);

        Thread t = new Thread(new ThreadStart(ComputeWave));
        t.Start();

        m_Thread = t;
    }

    private void ComputeWave()
    {
        while (m_IsRun)
        {
            for (int w = 1; w < m_WaveWidth - 1; w++)
            {
                for (int h = 1; h < m_WaveHeight - 1; h++)
                {
                    m_WaveB[w, h] = (m_WaveA[w - 1, h] +
                                    m_WaveA[w + 1, h] +
                                    m_WaveA[w, h - 1] +
                                    m_WaveA[w, h + 1] +
                                    m_WaveA[w - 1, h - 1] +
                                    m_WaveA[w + 1, h - 1] +
                                    m_WaveA[w - 1, h + 1] +
                                    m_WaveA[w + 1, h + 1]) / 4 - m_WaveB[w, h];

                    float value = m_WaveB[w, h];
                    if (value > 1)
                    {
                        m_WaveB[w, h] = 1;
                    }

                    if (value < -1)
                    {
                        m_WaveB[w, h] = -1;
                    }

                    float offset_u = (m_WaveB[w - 1, h] - m_WaveB[w + 1, h]) / 2;
                    float offset_v = (m_WaveB[w, h - 1] - m_WaveB[w, h + 1]) / 2;

                    float r = offset_u / 2 + 0.5f;
                    float g = offset_v / 2 + 0.5f;

                    m_ColorBuffer[w + m_WaveWidth * h] = new Color(r, g, 0);
                    m_WaveB[w, h] -= m_WaveB[w, h] * 0.0025f;
                }
            }

            float[,] temp = m_WaveA;
            m_WaveA = m_WaveB;
            m_WaveB = temp;

            Thread.Sleep(m_SleepTime);
        }
    }

    private void PutDrop(int x, int y)
    {
        int radius = 20;
        float dist;
        for (int index = -radius; index <= radius; index++)
        {
            for (int i = -radius; i < radius; i++)
            {
                if (((x + index >= 0) && (x + index < m_WaveWidth - 1)) && ((y + i >= 0) && (y + i < m_WaveHeight - 1)))
                {
                    dist = Mathf.Sqrt(index * index + i * i);
                    if (dist < radius)
                    {
                        m_WaveA[x + index, y + i] = Mathf.Cos(dist * Mathf.PI / radius);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(m_M.GetFloat("_TimeV1"));
            Vector2 t = new Vector2(0.5f, 0.5f);
            //sqrt(dot(v, v))
            Debug.Log(Mathf.Sqrt(Vector2.Dot(t, t)));
        }

        m_SleepTime = (int)(Time.deltaTime * 1000);
        m_TexUV.SetPixels(m_ColorBuffer);
        m_TexUV.Apply();

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                ///把世界坐标转换成图片的像素坐标
                Vector3 poss = transform.worldToLocalMatrix.MultiplyPoint(hit.point);
                int w = (int)((poss.x + 0.5) * m_WaveWidth);
                int h = (int)((poss.y + 0.5) * m_WaveHeight);
                PutDrop(w, h);
            }
        }
    }

    private void OnDestroy()
    {
        m_IsRun = false;

        if (m_Thread != null)
        {
            m_Thread.Abort();
            m_Thread = null;
        }
    }
}
