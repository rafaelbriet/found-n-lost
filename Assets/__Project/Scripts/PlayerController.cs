using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private float speed = 5f;

    [Header("Camera")]
    [SerializeField]
    private new Camera camera;
    [SerializeField]
    private Transform cameraHolder;
    [SerializeField]
    private float cameraOffset = 5f;
    [SerializeField]
    private Item selectedItem;

    [Header("Animation")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Vector2 direction;
    private Vector2 mousePosition;
    private new Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = (speed * Time.deltaTime * direction) + new Vector2(transform.position.x, transform.position.y);
        rigidbody2D.MovePosition(newPosition);

        if (direction.x > 0)
        {
            animator.Play("Base Layer.Player_Run_Side");
            spriteRenderer.flipX = true;
        }
        else if (direction.x < 0)
        {
            animator.Play("Base Layer.Player_Run_Side");
            spriteRenderer.flipX = false;
        }
        else if (direction.y < 0)
        {
            animator.Play("Base Layer.Player_Run_Front");
        }
        else if (direction.y > 0)
        {
            animator.Play("Base Layer.Player_Run_Back");
        }
        else
        {
            animator.Play("Base Layer.Player_Idle_Front");
        }
    }

    private void LateUpdate()
    {
        CameraFollow();
    }

    public void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        mousePosition = value.Get<Vector2>();
    }

    public void OnReload()
    {
        Debug.Log("OnReload");
    }

    public void OnUseItem()
    {
        selectedItem.Use(new ItemUseOptions { Owner = gameObject, Camera = camera, MousePosition = mousePosition });
    }

    private void CameraFollow()
    {
        Vector2 mouseViewportPosition = GetMouseViewportPosition();

        float xOffset = cameraOffset * mouseViewportPosition.x;
        float yOffset = cameraOffset * mouseViewportPosition.y;

        cameraHolder.position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, cameraHolder.position.z);
    }

    private Vector2 GetMouseViewportPosition()
    {
        Vector3 viewportPosition = camera.ScreenToViewportPoint(mousePosition);
        viewportPosition.x = (viewportPosition.x - 0.5f) * 2f;
        viewportPosition.y = (viewportPosition.y - 0.5f) * 2f;

        viewportPosition.x = Mathf.Clamp(viewportPosition.x, -1f, 1f);
        viewportPosition.y = Mathf.Clamp(viewportPosition.y, -1f, 1f);

        return viewportPosition;
    }
}
