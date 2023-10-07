// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;

public abstract class GameManagerClient : MonoBehaviour
{
    // Only accessible by inheritors, to avoid messy access to global state.
    protected GameManager GameManager { get; private set; }

    private void Awake()
    {
        // Note: An alternative to this approach is to turn GameManager into a
        // singleton. However, this might be more prone to abuse as a "global
        // variable", since any script could mutate its state.
        GameManager = GameObject
            .FindWithTag(GameManager.Tag)
            .GetComponent<GameManager>();
    }
}
