using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Escenas
{
    private static string escenaAnterior;
    private static string zonaPendiente;
    private static string ultimoNivel;
    private static HashSet<string> zonasCompletadas = new HashSet<string>();
    private static HashSet<string> huesosDestruidos = new HashSet<string>();

    // Se ejecuta SIEMPRE al darle Play, aunque Reload Domain esté desactivado
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reiniciar()
    {
        ReiniciarProgreso();
        ultimoNivel = null;
    }

    // Borra todo el progreso del nivel (zonas y huesos)
    public static void ReiniciarProgreso()
    {
        escenaAnterior = null;
        zonaPendiente = null;
        zonasCompletadas.Clear();
        huesosDestruidos.Clear();
    }

    // Ir a otra escena recordando de dónde vengo y qué zona la activó
    public static void IrAEscena(string escenaDestino, string idZona)
    {
        escenaAnterior = SceneManager.GetActiveScene().name;
        zonaPendiente = idZona;
        SceneManager.LoadScene(escenaDestino);
    }

    // Volver a la escena anterior, marcando la zona como completada
    public static void Volver()
    {
        if (string.IsNullOrEmpty(escenaAnterior))
        {
            Debug.LogWarning("No hay escena anterior a la que volver.");
            return;
        }

        if (!string.IsNullOrEmpty(zonaPendiente))
            zonasCompletadas.Add(zonaPendiente);

        string destino = escenaAnterior;
        escenaAnterior = null;
        zonaPendiente = null;
        SceneManager.LoadScene(destino);
    }

    public static bool EstaCompletada(string idZona)
    {
        return zonasCompletadas.Contains(idZona);
    }

    // --- Huesos ---
    // La clave incluye el nombre de la escena para que no se mezclen entre niveles

    public static void RegistrarHuesoDestruido(string idHueso)
    {
        huesosDestruidos.Add(SceneManager.GetActiveScene().name + "/" + idHueso);
    }

    public static bool HuesoYaDestruido(string idHueso)
    {
        return huesosDestruidos.Contains(SceneManager.GetActiveScene().name + "/" + idHueso);
    }

    // --- Game Over, Retry y Menú ---

    // Úsalo en vez de SceneManager.LoadScene("GameOver")
    public static void IrAGameOver(string escenaGameOver)
    {
        // Si perdiste dentro de un minijuego, el nivel al que volver es el de origen
        ultimoNivel = !string.IsNullOrEmpty(escenaAnterior)
            ? escenaAnterior
            : SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(escenaGameOver);
    }

    public static void Reintentar()
    {
        if (string.IsNullOrEmpty(ultimoNivel))
        {
            Debug.LogWarning("No hay un nivel al que reintentar. " +
                             "Llegaste a GameOver sin pasar por un nivel.");
            return;
        }

        ReiniciarProgreso();
        SceneManager.LoadScene(ultimoNivel);
    }

    public static void IrAlMenu(string escenaMenu)
    {
        ReiniciarProgreso();
        ultimoNivel = null;
        SceneManager.LoadScene(escenaMenu);
    }
}