// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2023

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SceneManager))]
public class GameManager : MonoBehaviour
{
    public const string Tag = "GameManager";
    
    public const string MenuSceneName = "MenuScene";
    public const string GameSceneName = "GameScene";
    
    private int _score;
    private float _gameTime;

    public UnityEvent<int> OnScoreChanged { get; } = new();
    public int Score
    {
        get => this._score;
        set
        {
            this._score = value;
            OnScoreChanged.Invoke(this._score);
        }
    }

    public float GameTime
    {
        get => this._gameTime;
        private set => this._gameTime = value;
    }

    private void Awake()
    {
        // Should not be created if there's already a manager present (at least
        // two total, including ourselves). This allows us to place a game
        // manager in every scene, in case we want to open scenes direct.
        if (GameObject.FindGameObjectsWithTag(Tag).Length > 1)
            Destroy(gameObject);

        // Make this game object persistent even between scene changes.
        DontDestroyOnLoad(gameObject);
        
        // Hook into scene loaded events.
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Init global game state values and/or set defaults.
        Score = 0;
    }

    private void Update()
    {
        // Update game time if we're in the game scene.
        if (SceneManager.GetActiveScene().name == GameSceneName)
            this._gameTime += Time.deltaTime;
    }
    
    public IEnumerator GotoScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);

        var asyncLoadOp = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoadOp.isDone)
        {
            yield return null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == GameSceneName)
        {
            Score = 0; // Reset score upon game start
            GameTime = 0.0f; // Also reset game time upon game start
        }
    }
}
