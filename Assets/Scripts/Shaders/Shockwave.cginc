#define MAX_SHOCKWAVES 10 
#define PI 3.141592f

uniform float _GameTime;
uniform float3 _ShockwavePositions[MAX_SHOCKWAVES];
uniform float _ShockwaveAmplitudes[MAX_SHOCKWAVES];
uniform float _ShockwaveSpeeds[MAX_SHOCKWAVES];
uniform float _ShockwaveStartTimes[MAX_SHOCKWAVES];
uniform float _ShockwaveEndTimes[MAX_SHOCKWAVES];

float4 ComputeShockwaveDisplacement(float4 vertexPosition)
{
    float3 totalDisplacement = 0;

    for (int i = 0; i < MAX_SHOCKWAVES; i++)
    {
        if (_GameTime >= _ShockwaveStartTimes[i] && _GameTime <= _ShockwaveEndTimes[i])
        {
            float3 direction = vertexPosition.xyz - _ShockwavePositions[i];
            float distance = length(direction);
            float wavefront = _ShockwaveSpeeds[i] * (_GameTime - _ShockwaveStartTimes[i]);
            
            if (distance < wavefront)
            {
                float factor = sin(distance * PI / wavefront);
                float magnitude = _ShockwaveAmplitudes[i] * factor;
                totalDisplacement += normalize(direction) * magnitude;
            }
        }
    }

    return vertexPosition + float4(totalDisplacement, 0);
}
