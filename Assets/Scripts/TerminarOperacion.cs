using UnityEngine;

public class TerminarOperacion : MonoBehaviour
{
    // Llama a este método cuando termines la operación
    public void Terminar()
    {
        Escenas.Volver();
    }

    // Solo para probar: presiona Enter para volver
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            Terminar();
    }
}