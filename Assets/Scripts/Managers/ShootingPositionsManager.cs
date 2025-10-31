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
        SetPositionOccupied(playerToMove.positionIndex, false);
        if(!GetPositionOccupied((playerToMove.positionIndex +1)%shootingPositions.Count))
        {
            playerToMove.positionIndex=(playerToMove.positionIndex +1)%shootingPositions.Count;
        } else
        {
            playerToMove.positionIndex+=(playerToMove.positionIndex +2)%shootingPositions.Count;
        }
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