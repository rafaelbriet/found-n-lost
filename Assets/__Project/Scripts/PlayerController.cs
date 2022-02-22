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

    private Vector2 direction;
    private Vector2 mousePosition;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction);
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
        Debug.Log("OnUseItem");
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
