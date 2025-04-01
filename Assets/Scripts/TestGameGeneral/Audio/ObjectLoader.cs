using UnityEngine;

public class ObjectLoader : MonoBehaviour
{
    public GameObject audioManagerPrefab; // Assign this in the Inspector

    void Awake()
    {
        if (AudioManager.Instance == null)
        {
            Instantiate(audioManagerPrefab);
        }
    }
}