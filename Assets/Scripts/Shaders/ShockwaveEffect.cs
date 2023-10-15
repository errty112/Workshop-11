using UnityEngine;

[RequireComponent(typeof(GameTime))]
public class ShockwaveEffect : MonoBehaviour, IGlobalShaderEffect
{
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private float amplitude = 1.0f;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float duration = 1.0f;

    private GameTime _gameTime;

    // Dynamic arrays are not supported in Unity's shader system, so we need to
    // specify a maximum number of shockwaves that can be active at any time.
    // See the respective Shockwave.cginc file for the corresponding constant.
    public const int MaxShockwaves = 10;  

    private static int _nextShockwaveIndex = 0;
    private static Vector4[] _shockwavePositions = new Vector4[MaxShockwaves];
    private static float[] _shockwaveAmplitudes = new float[MaxShockwaves];
    private static float[] _shockwaveSpeeds = new float[MaxShockwaves];
    private static float[] _shockwaveStartTimes = new float[MaxShockwaves];
    private static float[] _shockwaveEndTimes = new float[MaxShockwaves];

    private void Start()
    {
        this._gameTime = GetComponent<GameTime>();

        if (playOnStart) Play();
    }
    public void Play()
    {
        // Create a shockwave at the player's current position.
        Create(transform.position, amplitude, speed, duration, this._gameTime.Time);
    }

    private static void Create(
        Vector3 position, 
        float amplitude = 1.0f,
        float speed = 1.0f,
        float duration = 1.0f,
        float startTime = 0.0f)
    {
        // Use the next available index in the shockwave arrays, or overwrite
        // the oldest shockwave if all slots are full (this is known as a
        // circular buffer).
        int index = _nextShockwaveIndex;
        _nextShockwaveIndex = (_nextShockwaveIndex + 1) % MaxShockwaves;

        // Set the shockwave parameters.
        _shockwavePositions[index] = position;
        _shockwaveAmplitudes[index] = amplitude;
        _shockwaveSpeeds[index] = speed;
        _shockwaveStartTimes[index] = startTime;
        _shockwaveEndTimes[index] = startTime + duration;

        // Update the global shader variables. This technique does not require
        // constant CPU->GPU updates since the start and end times are used in
        // the shader to determine whether a shockwave is active.
        Upload();
    }

    private static void Upload()
    {
        // Upload current shockwave parameters to the GPU to make them
        // accessible in shaders globally (across all materials).
        Shader.SetGlobalVectorArray("_ShockwavePositions", _shockwavePositions);
        Shader.SetGlobalFloatArray("_ShockwaveAmplitudes", _shockwaveAmplitudes);
        Shader.SetGlobalFloatArray("_ShockwaveSpeeds", _shockwaveSpeeds);
        Shader.SetGlobalFloatArray("_ShockwaveStartTimes", _shockwaveStartTimes);
        Shader.SetGlobalFloatArray("_ShockwaveEndTimes", _shockwaveEndTimes);
    }

    public static void InitializeShaderEffect()
    {
        for (int i = 0; i < MaxShockwaves; i++)
        {
            _shockwaveStartTimes[i] = -1.0f;
            _shockwaveEndTimes[i] = -1.0f;
            _shockwaveAmplitudes[i] = 0.0f;
        }

        Upload();
    }
}
