using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tiempo : MonoBehaviour
{
    [Header("Tiempo")]
    [SerializeField] private float tiempoTotal = 60f;
    [SerializeField] private bool iniciarAutomaticamente = true;

    [Header("Barra")]
    [Tooltip("Imagen con Image Type = Filled")]
    [SerializeField] private Image barra;
    [Tooltip("Color según el tiempo: izquierda = sin tiempo, derecha = lleno")]
    [SerializeField] private Gradient colorBarra;

    [Header("Texto (opcional)")]
    [SerializeField] private TextMeshProUGUI textoTiempo;

    [Header("Al terminar")]
    [SerializeField] private string escenaGameOver = "GameOver";

    private float tiempoRestante;
    private bool corriendo;

    void Reset()
    {
        // Gradiente por defecto: rojo (vacío) -> amarillo -> verde (lleno)
        colorBarra = new Gradient();
        colorBarra.SetKeys(
            new[]
            {
                new GradientColorKey(Color.red, 0f),
                new GradientColorKey(Color.yellow, 0.5f),
                new GradientColorKey(Color.green, 1f)
            },
            new[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            });
    }

    void Start()
    {
        tiempoRestante = tiempoTotal;
        corriendo = iniciarAutomaticamente;
        ActualizarVisual();
    }

    void Update()
    {
        if (!corriendo) return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            corriendo = false;
            ActualizarVisual();
            SceneManager.LoadScene(escenaGameOver);
            return;
        }

        ActualizarVisual();
    }

    void ActualizarVisual()
    {
        float porcentaje = tiempoRestante / tiempoTotal;

        if (barra != null)
        {
            barra.fillAmount = porcentaje;
            if (colorBarra != null)
                barra.color = colorBarra.Evaluate(porcentaje);
        }

        if (textoTiempo != null)
        {
            int segundos = Mathf.CeilToInt(tiempoRestante);
            textoTiempo.text = (segundos / 60).ToString("00") + ":" +
                               (segundos % 60).ToString("00");
        }
    }

    // --- Métodos públicos (puedes llamarlos desde otros scripts o eventos) ---

    public void Iniciar() => corriendo = true;
    public void Detener() => corriendo = false;

    public void RestarTiempo(float segundos)
    {
        tiempoRestante = Mathf.Max(0f, tiempoRestante - segundos);
    }

    public void SumarTiempo(float segundos)
    {
        tiempoRestante = Mathf.Min(tiempoTotal, tiempoRestante + segundos);
    }
}