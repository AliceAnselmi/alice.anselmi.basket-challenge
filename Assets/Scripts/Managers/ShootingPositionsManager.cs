using System.Collections.Generic;
using UnityEngine;

public class ShootingPositionsManager : MonoBehaviour
{
    static public ShootingPositionsManager Instance { get; private set; }
    [SerializeField] private List<Transform> shootingPositions = new List<Transform>();
    
    private Dictionary<int, bool> positionOccupied = new Dictionary<int, bool>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public Transform MoveToNextPosition(MatchPlayer playerToMove)
    {
        int oldPositionIndex = playerToMove.positionIndex;
        int newPositionIndex = 0;
        SetPositionOccupied(oldPositionIndex, false);
        if(!GetPositionOccupied((oldPositionIndex + 1)%shootingPositions.Count))
        {
            newPositionIndex = (oldPositionIndex + 1)%shootingPositions.Count;
        } else
        {
            newPositionIndex = (oldPositionIndex + 2)%shootingPositions.Count;
        }
        playerToMove.positionIndex = newPositionIndex;
        SetPositionOccupied(playerToMove.positionIndex, true);
        return GetPositionAtIndex(playerToMove.positionIndex);
    }
    
    public Transform MoveToPositionAtIndex(MatchPlayer playerToMove, int index)
    {
        SetPositionOccupied(playerToMove.positionIndex, false);
        playerToMove.positionIndex = index;
        SetPositionOccupied(playerToMove.positionIndex, true);
        return GetPositionAtIndex(playerToMove.positionIndex);
    }
    
    private Transform GetPositionAtIndex(int index)
    {
        if (shootingPositions.Count == 0) return null;
        if (index < 0 || index >= shootingPositions.Count) return null;

        return shootingPositions[index];
    }
    
    private void SetPositionOccupied(int index, bool occupied)
    {
        if (index < 0 || index >= shootingPositions.Count) return;
        positionOccupied[index] = occupied;
    }
    
    private bool GetPositionOccupied(int index)
    {
        if (index < 0 || index >= shootingPositions.Count) return false;
        return positionOccupied.ContainsKey(index) && positionOccupied[index];
    }

}