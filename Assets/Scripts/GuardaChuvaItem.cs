using UnityEngine;

public class GuardaChuvaItem : MonoBehaviour
{
    private bool playerNear = false;
    private Player player;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entrou na área do item");
            playerNear = true;
            player = other.GetComponent<Player>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player saiu da área do item");
            playerNear = false;
            player = null;
        }
    }
}
