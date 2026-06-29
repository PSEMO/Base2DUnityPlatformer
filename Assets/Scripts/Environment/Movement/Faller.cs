using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Faller : MonoBehaviour, IPoolable, IMover
{
    [field: SerializeField] public string ID { get; set; }

    [Space]

    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float gravity = 9.8f;
    
    private float currentSpeed = 0f;
    private Vector3 directionalSpeed = Vector3.zero;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

    public Vector2 GetVelocity()
    {
        return directionalSpeed;
    }

    public void ResetObject()
    {
        currentSpeed = 0f;
        rb.linearVelocity = Vector2.zero;
    }
}