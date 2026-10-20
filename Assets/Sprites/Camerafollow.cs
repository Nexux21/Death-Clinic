using UnityEngine;
using UnityEngine.InputSystem;
/// Camara parallax
public class Camerafollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed;

    public Vector2 offset;

    public float DinamicCamera;
    void Update()
    {
        Vector3 targetPos = new Vector3(
            target.transform.position.x + offset.x, 
            target.transform.position.y + offset.y, 
            -10);
        Vector2 mousePos = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue()) - new Vector3(0.5f, 0.5f, 0.5f);

        Vector3 dinamicPos = targetPos + ((Vector3)mousePos * DinamicCamera);


        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            followSpeed * Time.deltaTime
            );

    }
}
