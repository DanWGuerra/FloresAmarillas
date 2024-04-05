using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    // Static reference to the AudioController instance
    public static GameObject instance;

    void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            // If not, set this instance as the singleton instance
            instance = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this GameObject
            Destroy(gameObject);
        }
    }

}
