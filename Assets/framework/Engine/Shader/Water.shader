Shader "Custom/Water"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_TimeV("TimeV",float) = 30.0
		_TimeV1("TimeV1",float) = 0
	}

		SubShader
		{
			Tags{ "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				//#pragma target 3.0
				// make fog work
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					//UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;

				///使用TRANSFORM_TEX必须要定义的内容
				float4 _MainTex_ST;
				float _TimeV;
				float _TimeV1;

				v2f vert(appdata v)
				{
					v2f o;

					//UNITY_MATRIX_MVP 当前模型视图投影矩阵
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					//UNITY_TRANSFER_FOG(o,o.vertex);
					//#if UNITY_UV_STARTS_AT_TOP
					// float texcoord_y=1-i.texcoord.y;
					//#else
					// float texcoord_y=i.texcoord.y;
					//#endif
					o.uv.y = 1 - o.uv.y;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					fixed2 center = fixed2(0.5,0.5);
					fixed2 uv = i.uv - center;

					fixed rMax = length(0 - center);
					_TimeV1 = rMax;
					/*printf("%d", rMax);*/
					fixed tMax = 5;

					//length(v)返回一个向量的模，也相当于sqrt(dot(v,v))
					fixed rr = length(uv);

					//利用_TimeV来控制波的衰减程度,数值越小，衰减程度越大，数越大，衰减程度越小
					//fixed rrr = rr * 30 * (1 - 0.3*rr / rMax);//这儿*(1- 0.3*rr/rMax) 体现出波的周期衰减
					fixed rrr = rr * _TimeV * (1 - 0.3*rr / rMax);//这儿*(1- 0.3*rr/rMax) 体现出波的周期衰减

					//_Time 是一个四维数，表达式为（t/20,t,t*2,t*3），也就是由这么一个数字组成的,其中t代表的是时间
					fixed xx = rrr - _Time.y;// fmod(rrr -_TimeV,2);//
					fixed sinA = uv.y / rr;
					fixed cosA = uv.x / rr;

					//sin(A*x*PI); 周期为At = 2/A；用来控制波浪与波浪之间的间距的
					fixed At = 2;
					fixed A = 1;

					///fmod（x，y）,这个函数是用来返回x/y的余数，如果y为0，结果不可预料
					fixed ss = fmod(xx,At) - 0.5*At;
					fixed sinYy = sin(A*3.14159*ss);

					fixed4 col;
					rr += 0.05*sinYy*(1 - 0.9*rr / rMax);//这儿*(1- 0.9*rr/rMax) 体现出波的高度衰减
					fixed2 uvn = fixed2(cosA,sinA)*rr;
					col = tex2D(_MainTex, uvn + center);
					//ss = fmod(dis,2.0); 
					if (sinYy >= 0)
					{
						//col += fixed4(0,0.8,0,0);
					}
					else
					{
						//col += fixed4(0.8,0.0,0,0);
					}

					return col;
				}
				ENDCG
			}
		}
			FallBack "Diffuse"
}
