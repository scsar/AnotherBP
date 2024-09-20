using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.PixelArtPlatformer_VillageProps;


public class RootChest : MonoBehaviour, IInteraction
{
    [SerializeField]
    private List<int> itemCategory, weaphonCategory;
    [SerializeField]
    private int maxValue;

    public void InterAction()
        {
            if (!GetComponent<Chest>().IsOpened)
            {
                GetComponent<Chest>().Open();
                GameObject.FindWithTag("Root").GetComponent<RootSystem>().RandomRoot(maxValue, itemCategory, weaphonCategory, gameObject.transform);
            }
        }
}
