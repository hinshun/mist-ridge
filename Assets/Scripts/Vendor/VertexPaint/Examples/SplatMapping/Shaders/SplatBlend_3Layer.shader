//
// If your looking for example code to make your own shaders, I suggest looking at the included examples. 
// This shader uses lots of compiler/C Macro abuse which is going to make it much harder to understand
// than the included examples. 

Shader "VertexPainter/SplatBlend_3Layer" 
{
   Properties {
      _Tex1 ("Albedo + Height", 2D) = "white" {}
      [NoScaleOffset][Normal]_Normal1("Normal", 2D) = "bump" {}
      _Glossiness1 ("Smoothness", Range(0,1)) = 0.5
      [NoScaleOffset]_GlossinessTex1("Metallic(R)/Smoothness(A)", 2D) = "black" {}
      _Metallic1 ("Metallic", Range(0,1)) = 0.0
      _Emissive1  ("Emissive", 2D) = "black" {}
      _EmissiveMult1("Emissive Multiplier", Float) = 1
      _Parallax1 ("Parallax Height", Range (0.005, 0.08)) = 0.02
      _TexScale1 ("Texture Scale", Float) = 1 
      
      
      _Tex2("Albedo + Height", 2D) = "white" {}
      [NoScaleOffset][Normal]_Normal2("Normal", 2D) = "bump" {}
      _Glossiness2 ("Smoothness", Range(0,1)) = 0.5
      [NoScaleOffset]_GlossinessTex2("Metallic(R)/Smoothness(A)", 2D) = "black" {}
      _Metallic2 ("Metallic", Range(0,1)) = 0.0
      _Emissive2  ("Emissive", 2D) = "black" {}
      _EmissiveMult2("Emissive Multiplier", Float) = 1
      _Parallax2 ("Parallax Height", Range (0.005, 0.08)) = 0.02
      _TexScale2 ("Texture Scale", Float) = 1
      _Contrast2("Contrast", Range(0,0.99)) = 0.5
      
      _Tex3("Albedo + Height", 2D) = "white" {}
      [NoScaleOffset][Normal]_Normal3("Normal", 2D) = "bump" {}
      _Glossiness3 ("Smoothness", Range(0,1)) = 0.5
      [NoScaleOffset]_GlossinessTex3("Metallic(R)/Smoothness(A)", 2D) = "black" {}
      _Metallic3 ("Metallic", Range(0,1)) = 0.0
      _Emissive3  ("Emissive", 2D) = "black" {}
      _EmissiveMult3("Emissive Multiplier", Float) = 1
      _Parallax3 ("Parallax Height", Range (0.005, 0.08)) = 0.02 
      _TexScale3 ("Texture Scale", Float) = 1
      _Contrast3("Contrast", Range(0,0.99)) = 0.5
      
      _FlowSpeed ("Flow Speed", Float) = 0
      _FlowIntensity ("Flow Intensity", Float) = 1
      
   }
   SubShader {
      Tags { "RenderType"="Opaque" }
      LOD 200
      
      CGPROGRAM
      
      // these are done with shader compile options - but honestly, on most modern hardware,
      // doing a branch would be fine since the branch would always go the same direction on
      // each pixel. If you are running low on keywords, that could be a viable option for you.
      #pragma surface surf Standard vertex:vert fullforwardshadows
      #pragma shader_feature __ _PARALLAXMAP
      #pragma shader_feature __ _NORMALMAP
      #pragma shader_feature __ _METALLICGLOSSMAP
      #pragma shader_feature __ _EMISSION
      // flow map keywords. 
      #pragma shader_feature __ _FLOW1 _FLOW2 _FLOW3
      #pragma shader_feature __ _FLOWDRIFT 
      #pragma target 3.0
      #include "/SplatBlend_Shared.cginc"
      
      void vert (inout appdata_full v, out Input o) 
      {
          SharedVert(v,o);
      }
      
      void surf (Input IN, inout SurfaceOutputStandard o) 
      {
         float2 uv1 = IN.uv_Tex1 * _TexScale1;
         float2 uv2 = IN.uv_Tex1 * _TexScale2;
         float2 uv3 = IN.uv_Tex1 * _TexScale3;
         INIT_FLOW
         #if _FLOWDRIFT || !_PARALLAXMAP 
         fixed4 c1 = FETCH_TEX1(_Tex1, uv1);
         fixed4 c2 = FETCH_TEX2(_Tex2, uv2);
         fixed4 c3 = FETCH_TEX3(_Tex3, uv3);
         #else
         fixed4 c1 = tex2D(_Tex1, uv1);
         fixed4 c2 = tex2D(_Tex2, uv2);
         fixed4 c3 = tex2D(_Tex3, uv3);
         #endif
         
         half b1 = HeightBlend(c1.a, c2.a, IN.color.r, _Contrast2);
         fixed h1 =  lerp(c1.a, c2.a, b1);
         half b2 = HeightBlend(h1, c3.a, IN.color.g, _Contrast3);
 
         #if _PARALLAXMAP
         float parallax = lerp(lerp(_Parallax1, _Parallax2, b1), _Parallax3, b2);
         float2 offset = ParallaxOffset (lerp(lerp(c1.a, c2.a, b1), c3.a, b2), parallax, IN.viewDir);
         uv1 += offset;
         uv2 += offset;
         uv3 += offset;
         c1 = FETCH_TEX1(_Tex1, uv1);
         c2 = FETCH_TEX2(_Tex2, uv2);
         c3 = FETCH_TEX3(_Tex3, uv3);
          #if (_FLOW1 || _FLOW2 || _FLOW3 || _FLOW4 || _FLOW5)
         fuv1 += offset;
         fuv2 += offset;
         #endif
         #endif
         
         fixed4 c = lerp(lerp(c1, c2, b1), c3, b2);
         
         #if _METALLICGLOSSMAP
         fixed4 g1 = FETCH_TEX1(_GlossinessTex1, uv1);
         fixed4 g2 = FETCH_TEX2(_GlossinessTex2, uv2);
         fixed4 g3 = FETCH_TEX3(_GlossinessTex3, uv3);
         fixed4 gf = lerp(lerp(g1, g2, b1), g3, b2);
         o.Smoothness = gf.a;
         o.Metallic = gf.r;
         #else
         o.Smoothness = lerp(lerp(_Glossiness1, _Glossiness2, b1), _Glossiness3, b2);
         o.Metallic = lerp(lerp(_Metallic1, _Metallic2, b1), _Metallic3, b2);
         #endif
         
         #if _EMISSION
         fixed4 e1 = FETCH_TEX1(_Emissive1, uv1);
         fixed4 e2 = FETCH_TEX2(_Emissive2, uv2);
         fixed4 e3 = FETCH_TEX3(_Emissive3, uv3);
         o.Emission = lerp(lerp(e1.rgb * _EmissiveMult1, 
                                e2.rgb * _EmissiveMult2, b1), 
                                e3.rgb * _EmissiveMult3, b2);
         #endif
         
         #if _NORMALMAP
         half4 n1 = (FETCH_TEX1 (_Normal1, uv1));
         half4 n2 = (FETCH_TEX2 (_Normal2, uv2));
         half4 n3 = (FETCH_TEX3 (_Normal3, uv3));
         o.Normal = UnpackNormal(lerp(lerp(n1, n2, b1), n3, b2));
         #endif
         
         o.Albedo = c.rgb;
         
      }
      ENDCG
   } 
   CustomEditor "SplatMapShaderGUI"
   FallBack "Diffuse"
}
