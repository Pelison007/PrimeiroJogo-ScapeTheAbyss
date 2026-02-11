using System.Collections;
using UnityEngine;
using static GotaSpawn;


public class AreaDeChuva : MonoBehaviour
{
    public GotaSpawn gotaSpawn;

    public float tempoChuva = 10f;
    public float pausaChuva = 8f;

    private bool PlayerInArea;
    private Coroutine routine;

    public GotaSpawn.IntensidadeChuva intensidadeDaArea = GotaSpawn.IntensidadeChuva.Media;


    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();

        if (player == null)
            return;

        if (!PlayerInArea)
        {
            PlayerInArea = true;
            routine = StartCoroutine(ChuvaCiclo());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        if (player == null)
            return;

        PlayerInArea = false;

        if (routine != null)
            StopCoroutine(routine);
        
        gotaSpawn.PararChuva();
    }

    IEnumerator ChuvaCiclo()
    {
        while (PlayerInArea)
        {
            gotaSpawn.InicioChuva(intensidadeDaArea);

            yield return new WaitForSeconds(tempoChuva);

            gotaSpawn.PararChuva();

            yield return new WaitForSeconds(pausaChuva);
        }
    }
}
