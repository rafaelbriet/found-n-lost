using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpray", menuName = "Itens/Spray")]
public class SprayScriptableObject : ItemScriptableObject
{
    public override void Use(ItemUseOptions options)
    {
        options.Owner.GetComponent<Character>().ApplySpray(duration);

        options.Animator.SetBool("IsSprayActive", true);
        options.Animator.Play("Base Layer.ItensEffects_Spray");
    }

    public override void ResetAnimation(ItemUseOptions options)
    {
        if (options == null)
        {
            return;
        }

        options.Animator.SetBool("IsSprayActive", false);
        options.Animator.GetComponent<SpriteRenderer>().sprite = null;
    }
}
