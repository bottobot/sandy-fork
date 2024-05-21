#version 120

uniform sampler2DRect tex;
uniform float time;
uniform vec2 resolution;
uniform float feed;
uniform float kill;

void main(void)
{
    vec2 uv = gl_TexCoord[0].st / resolution;
    
    vec2 p = gl_FragCoord.xy / resolution.xy;
    vec4 color = texture2DRect(tex, gl_TexCoord[0].st);
    
    float dA = 1.0;
    float dB = 0.5;
    float f = feed;
    float k = kill;
    
    vec2 dx = vec2(1.0 / resolution.x, 0.0);
    vec2 dy = vec2(0.0, 1.0 / resolution.y);
    
    vec4 n = texture2DRect(tex, gl_TexCoord[0].st + dx);
    vec4 s = texture2DRect(tex, gl_TexCoord[0].st - dx);
    vec4 e = texture2DRect(tex, gl_TexCoord[0].st + dy);
    vec4 w = texture2DRect(tex, gl_TexCoord[0].st - dy);
    
    vec4 c = texture2DRect(tex, gl_TexCoord[0].st);
    
    vec4 lap = (n + s + e + w - 4.0 * c);
    
    float A = c.r;
    float B = c.g;
    float dA_dt = dA * lap.r - A * B * B + f * (1.0 - A);
    float dB_dt = dB * lap.g + A * B * B - (k + f) * B;
    
    float dt = 1.0;
    A += dA_dt * dt;
    B += dB_dt * dt;
    
    gl_FragColor = vec4(A, B, 0.0, 1.0);
}
