using UnityEngine;
using UnityEngine.EventSystems;
public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip hoverSound;
    public AudioClip clickSound;

    // Update is called once per frame
    public void OnPointerEnter(PointerEventData eventData)
    {
        MusicController.mC.PlayUISound(hoverSound);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
        MusicController.mC.PlayUISound(clickSound);
    }
}
