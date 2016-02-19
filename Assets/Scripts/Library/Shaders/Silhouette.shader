Shader "Seethrough/Silhouette" {
  Properties {
    _Color ("Sihouette Color", Color) = (0,0,0,1)
  }
 
  SubShader {
    Tags { "Queue" = "Transparent" }
 
    Pass {
      Name "OUTLINE"
      Tags { "LightMode" = "Always" }
      Cull Off
      ZWrite Off
      ZTest Greater
      ColorMask RGB
      Blend SrcAlpha OneMinusSrcAlpha // Normal
 
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      struct appdata {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
      };
       
      struct v2f {
        float4 pos : POSITION;
        float4 color : COLOR;
      };
       
      uniform float4 _Color;
       
      v2f vert(appdata v) {
        v2f o;
        o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
       
        o.color = _Color;
        return o;
      }

      half4 frag(v2f i) :COLOR {
        return i.color;
      }

      ENDCG
    }
  }

  FallBack "Standard"
}
