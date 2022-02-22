using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPistol", menuName = "Itens/Pistol")]
public class Pistol : Item
{
    [SerializeField]
    private int damageAmount = 10;
    [SerializeField]
    private float range = 5f;
    [SerializeField]
    private LayerMask layerMask;

    public override void Use(ItemUseOptions options)
    {
        Vector3 mouseWorldPosition = options.Camera.ScreenToWorldPoint(options.MousePosition);
        Vector3 directionToMouse = mouseWorldPosition - options.Owner.transform.position;

        RaycastHit2D hit = Physics2D.Raycast(options.Owner.transform.position, directionToMouse, range, layerMask);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            Character character = hit.collider.GetComponent<Character>();
            character.Damage(damageAmount);
        }
    }
}
