using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
public class FaseManager : MonoBehaviour
{
    public TMP_Text mensagemFase;

    public static FaseManager fm; // singleton
    [System.Serializable]
    public class Fase
    {
        public Transform spawnEntradaEsquerda;
        public Transform spawnEntradaDireita;
    }

    public Fase[] fases;

    private GameController gc;
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


    public void IrParaFase(int faseDestino, Transform player, bool veioDaDireita)
    {
        GameController.gc.ultimaEntradaFoiDireita = veioDaDireita;
        

        if (faseDestino < 0 || faseDestino >= fases.Length)
            return;

        GameController.gc.faseAtual = faseDestino;

        if (veioDaDireita)
            player.position = fases[faseDestino].spawnEntradaDireita.position;
        else
            player.position = fases[faseDestino].spawnEntradaEsquerda.position;

        GameController.gc.SalvarProgresso();
    }

    public void MostrarMensagem(string msg)
    {
        StopAllCoroutines();
        StartCoroutine(MostrarTemporario(msg));
    }

    IEnumerator MostrarTemporario(string msg)
    {
        mensagemFase.text = msg;
        mensagemFase.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        mensagemFase.gameObject.SetActive(false);
    }

    // ? Próxima fase
    public void ProximaFase(Transform player)
    {
        int proxima = GameController.gc.faseAtual + 1;

        IrParaFase(proxima, player, false);
    }

    public void FaseAnterior(Transform player)
    {
        int anterior = GameController.gc.faseAtual - 1;

        IrParaFase(anterior, player, true);
    }

}
