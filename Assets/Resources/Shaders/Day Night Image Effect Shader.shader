Shader "Hidden/Day Night Image Effect Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    CGINCLUDE
        #include "UnityCG.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }

        // The day and night textures used for the day-night cycle.
        sampler2D _ColorGradeDay;
        sampler2D _ColorGradeNight;

        // The lerp between the colour codes.
        fixed _LerpT;

    ENDCG

    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                // Base colour.
                fixed4 col = tex2D(_MainTex, i.uv);
               
                // The new color.
                fixed4 newCol = col;

                // The day colors.
                fixed4 dayColRed = tex2D(_ColorGradeDay, fixed2(col.r, 1.0F));
                fixed4 dayColGreen = tex2D(_ColorGradeDay, fixed2(col.g, 0.5F));
                fixed4 dayColBlue = tex2D(_ColorGradeDay, fixed2(col.b, 0.0F));

                // The night colors.
                fixed4 nightColRed = tex2D(_ColorGradeNight, fixed2(col.r, 1.0F));
                fixed4 nightColGreen = tex2D(_ColorGradeNight, fixed2(col.g, 0.5F));
                fixed4 nightColBlue = tex2D(_ColorGradeNight, fixed2(col.b, 0.0F));

                // Mixes the day colors and night colors using lerp T.
                newCol.r = lerp(dayColRed.r, nightColRed.r, _LerpT);
                newCol.g = lerp(dayColGreen.g, nightColGreen.g, _LerpT);
                newCol.b = lerp(dayColBlue.b, nightColBlue.b, _LerpT);
                newCol.a = col.a; // Likely unneeded.

                // Returns the new color.
                return newCol;
            }
            ENDCG
        }
    }
}