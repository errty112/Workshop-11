using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

public class ShaderEffectsManager : MonoBehaviour
{
    [SerializeField] private GameTime _gameTime;
    
    private static ShaderEffectsManager _instance;
    private static List<Type> _shaderEffects;

    private void Start()
    {
        // Can't have more than one shader effects manager.
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        // Similar to the ViewportManager, we are using a singleton pattern to
        // reflect the fact that there is only ever one shader effects manager
        // in the scene (and enforce this). Given that custom shaders are used
        // in all scenes, this is necessary to ensure that the shader effects
        // are kept in sync and initialised from the start of the game.
        _instance = this;

        // By reflection find all classes that implement the IGlobalShaderEffect
        // interface and add them to the list of shader effects.
        _shaderEffects = new List<Type>();
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.GetInterfaces().Contains(typeof(IGlobalShaderEffect)))
                _shaderEffects.Add(type);
        }

        InitializeAllShaderEffects();
    }

    private void InitializeAllShaderEffects()
    {
        foreach (Type type in _shaderEffects)
        {
            MethodInfo method = type.GetMethod("InitializeShaderEffect");
            method?.Invoke(null, null);
        }
    }

    private void UpdateAllShaderEffects()
    {
        foreach (Type type in _shaderEffects)
        {
            MethodInfo method = type.GetMethod("UpdateShaderEffect");
            method?.Invoke(null, null);
        }
    }

    private void Update()
    {
        // Update shader time variable to keep shader effects in sync.
        Shader.SetGlobalFloat("_GameTime", _gameTime.Time);

        UpdateAllShaderEffects();
    }
}
