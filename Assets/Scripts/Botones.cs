using UnityEngine;

public class Botones : MonoBehaviour
{
    [SerializeField] private string escenaMenu = "Menu";

    // Conéctalo al botón Retry
    public void Retry()
    {
        Escenas.Reintentar();
    }

    // Conéctalo al botón Menu
    public void Menu()
    {
        Escenas.IrAlMenu(escenaMenu);
    }
}