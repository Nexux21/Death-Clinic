using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform targetCamera;

    [SerializeField, Range(-1, 1)]

    private float parllaxEffect = 0;

    [SerializeField]

    private float spriteLenght;

    private float initialPos;

    void Start()
    {
        initialPos = transform.position.x;
    }

    private void LateUpdate()
    {
        float parallaxDistance = targetCamera.position.x * parllaxEffect;

        transform.position = new Vector3(initialPos + parallaxDistance, transform.position.y, transform.position.z);

        float temporalDistance = targetCamera.position.x * (1 - parllaxEffect);

        if (temporalDistance > initialPos + spriteLenght)
        {
            initialPos += spriteLenght;
        }
        else if (temporalDistance < initialPos - spriteLenght)
        {

        }
        void Update()
        {

        }
    }
}

