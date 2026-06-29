using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Faller : MonoBehaviour, IResettable, IMover
{
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float gravity = 9.8f;
    
    private float currentSpeed = 0f;
    private Vector3 directionalSpeed = Vector3.zero;

    private Vector3 initialPos;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        initialPos = transform.position;
    }

    private void Update()
    {
    }

    void FixedUpdate()
    {
        if (currentSpeed >= maxSpeed)
        {
            directionalSpeed = Vector3.down * maxSpeed;
        }
        else
        {
            currentSpeed += gravity * Time.fixedDeltaTime;
            directionalSpeed = Vector3.down * currentSpeed;
        }
        rb.linearVelocity = directionalSpeed;
    }

    public void ResetObject()
    {
        currentSpeed = 0f;
        transform.position = initialPos;
        rb.linearVelocity = Vector2.zero;
    }

    public Vector2 GetVelocity()
    {
        return directionalSpeed;
    }
}