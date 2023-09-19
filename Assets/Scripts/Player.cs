using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public PlayerInputActions playerControls;
    public Camera camera;
    public float maxSpeed;
    public float targetRadius;

    private InputAction fire;
    private Vector2 mouseScreenPosition;
    private Vector3 targetPosition;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable()
    {
        fire.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 direction = targetPosition - transform.position;
        float distance = direction.magnitude;

        if (distance <= targetRadius)
        {
            return;
        }

        Vector2 velocity = direction.normalized * (distance / targetRadius);

        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        FaceTarget(velocity);

        float newX = transform.position.x + velocity.x * Time.deltaTime;
        float newY = transform.position.y + velocity.y * Time.deltaTime;

        transform.position = new Vector2(newX, newY);
    }

    private void Fire(InputAction.CallbackContext context)
    {

        mouseScreenPosition = Mouse.current.position.ReadValue();
        targetPosition = camera.ScreenToWorldPoint(mouseScreenPosition);

        Debug.Log(targetPosition);

    }

    private void FaceTarget(Vector3 velocity)
    {
        if (velocity.magnitude > 0)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}

