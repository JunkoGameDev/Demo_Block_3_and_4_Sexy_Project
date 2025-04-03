using UnityEngine;

public class Habitat : MonoBehaviour
{
    [SerializeField] private GameObject habitatProps;

    public void ActivateHabitat()
    {
        habitatProps.SetActive(true);
    }
}
