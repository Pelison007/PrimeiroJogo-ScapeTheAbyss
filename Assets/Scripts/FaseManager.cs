using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class FaseManager : MonoBehaviour
{
    public static FaseManager fm; // singleton

    public Transform[] spawnsFases;
    public int faseAtual;
    private GameController gc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        if (fm == null)
        {
            fm = this; // garante que fm sempre aponta para o objeto na cena
        }
        else
        {
            Destroy(gameObject); // evita múltiplos FaseManager
        }
        gc = GameController.gc;
    }
    void Start()
    {
        faseAtual = PlayerPrefs.GetInt("FaseAtual", 0);
    }
    public void IrParaFase(int faseDestino, Transform player)
    {
        if (faseDestino < 0 || faseDestino >= spawnsFases.Length)
        {
            Debug.LogError("Fase destino inválida: " + faseDestino);
            return;
        }

        if (spawnsFases[faseDestino] == null)
        {
            Debug.LogError("Spawn da fase " + faseDestino + " está vazio!");
            return;
        }
        faseAtual = faseDestino;
        player.position = spawnsFases[faseDestino].position;
    }

    // ? Próxima fase
    public void ProximaFase(Transform player)
    {
        IrParaFase(faseAtual + 1, player);
    }

    // ? Fase anterior
    public void FaseAnterior(Transform player)
    {
        IrParaFase(faseAtual - 1, player);
    }

}
