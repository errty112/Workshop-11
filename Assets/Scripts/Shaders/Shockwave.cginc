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
        // If the shockwave is not active at this time, skip it.
        if (_GameTime < _ShockwaveStartTimes[i] || _GameTime > _ShockwaveEndTimes[i])
            continue;

        // Get local time for this shockwave (time since "creation").
        float localTime = _GameTime - _ShockwaveStartTimes[i];

        // Get normalised local time for this shockwave, where 0.0 is the 
        // start time and 1.0 is the end time. This is used to fade out the
        // shockwave as it reaches the end of its lifetime.
        float normTime  = localTime / (_ShockwaveEndTimes[i] - _ShockwaveStartTimes[i]);
        
        // Calculate various values needed for the displacement computation.
        float  dist      = length(vertexPosition - _ShockwavePositions[i]);
        float3 dir       = normalize(vertexPosition - _ShockwavePositions[i]);

        float  powr      = _ShockwaveAmplitudes[i] * (1 - normTime);
        float  waveFront = _ShockwaveSpeeds[i] * localTime;
        float  hitFront  = step(dist, waveFront + PI / 2.0f); 

        // Compute the displacement of the vertex due to this shockwave.
        // There are many different ways that this can be done, but this is
        // one of the simplest that produces a 3D shockwave effect.
        float3 displacement = hitFront * powr * dir * cos(dist - waveFront);

        // Add this displacement to the total displacement.
        totalDisplacement += displacement;
    }

    return vertexPosition + float4(totalDisplacement, 0);
}

