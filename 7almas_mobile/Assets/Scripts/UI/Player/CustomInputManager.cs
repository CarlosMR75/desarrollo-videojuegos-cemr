using UnityEngine;

public class CustomInputManager : MonoBehaviour
{
    private bool isButtonPressed = false;

    // Método para simular el Input del Manager para cualquier botón que recibas
    public void SimularEntrada(string buttonName)
    {
        //isButtonPressed = true;  // Aquí puedes ajustar si quieres que el botón se active o no
        GetButtonDown(buttonName);
    }

    // Método que revisa si se presionó un botón
    public bool GetButtonDown(string buttonName)
    {
        if (isButtonPressed)
        {
            isButtonPressed = false; // Resetear después de ejecutar el comando
            return true; // Indica que el botón fue presionado
        }
        
        // Fallback al Input Manager tradicional (si no se ha presionado desde la UI)
        return Input.GetButtonDown(buttonName);
    }
}
