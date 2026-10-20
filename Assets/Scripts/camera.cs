using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class camera : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private float followCamera;
    public Vector2 Offset;
    public float DinamicCamera;
    
    void Update()
    {
        Vector3 targetPos = new Vector3(
            Target.transform.position.x + Offset.x,
            Target.transform.position.y + Offset.y,
            -10);

        
        Vector3 mousePos = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue())
            - new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 DinamicPos = targetPos + mousePos * DinamicCamera;
        //<-

        
        transform.position = Vector3.Lerp(
            transform.position,
            DinamicPos,
            followCamera * Time.deltaTime
            );
    }
}
