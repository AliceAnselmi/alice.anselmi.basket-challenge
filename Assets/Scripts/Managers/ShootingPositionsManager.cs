using System.Collections.Generic;
using UnityEngine;

public class ShootingPositionsManager : MonoBehaviour
{
    static public ShootingPositionsManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    [SerializeField] private List<Transform> shootingPositions = new List<Transform>();
    private int currentIndex = 0;
    
    public Transform GetCurrentPosition()
    {
        if (shootingPositions.Count == 0)
        {
            return null;
        }
        return shootingPositions[currentIndex];
    }
    
    public Transform GetNextPosition()
    {
        if (shootingPositions.Count == 0) return null;

        currentIndex = (currentIndex + 1) % shootingPositions.Count; // Using modulo to loop back to first position
        return shootingPositions[currentIndex];
    }

    public void ResetPositions()
    {
        currentIndex = 0;
    }
}