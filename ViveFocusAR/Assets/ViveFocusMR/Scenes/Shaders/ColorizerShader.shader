//MIT License

//Copyright(c) 2018-2019 Antony Vitillo(a.k.a. "Skarredghost")

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

//This shader has been written in a fast and dirty way. It is just for demonstration purposes

Shader "Custom/Colorizer" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_MagicVal("MagicVal", range(0, 10)) = 1
		_Color1("Color1", Color) = (1,1,1,1)
		_Color2("Color2", Color) = (1,1,1,1)
		_Color3("Color3", Color) = (1,1,1,1)
		_Color4("Color4", Color) = (1,1,1,1)
		_Color5("Color5", Color) = (1,1,1,1)
		_Color6("Color6", Color) = (1,1,1,1)
		_Color7("Color7", Color) = (1,1,1,1)				
	}
		SubShader{
		pass {
		Tags{ "LightMode" = "ForwardBase" }
			Cull off
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _Color3;
		fixed4 _Color4;
		fixed4 _Color5;
		fixed4 _Color6;
		fixed4 _Color7;
		float _MagicVal;
		float4 _MainTex_ST;
		struct v2f {
			float4 pos:SV_POSITION;
			float2 uv_MainTex:TEXCOORD0;
		};

		v2f vert(appdata_full v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv_MainTex = TRANSFORM_TEX(v.texcoord,_MainTex);
			return o;
		}
		float4 frag(v2f i) :COLOR
		{
			float3 col = tex2D(_MainTex, i.uv_MainTex).rgb / _MagicVal;
			float3 resCol;

			if (col.r < (1.0 / 8))
			{
				resCol.r = _Color1.r * col.r;
				resCol.b = _Color1.b * col.r;
				resCol.g = _Color1.g * col.r;				
			}
			else if (col.r < (2.0 / 8))
			{
				resCol.r = 8 * (_Color1.r * (2.0 / 8 - col.r) + _Color2.r * (col.r - 1.0 / 8));
				resCol.b = 8 * (_Color1.b * (2.0 / 8 - col.r) + _Color2.b * (col.r - 1.0 / 8));
				resCol.g = 8 * (_Color1.g * (2.0 / 8 - col.r) + _Color2.g * (col.r - 1.0 / 8));				
			}
			else if (col.r < (3.0 / 8))
			{
				resCol.r = 8 * (_Color2.r * (3.0 / 8 - col.r) + _Color3.r * (col.r - 2.0 / 8));
				resCol.b = 8 * (_Color2.b * (3.0 / 8 - col.r) + _Color3.b * (col.r - 2.0 / 8));
				resCol.g = 8 * (_Color2.g * (3.0 / 8 - col.r) + _Color3.g * (col.r - 2.0 / 8));
			}
			else if (col.r < (4.0 / 8))
			{
				resCol.r = 8 * (_Color3.r * (4.0 / 8 - col.r) + _Color4.r * (col.r - 3.0 / 8));
				resCol.b = 8 * (_Color3.b * (4.0 / 8 - col.r) + _Color4.b * (col.r - 3.0 / 8));
				resCol.g = 8 * (_Color3.g * (4.0 / 8 - col.r) + _Color4.g * (col.r - 3.0 / 8));
			}
			else if (col.r < (5.0 / 8))
			{
				resCol.r = 8 * (_Color4.r * (5.0 / 8 - col.r) + _Color5.r * (col.r - 4.0 / 8));
				resCol.b = 8 * (_Color4.b * (5.0 / 8 - col.r) + _Color5.b * (col.r - 4.0 / 8));
				resCol.g = 8 * (_Color4.g * (5.0 / 8 - col.r) + _Color5.g * (col.r - 4.0 / 8));
			}
			else if (col.r < (6.0 / 8))
			{
				resCol.r = 8 * (_Color5.r * (6.0 / 8 - col.r) + _Color6.r * (col.r - 5.0 / 8));
				resCol.b = 8 * (_Color5.b * (6.0 / 8 - col.r) + _Color6.b * (col.r - 5.0 / 8));
				resCol.g = 8 * (_Color5.g * (6.0 / 8 - col.r) + _Color6.g * (col.r - 5.0 / 8));
			}
			else if (col.r < (7.0 / 8))
			{
				resCol.r = 8 * (_Color6.r * (7.0 / 8 - col.r) + _Color7.r * (col.r - 6.0 / 8));
				resCol.b = 8 * (_Color6.b * (7.0 / 8 - col.r) + _Color7.b * (col.r - 6.0 / 8));
				resCol.g = 8 * (_Color6.g * (7.0 / 8 - col.r) + _Color7.g * (col.r - 6.0 / 8));
			}
			else
			{
				resCol.r = 8 * (_Color7.r * (8.0 / 8 - col.r) + 1 * (col.r - 7.0 / 8));
				resCol.b = 8 * (_Color7.b * (8.0 / 8 - col.r) + 1 * (col.r - 7.0 / 8));
				resCol.g = 8 * (_Color7.g * (8.0 / 8 - col.r) + 1 * (col.r - 7.0 / 8));
			}
		
			resCol = resCol * (col.r);
			return float4(resCol.r, resCol.g, resCol.b, 1);
			//return float4(_Color.r * len, _Color.g * len, _Color.b * len, 1);
		}
			ENDCG
	}//

	}
}