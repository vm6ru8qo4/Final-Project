// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shaders/PhongReflection"
{
    Properties
    {
        _Tex("Pattern", 2D) = "bump"{}
        _Color("Color",Color) = (1,1,1,1)
        _Ka("Ka",Color) = (1,1,1,1)
        _Kd("Kd",Color) = (1,1,1,1)
        _Ks("Ks",Color) = (1,1,1,1)
        //specular reflection
        _Shininess("Shininess", Float) = 10
        _SpecColor("Specular Color", Color) = (1,1,1,1)
    }
    
    SubShader
    {
        Tags {"RenderType" = "Opaque"}
        Pass{
            Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
                #pragma vertex vert//points
                #pragma fragment frag//colors
                #include "UnityCG.cginc"

                uniform float4 _LightColor0;
                float4 _Tex_ST;
                uniform float4 _Ka;
                uniform float4 _Kd;
                uniform float4 _Ks;

                sampler2D  _Tex;
                uniform float4 _Color;
                uniform float _Shininess;
                uniform float4 _SpecColor;

                struct appdata {//model
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };
                struct v2f {//screen:vertex to fragment
                    float4 pos : SV_POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                    float4 posWorld : TEXCOORD1;
                };
                v2f vert(appdata v) {
                    v2f o;

                    o.pos = UnityObjectToClipPos(v.vertex);//dot P V M v
                    o.normal = normalize(mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyz);//normalize(mul(float4(v.normal,0.0),unity_WorldToObject).xyz);
                    o.uv = TRANSFORM_TEX(v.uv, _Tex);
                    o.posWorld = mul(unity_ObjectToWorld,v.vertex);//dot M v

                    return o;
                }
                fixed4 frag(v2f i) : COLOR{//SV_Target{
                    float3 normalDirection = i.normal;
                    float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz - i.posWorld.xyz * _WorldSpaceLightPos0.w);
                    float3 viewDirection = normalize(_WorldSpaceCameraPos - i.posWorld.xyz);
                    float3 reflectDirection = reflect(-lightDirection, normalDirection);

                    //Ambient light
                    float3 ambientLighting = _Ka.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
                    //Diffuse light
                    float3 diffuseReflection = _Kd.rgb * _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection));
                    //Specular light
                    float3 specularReflection;
                    if (dot(normalDirection, lightDirection) < 0.0) {
                        specularReflection = float3(0, 0, 0);
                    }
                    else {
                        specularReflection = _Ks.rgb * _LightColor0.rgb * _SpecColor.rgb * pow(max(0.0, dot(reflectDirection, viewDirection)), _Shininess);
                    }
                    float3 color = (ambientLighting + diffuseReflection) * tex2D(_Tex, i.uv) + specularReflection;
                    return float4(color, 1.0);
                }

            ENDCG
        }
        Pass{
            Tags {"LightMode" = "ForwardAdd"}
            Blend One One
            CGPROGRAM
                #pragma vertex vert//points
                #pragma fragment frag//colors
                #include "UnityCG.cginc"

                uniform float4 _LightColor0;
                float4 _Tex_ST;
                uniform float4 _Ka;
                uniform float4 _Kd;
                uniform float4 _Ks;

                sampler2D  _Tex;
                uniform float4 _Color;
                uniform float _Shininess;
                uniform float4 _SpecColor;

                struct appdata {//model
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };
                struct v2f {//screen:vertex to fragment
                    float4 pos : SV_POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                    float4 posWorld : TEXCOORD1;
                };
                v2f vert(appdata v) {
                    v2f o;

                    o.pos = UnityObjectToClipPos(v.vertex);//dot P V M v
                    o.normal = normalize(mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyz);//normalize(mul(float4(v.normal,0.0),unity_WorldToObject).xyz);
                    o.uv = TRANSFORM_TEX(v.uv, _Tex);
                    o.posWorld = mul(unity_ObjectToWorld,v.vertex);//dot M v

                    return o;
                }
                fixed4 frag(v2f i) : COLOR{//SV_Target{
                    float3 normalDirection = i.normal;
                    float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz - i.posWorld.xyz * _WorldSpaceLightPos0.w);
                    float3 viewDirection = normalize(_WorldSpaceCameraPos - i.posWorld.xyz);
                    float3 reflectDirection = reflect(-lightDirection, normalDirection);

                    //Ambient light
                    float3 ambientLighting = _Ka.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
                    //Diffuse light
                    float3 diffuseReflection = _Kd.rgb * _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection));
                    //Specular light
                    float3 specularReflection;
                    if (dot(normalDirection, lightDirection) < 0.0) {
                        specularReflection = float3(0, 0, 0);
                    }
                    else {
                        specularReflection = _Ks.rgb * _LightColor0.rgb * _SpecColor.rgb * pow(max(0.0, dot(reflectDirection, viewDirection)), _Shininess);
                    }
                    float3 color = (ambientLighting + diffuseReflection) * tex2D(_Tex, i.uv) + specularReflection;
                    return float4(color, 1.0);
                }

            ENDCG
        }
    }
    //FallBack "Diffuse"
}
