using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpray", menuName = "Itens/Spray")]
public class SprayScriptableObject : ItemScriptableObject
{
    [SerializeField]
    private float durationInSeconds = 15f;
    [SerializeField]
    private float range = 5f;
    [SerializeField]
    private float areaOfEffect = 1f;
    [SerializeField]
    private LayerMask layerMask;

    public override void Use(ItemUseOptions options)
    {
        Vector3 mouseWorldPosition = options.Camera.ScreenToWorldPoint(options.MousePosition);
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(mouseWorldPosition, areaOfEffect, layerMask);

        foreach (Collider2D collider in colliders)
        {
            if (Vector3.Distance(options.Owner.transform.position, collider.transform.position) < range)
            {
                collider.GetComponent<Character>().ApplySpray(durationInSeconds);
            }
        }
    }
}
