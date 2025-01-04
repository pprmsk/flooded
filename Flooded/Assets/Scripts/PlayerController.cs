using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float flipSpeed;
    public float jumpForce;
    bool isGrounded;

    enum LookDirection { Left, Right };
    LookDirection lookDirection;
    LookDirection currentLookDirection;
    bool isFlipping;
    Vector2 moveInput;

    void Start()
    {
        currentLookDirection = LookDirection.Right;
    }

    void Update()
    {
        transform.position += new Vector3(moveInput.x, 0, moveInput.y) * speed * Time.deltaTime;

        Debug.Log(isFlipping);
    }

    public void onMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        lookDirection = input.x > 0 ? LookDirection.Right : (input.x < 0 ? LookDirection.Left : lookDirection);
        moveInput = input;

        ChangeDirection(lookDirection);
    }

    void ChangeDirection(LookDirection newDirection)
    {
        if (newDirection != currentLookDirection && !isFlipping)
        {
            currentLookDirection = newDirection;
            StartCoroutine(Flip());
        }
    }

    IEnumerator Flip()
{
    float targetRotation = (lookDirection == LookDirection.Right) ? 0f : 180f;
    float currentRotation = transform.eulerAngles.y;
    float angleDifference = Mathf.DeltaAngle(currentRotation, targetRotation);
    
    isFlipping = true;

    while (Mathf.Abs(angleDifference) > 2f)
    {
        currentRotation = Mathf.MoveTowardsAngle(currentRotation, targetRotation, flipSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
        angleDifference = Mathf.DeltaAngle(currentRotation, targetRotation);

        yield return null;
    }

    transform.rotation = Quaternion.Euler(0f, targetRotation, 0f);

    isFlipping = false;
}
}
