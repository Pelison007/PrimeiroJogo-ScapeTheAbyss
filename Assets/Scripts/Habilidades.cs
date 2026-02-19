using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Habilidades : MonoBehaviour
{
    [SerializeField] private GameObject skillPrefab;

    public void CastSkill()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f;

        Instantiate(skillPrefab, mouseWorldPos, Quaternion.identity);
    }


}
