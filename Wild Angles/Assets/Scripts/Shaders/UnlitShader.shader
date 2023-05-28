Shader"Universal Render Pipeline/Unlit/LessonShader"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _MainTex("Texture", 2D) = "white" {}
        _DetailTex("Detail Texture", 2D) = "gray" {}
        _DissolveTexture("Dissolve Texture", 2D) = "white" {}
        _DissolveCutOff("Dissolve Cutoff", Range(0, 1)) = 0
        _ExtrudeAmount("Extrude Amount", float) = 0
        _DeformTex("Deform Texture", 2D) = "gray" {}
        _Num1("Number 1", float) = 0
        _Num2("Number 2", float) = 0
        _Num3("Number 3", float) = 0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

#include "UnityCG.cginc"

struct VertexData
{
    float4 position : POSITION;
    float2 uv : TEXCOORD0;
    float3 normals : NORMAL;
};

struct VertexToFragment
{
    float4 position : SV_POSITION;
    float3 localPosition : TEXCOORD0;
    float3 normals : NORMAL;
    float2 uv : TEXCOORD1;
    float2 uvDetail : TEXCOORD2;
};

float4 _Color;
sampler2D _MainTex, _DetailTex, _DissolveTexture, _DeformTex;
float4 _MainTex_ST, _DetailTex_ST;
float _DissolveCutOff, _Num1, _Num2, _Num3, _ExtrudeAmount;

VertexToFragment vert(VertexData vertData)
{
    VertexToFragment v2f;
    v2f.localPosition = vertData.position.xyz;
                
    v2f.normals = vertData.normals;
    v2f.uv = vertData.uv * _MainTex_ST.xy + _MainTex_ST.zw;
    v2f.uvDetail = vertData.uv * _DetailTex_ST.xy + _DetailTex_ST.zw;
    float4 deformColor = tex2Dlod(_DeformTex, float4(vertData.uv.xy, 0, 0));
                //vertData.position.x *= deformColor.x;
    vertData.position.y += deformColor.r;
    vertData.position.x += sin((_Time.z * _Num1) + (vertData.position.z * _Num2)) * _Num3;
                /*v2f.uv.y += _Time.y * 0.5;
                v2f.uv.x += _Time.y * 0.5;
                v2f.uvDetail.y += _Time.y * 0.5;
                v2f.uvDetail.x += _Time.y * 0.5;*/
                //vertData.position.xyz += vertData.normals.xyz * _ExtrudeAmount;
    v2f.position = UnityObjectToClipPos(vertData.position);
    return v2f;
}

float4 frag(VertexToFragment v2f) : SV_TARGET
{
    float4 color = tex2D(_MainTex, v2f.uv) * _Color;
    color *= tex2D(_DetailTex, v2f.uvDetail) * 2;
    float4 dissolveColor = tex2D(_DissolveTexture, v2f.uv);
    clip(dissolveColor.rgb - _DissolveCutOff * 0.37);
    return color;
                //color *= tex2D(_DetailTex, v2f.uvDetail) * 2;
                //return color;
                //return tex2D(_MainTex, v2f.uv) * _Color;
                //return float4(v2f.normals.xyz, 1);
                //return float4(v2f.localPosition + 0.5, 1) * _Color;
}
            ENDCG
        }
    }
}