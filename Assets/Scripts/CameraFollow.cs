using UnityEngine;

public class CameraFollow : MonoBehaviour 
{
    
    [Header("Limites verticais")]
    public float followSpeed = 15f; // quanto maior, mais rápido a câmera segue
    public float minY = 0f;  // altura mínima da câmera (ex: chão)
    public float maxY = 5f;  // altura máxima da câmera (ex: teto)

    public Transform Player;      // quem a câmera vai seguir
    public Vector3 offset = new Vector3(0f, 3.16f, -10f); // distância da câmera em relação ao player
    private void FixedUpdate()
    {
        if (Player == null) return;

        // posição alvo
        Vector3 targetPos = Player.position + offset;

        // limita apenas o Y
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

        // suavidade
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
    // Teleporte instantâneo da câmera
    public void TeleportTo(Vector3 position)
    {
        Vector3 targetPos = position + offset;

        // aplica limites Y também
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

        transform.position = targetPos;
    }

}
