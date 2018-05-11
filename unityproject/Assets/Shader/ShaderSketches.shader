Shader "ShaderSketches/Template"
{
   Properties
   {
       _MainTex ("MainTex", 2D) = "white"{}
   }

   CGINCLUDE
   #include "UnityCG.cginc"

   float4 frag(v2f_img i) : SV_Target
   {
       //return float4(i.uv.x, i.uv.y, 0, 1);

        float d = distance(float2(0.5f, 0.5f), i.uv);   // 距離で変化
        // float a = 0.4f;  // 閾値
        /*
        float a = abs(sin(_Time.y)) * 0.4; // 閾値
        return step(a, d);  // b が a 以上なら 1。 a より小さければ 0
        */
        /*
        d = d * 30;
        d = abs(sin(d));
        //d = step(0.5, d);

        float a = abs(sin(_Time.y * 2)) * 0.4; // 閾値
        return step(a, d);  // b が a 以上なら 1。 a より小さければ 0
        */

        {
            float2 st = 0.5 - i.uv;
            return distance(0, st);
        }
   }

   ENDCG

   SubShader
   {
       Pass
       {
           CGPROGRAM
           #pragma vertex vert_img
           #pragma fragment frag
           ENDCG
       }
   }
}
