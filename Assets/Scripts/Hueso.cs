using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hueso : MonoBehaviour
{
    // Lista de huesos que siguen en la escena
    public static readonly List<Hueso> Todos = new List<Hueso>();
    public static bool QuedanHuesos => Todos.Count > 0;

    [Header("Uso")]
    [SerializeField] private string herramientaRequerida = "Jeringa";
    [SerializeField] private KeyCode tecla = KeyCode.E;
    [Tooltip("Señal tipo 'Presiona E' (opcional, hijo del hueso)")]
    [SerializeField] private GameObject senal;

    [Header("Desintegración")]
    [SerializeField] private float duracion = 1f;
    [Tooltip("Cuánto tiembla mientras se desintegra")]
    [SerializeField] private float temblor = 0.05f;
    [SerializeField] private UnityEvent alDesintegrarse;

    [Header("Identificación")]
    [Tooltip("Único por hueso (si lo dejas vacío usa el nombre del objeto)")]
    [SerializeField] private string idHueso = "";

    private Collider2D col;
    private SpriteRenderer sr;
    private bool desintegrando;
    private bool yaDestruido;

    private string IdFinal => idHueso != "" ? idHueso : gameObject.name;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reiniciar()
    {
        Todos.Clear();
    }

    void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        if (senal != null) senal.SetActive(false);

        // Si ya lo destruí antes de salir de esta escena, no debe volver a aparecer
        if (Escenas.HuesoYaDestruido(IdFinal))
        {
            yaDestruido = true;
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        if (yaDestruido) return;
        Todos.Add(this);
    }

    void OnDisable()
    {
        Todos.Remove(this);
    }

    void Update()
    {
        if (desintegrando || yaDestruido) return;

        bool herramientaCerca = HayHerramientaCorrecta();

        if (senal != null && senal.activeSelf != herramientaCerca)
            senal.SetActive(herramientaCerca);

        if (herramientaCerca && Input.GetKeyDown(tecla))
            StartCoroutine(Desintegrar());
    }

    bool HayHerramientaCorrecta()
    {
        Herramienta herramienta = Herramienta.HerramientaActual;
        if (herramienta == null) return false;

        if (herramientaRequerida != "" &&
            herramienta.nombreHerramienta != herramientaRequerida)
            return false;

        Collider2D herramientaCol = herramienta.GetComponent<Collider2D>();
        return herramientaCol != null && col.Distance(herramientaCol).isOverlapped;
    }

    IEnumerator Desintegrar()
    {
        desintegrando = true;
        if (senal != null) senal.SetActive(false);
        col.enabled = false;

        // Se registra ya, para que cuente como destruido aunque cambies de escena a mitad
        Escenas.RegistrarHuesoDestruido(IdFinal);

        Vector3 posInicial = transform.position;
        Vector3 escalaInicial = transform.localScale;
        Color colorInicial = sr != null ? sr.color : Color.white;

        float t = 0f;
        while (t < duracion)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duracion);

            // Se encoge, se vuelve transparente y tiembla
            transform.localScale = escalaInicial * (1f - p * 0.5f);
            transform.position = posInicial + (Vector3)(Random.insideUnitCircle * temblor);

            if (sr != null)
            {
                Color c = colorInicial;
                c.a = 1f - p;
                sr.color = c;
            }

            yield return null;
        }

        alDesintegrarse.Invoke();
        Destroy(gameObject);
    }
}