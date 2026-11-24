using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Oxigeno : MonoBehaviour
{
    [Header("Oxígeno")]
    [SerializeField] private float oxigenoMaximo = 60f;
    [Tooltip("Cuánto oxígeno se gasta por segundo")]
    [SerializeField] private float consumoPorSegundo = 1f;
    [SerializeField] private bool iniciarAutomaticamente = true;

    [Header("Recarga")]
    [SerializeField] private int recargasDisponibles = 1;
    [SerializeField] private KeyCode teclaRecarga = KeyCode.R;
    [Tooltip("Cuánto oxígeno devuelve (0 = recarga hasta el máximo)")]
    [SerializeField] private float cantidadRecarga = 0f;
    [Tooltip("Se muestra solo mientras te quede una recarga (opcional)")]
    [SerializeField] private GameObject indicadorRecarga;
    [SerializeField] private UnityEvent alRecargar;

    [Header("Barra")]
    [Tooltip("Imagen con Image Type = Filled")]
    [SerializeField] private Image barra;
    [SerializeField] private Gradient colorBarra;

    [Header("Texto (opcional)")]
    [SerializeField] private TextMeshProUGUI textoOxigeno;

    [Header("Al agotarse")]
    [SerializeField] private string escenaGameOver = "GameOver";

    private float oxigenoActual;
    private bool activo;

    public bool PuedeRecargar => recargasDisponibles > 0 && oxigenoActual > 0f;

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
        oxigenoActual = oxigenoMaximo;
        activo = iniciarAutomaticamente;
        ActualizarVisual();
    }

    void Update()
    {
        if (!activo) return;

        // Recargar con la tecla
        if (Input.GetKeyDown(teclaRecarga))
            Recargar();

        oxigenoActual -= consumoPorSegundo * Time.deltaTime;

        if (oxigenoActual <= 0f)
        {
            oxigenoActual = 0f;
            activo = false;
            ActualizarVisual();
            Escenas.IrAGameOver(escenaGameOver);
            return;
        }

        ActualizarVisual();
    }

    void ActualizarVisual()
    {
        float porcentaje = oxigenoActual / oxigenoMaximo;

        if (barra != null)
        {
            barra.fillAmount = porcentaje;
            if (colorBarra != null)
                barra.color = colorBarra.Evaluate(porcentaje);
        }

        if (textoOxigeno != null)
            textoOxigeno.text = Mathf.CeilToInt(porcentaje * 100f) + "%";

        if (indicadorRecarga != null)
            indicadorRecarga.SetActive(recargasDisponibles > 0);
    }

    // --- Métodos públicos (puedes llamarlos desde botones, zonas o eventos) ---

    public void Recargar()
    {
        if (!PuedeRecargar) return;

        recargasDisponibles--;

        if (cantidadRecarga <= 0f)
            oxigenoActual = oxigenoMaximo;
        else
            oxigenoActual = Mathf.Min(oxigenoMaximo, oxigenoActual + cantidadRecarga);

        alRecargar.Invoke();
        ActualizarVisual();
    }

    public void Iniciar() => activo = true;
    public void Detener() => activo = false;

    public void GastarOxigeno(float cantidad)
    {
        oxigenoActual = Mathf.Max(0f, oxigenoActual - cantidad);
    }
}