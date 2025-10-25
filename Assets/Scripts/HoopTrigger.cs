using UnityEngine;

public class HoopTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("Ball entered the basket ring!");
            
            // Here update score (later)
        }
    }
}
