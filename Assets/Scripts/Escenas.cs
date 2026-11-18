using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Escenas
{
    private static string escenaAnterior;
    private static string zonaPendiente;
    private static HashSet<string> zonasCompletadas = new HashSet<string>();

    // Se ejecuta SIEMPRE al darle Play, aunque Reload Domain esté desactivado
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reiniciar()
    {
        escenaAnterior = null;
        zonaPendiente = null;
        zonasCompletadas.Clear();
    }

    public static void IrAEscena(string escenaDestino, string idZona)
    {
        escenaAnterior = SceneManager.GetActiveScene().name;
        zonaPendiente = idZona;
        SceneManager.LoadScene(escenaDestino);
    }

    public static void Volver()
    {
        if (string.IsNullOrEmpty(escenaAnterior))
        {
            Debug.LogWarning("No hay escena anterior a la que volver.");
            return;
        }

        if (!string.IsNullOrEmpty(zonaPendiente))
            zonasCompletadas.Add(zonaPendiente);

        SceneManager.LoadScene(escenaAnterior);
    }

    public static bool EstaCompletada(string idZona)
    {
        return zonasCompletadas.Contains(idZona);
    }
}
