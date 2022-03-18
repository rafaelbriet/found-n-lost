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

    [Header("Animation")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Animator effectsAnimator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [Header("Others")]
    [SerializeField]
    private WarehouseManager warehouseManager;

    private Vector2 direction;
    private Vector2 mousePosition;
    private new Rigidbody2D rigidbody2D;
    private Inventory inventory;
    private ItemUseOptions itemUseOptions;
    private PlayerSFXManager sfxManager;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        inventory = GetComponent<Inventory>();
        sfxManager = GetComponent<PlayerSFXManager>();

        itemUseOptions = new ItemUseOptions { Owner = gameObject, Camera = camera, Animator = effectsAnimator, PlayerSFXManager = sfxManager };
    }

    private void OnEnable()
    {
        SetPistolAnimation();
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = (speed * Time.deltaTime * direction) + new Vector2(transform.position.x, transform.position.y);
        rigidbody2D.MovePosition(newPosition);

        string animationName = "";

        if (animator.GetBool("IsUsingPistol"))
        {
            animationName = "_Pistol";
        }

        if (direction.x > 0)
        {
            animator.Play($"Base Layer.Player{animationName}_Run_Side");
            spriteRenderer.flipX = true;
        }
        else if (direction.x < 0)
        {
            animator.Play($"Base Layer.Player{animationName}_Run_Side");
            spriteRenderer.flipX = false;
        }
        else if (direction.y < 0)
        {
            animator.Play($"Base Layer.Player{animationName}_Run_Front");
        }
        else if (direction.y > 0)
        {
            animator.Play($"Base Layer.Player{animationName}_Run_Back");
        }
        else
        {
            animator.Play($"Base Layer.Player{animationName}_Idle_Front");
        }
    }

    private void LateUpdate()
    {
        CameraFollow();
    }

    private void OnDisable()
    {
        direction = Vector2.zero;
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
        
    }

    public void OnUseItem()
    {
        if (warehouseManager.IsGamePaused())
        {
            return;
        }

        if (inventory.SelectedItem == null)
        {
            return;
        }

        itemUseOptions.MousePosition = mousePosition;

        inventory.SelectedItem.Use(itemUseOptions);
    }

    public void OnSelectItem(InputValue value)
    {
        if (warehouseManager.IsGamePaused())
        {
            return;
        }

        int index = (int)value.Get<float>() - 1;
        inventory.SelectItem(index);

        SetPistolAnimation();
    }

    public void OnPause()
    {
        if (warehouseManager.IsGamePaused())
        {
            warehouseManager.UnpauseGame();
        }
        else
        {
            warehouseManager.PauseGame();
        }
    }

    private void SetPistolAnimation()
    {
        if (inventory.SelectedItem?.ItemScriptableObject as PistolScriptableObject)
        {
            animator.SetBool("IsUsingPistol", true);
        }
        else
        {
            animator.SetBool("IsUsingPistol", false);
        }
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
