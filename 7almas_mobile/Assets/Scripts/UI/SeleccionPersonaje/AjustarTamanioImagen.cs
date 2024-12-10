using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class AjustarTamanioImagen : MonoBehaviour
{
    private RectTransform rectTransform;
    private Image image;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        if (image != null && image.sprite != null)
        {
            AjustarTamanio();
        }
    }

    void AjustarTamanio()
    {
        // Obtener las dimensiones de la imagen
        float ancho = image.sprite.rect.width;
        float alto = image.sprite.rect.height;

        Debug.Log("Alto:"+alto);

        // Ajustar el tamanio del RectTransform
        rectTransform.sizeDelta = new Vector2(ancho, alto);
    }
}
