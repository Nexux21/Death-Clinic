using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Corazon : MonoBehaviour
{
    [Header("Círculos")]
    [SerializeField] private Circulos circuloPrefab;
    [SerializeField] private float intervaloSpawn = 0.7f;
    [SerializeField] private float vidaCirculo = 2f;
    [SerializeField] private int maxCirculosSimultaneos = 4;
    [Tooltip("Separación mínima entre círculos para que no se encimen")]
    [SerializeField] private float distanciaMinima = 1.5f;

    [Header("Área donde aparecen")]
    [SerializeField] private Vector2 areaCentro = Vector2.zero;
    [SerializeField] private Vector2 areaTamano = new Vector2(12f, 6f);

    [Header("Puntaje")]
    [SerializeField] private int puntajeObjetivo = 10;
    [SerializeField] private TextMeshProUGUI textoPuntaje;

    [Header("Al completar")]
    [Tooltip("Panel a pantalla completa con el texto 'Minijuego completado'")]
    [SerializeField] private GameObject panelCompletado;
    [SerializeField] private float segundosAntesDeVolver = 2.5f;

    private readonly List<Circulos> circulos = new List<Circulos>();
    private Camera cam;
    private int puntaje;
    private bool terminado;
    private float proximoSpawn;

    void Start()
    {
        cam = Camera.main;
        if (panelCompletado != null) panelCompletado.SetActive(false);
        ActualizarTexto();
        proximoSpawn = Time.time + 0.5f;
    }

    void Update()
    {
        if (terminado) return;

        // Clic sobre un círculo
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(mouse);

            if (col != null && col.TryGetComponent(out Circulos circulo))
            {
                if (circulo.Golpear())
                {
                    puntaje++;
                    ActualizarTexto();

                    if (puntaje >= puntajeObjetivo)
                    {
                        StartCoroutine(Completar());
                        return;
                    }
                }
            }
        }

        // Generar círculos
        if (Time.time >= proximoSpawn)
        {
            proximoSpawn = Time.time + intervaloSpawn;
            GenerarCirculo();
        }
    }

    void GenerarCirculo()
    {
        circulos.RemoveAll(c => c == null);
        if (circulos.Count >= maxCirculosSimultaneos) return;

        // Busca una posición libre (hasta 10 intentos)
        for (int i = 0; i < 10; i++)
        {
            Vector2 pos = areaCentro + new Vector2(
                Random.Range(-areaTamano.x / 2f, areaTamano.x / 2f),
                Random.Range(-areaTamano.y / 2f, areaTamano.y / 2f));

            if (EstaLibre(pos))
            {
                Circulos nuevo = Instantiate(circuloPrefab, pos, Quaternion.identity);
                nuevo.Iniciar(vidaCirculo);
                circulos.Add(nuevo);
                return;
            }
        }
    }

    bool EstaLibre(Vector2 pos)
    {
        foreach (Circulos c in circulos)
        {
            if (c != null && Vector2.Distance(c.transform.position, pos) < distanciaMinima)
                return false;
        }
        return true;
    }

    void ActualizarTexto()
    {
        if (textoPuntaje != null)
            textoPuntaje.text = "Puntaje: " + puntaje + " / " + puntajeObjetivo;
    }

    IEnumerator Completar()
    {
        terminado = true;

        // Limpiar los círculos que queden
        foreach (Circulos c in circulos)
            if (c != null) Destroy(c.gameObject);
        circulos.Clear();

        if (panelCompletado != null) panelCompletado.SetActive(true);

        yield return new WaitForSeconds(segundosAntesDeVolver);

        // Vuelve al nivel y marca la zona del corazón como completada
        Escenas.Volver();
    }

    // Dibuja el área en la vista Scene para poder ajustarla
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(areaCentro, areaTamano);
    }
}