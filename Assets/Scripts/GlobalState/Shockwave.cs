using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public float ShockwaveSpeed = 1.0f;
    public float ShockwaveAmplitude = 10.0f;
    public float ShockwaveDuration = 0.5f;
    public Shader ShockwaveShader;

    private List<ShockwaveInstance> activeShockwaves = new List<ShockwaveInstance>();
    private const int MAX_SHOCKWAVES = 10;

    public void TriggerShockwave()
    {
        if (activeShockwaves.Count < MAX_SHOCKWAVES)
        {
            Vector3 position = transform.position; // Using object's position
            float amplitude = ShockwaveAmplitude; // Using the public variable
            float duration = ShockwaveDuration; // Using the public variable

            activeShockwaves.Add(new ShockwaveInstance
            {
                Position = position,
                Amplitude = amplitude,
                StartTime = Time.time,
                EndTime = Time.time + duration
            });

            StartCoroutine(ShockwaveEffectCoroutine());
        }
    }

    private IEnumerator ShockwaveEffectCoroutine()
    {
        while (activeShockwaves.Count > 0)
        {
            UpdateShader();
            activeShockwaves.RemoveAll(s => Time.time > s.EndTime);
            yield return null;
        }

        ResetShader();
    }

    private void UpdateShader()
    {
        float[] amplitudes = new float[MAX_SHOCKWAVES];
        Vector4[] positions = new Vector4[MAX_SHOCKWAVES];
        float[] startTimes = new float[MAX_SHOCKWAVES];
        float[] endTimes = new float[MAX_SHOCKWAVES];

        for (int i = 0; i < activeShockwaves.Count; i++)
        {
            positions[i] = activeShockwaves[i].Position;
            amplitudes[i] = activeShockwaves[i].Amplitude;
            startTimes[i] = activeShockwaves[i].StartTime;
            endTimes[i] = activeShockwaves[i].EndTime;
        }

        Shader.SetGlobalFloat("_GameTime", Time.time);
        Shader.SetGlobalFloatArray("_ShockwaveAmplitudes", amplitudes);
        Shader.SetGlobalVectorArray("_ShockwavePositions", positions);
        Shader.SetGlobalFloatArray("_ShockwaveStartTimes", startTimes);
        Shader.SetGlobalFloatArray("_ShockwaveEndTimes", endTimes);
    }

    private void ResetShader()
    {
        Shader.SetGlobalFloat("_GameTime", -1);
        Shader.SetGlobalFloatArray("_ShockwaveAmplitudes", new float[MAX_SHOCKWAVES]);
        Shader.SetGlobalVectorArray("_ShockwavePositions", new Vector4[MAX_SHOCKWAVES]);
        Shader.SetGlobalFloatArray("_ShockwaveStartTimes", new float[MAX_SHOCKWAVES]);
        Shader.SetGlobalFloatArray("_ShockwaveEndTimes", new float[MAX_SHOCKWAVES]);
    }

    private class ShockwaveInstance
    {
        public Vector3 Position;
        public float Amplitude;
        public float StartTime;
        public float EndTime;
    }
}
