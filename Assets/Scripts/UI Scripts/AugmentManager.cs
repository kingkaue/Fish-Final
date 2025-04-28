using UnityEngine;

public class AugmentManager : MonoBehaviour
{
    [SerializeField] private GameObject[] augments;

    public void ActivateGlassCannon()
    {
        augments[0].SetActive(true);
    }

    public void ActivateSousVide()
    {
        augments[1].SetActive(true);
    }

    public void ActivateTheTank()
    {
        augments[2].SetActive(true);
    }

    public void ActivateClassAugment1()
    {
        augments[3].SetActive(true);
    }

    public void ActivateClassAugment2()
    {
        augments[4].SetActive(true);
    }
}
