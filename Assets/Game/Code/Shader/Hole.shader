Shader "Custom/Hole"
{
    SubShader
    {
        Tags { "Queue" = "Geometry-1" }
        LOD 200

        ColorMask 0
        ZWrite off

        Stencil
        {
            Ref 1
            Comp always
            Pass replace
        }

        ColorMask 0
        Pass{}
    }
}
