using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class camera : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float followCamera;
    public Vector2 Offset;
    public float DinamicCamera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = new Vector3(
            Target.transform.position.x + Offset.x,
            Target.transform.position.y + Offset.y,
            -10);

        //->Unicamente si quieremos camara dinamica
        Vector3 mousePos = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue())
            - new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 DinamicPos = targetPos + mousePos * DinamicCamera;
        //<-

        //Si queremos camara normal reemplzar DinamicPos con TargetPos
        transform.position = Vector3.Lerp(
            transform.position,
            DinamicPos,
            followCamera * Time.deltaTime
            );
    }
}
