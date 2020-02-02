Shader "Custom/TransparentShader" 
{
	SubShader{
		// draw after all opaque objects (queue = 2001):
		Tags { "Queue" = "Overlay" }
		Pass {
		  Blend Zero One // keep the image behind it
		}
	}
}