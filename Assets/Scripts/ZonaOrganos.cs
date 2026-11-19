using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ZonaOrganos : MonoBehaviour
{
    public static readonly List<ZonaOrganos> Todas = new List<ZonaOrganos>();

    [Tooltip("Déjalo vacío para aceptar cualquier órgano")]
    [SerializeField] private string tipoAceptado = "";
    [SerializeField] private int organosNecesarios = 1;

    [Tooltip("Si está activo, el órgano se acomoda en el punto de depósito")]
    [SerializeField] private bool centrarOrgano = false;
    [Tooltip("Dónde se coloca el órgano si Centrar Organo está activo (opcional)")]
    [SerializeField] private Transform puntoDeposito;

    [Header("Al completar")]
    [Tooltip("Escena a cargar cuando se llene (vacío = no cambia de escena)")]
    [SerializeField] private string escenaAlCompletar = "Victory";
    [Tooltip("Segundos de espera para ver el último órgano antes de cambiar")]
    [SerializeField] private float retraso = 1f;
    [SerializeField] private UnityEvent alCompletar;

    private Collider2D zonaCol;
    private int depositados;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reiniciar()
    {
        Todas.Clear();
    }

    void Awake()
    {
        zonaCol = GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        Todas.Add(this);
    }

    void OnDisable()
    {
        Todas.Remove(this);
    }

    public bool EstaSobre(Collider2D otro)
    {
        return zonaCol.Distance(otro).isOverlapped;
    }

    public bool Recibir(Organo organo)
    {
        if (depositados >= organosNecesarios) return false;

        if (tipoAceptado != "" && organo.TipoOrgano != tipoAceptado)
            return false;

        depositados++;

        if (centrarOrgano)
        {
            Vector3 destino = puntoDeposito != null
                ? puntoDeposito.position
                : transform.position;
            organo.Depositar(destino);
        }
        else
        {
            organo.Depositar();
        }

        Debug.Log("Depositado: " + organo.TipoOrgano +
                  " (" + depositados + "/" + organosNecesarios + ")");

        if (depositados >= organosNecesarios)
            Completar();

        return true;
    }

    void Completar()
    {
        alCompletar.Invoke();

        if (escenaAlCompletar != "")
            Invoke(nameof(CargarEscena), retraso);
    }

    void CargarEscena()
    {
        SceneManager.LoadScene(escenaAlCompletar);
    }
}