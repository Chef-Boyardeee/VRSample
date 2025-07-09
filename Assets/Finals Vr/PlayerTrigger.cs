using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Monster touched the player! GAME OVER.");
            GameManager.Instance.GameOver();
        }
        else
        {
            Debug.Log($"Triggered with: {other.name}");
        }
    }
}
