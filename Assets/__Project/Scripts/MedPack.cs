using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMedPack", menuName = "Itens/Med Pack")]
public class MedPack : Item
{
    [SerializeField]
    private int healAmount = 10;

    public override void Use(ItemUseOptions options)
    {
        options.Owner.GetComponent<Character>().Heal(healAmount);
    }
}
