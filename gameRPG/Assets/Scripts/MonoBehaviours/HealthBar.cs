using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI;//Componentes Interfaz Gráfica 
 
public class HealthBar : MonoBehaviour 
{ 
    [HideInInspector] 
    public Player character;    //Referencia al jugador 
    public Image meterImage;    //Medidor Meter de la salud 
    public Text hpText;         //Texto en barra de salud 
 
    void Start() 
    { 
        character.hitPoints.value = 0; 
    } 
 
    void Update() 
    { 
      if (character != null) 
        { 
            //Modifica barra de salud 
            meterImage.fillAmount = character.hitPoints.value / character.maxHitPoints; 
            //Texto a mostrar 
            hpText.text = "HP:" + (meterImage.fillAmount * 100); 
        } 
    } 
}