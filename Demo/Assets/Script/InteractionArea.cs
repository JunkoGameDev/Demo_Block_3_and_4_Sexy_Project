using UnityEngine;

public class InteractionArea : MonoBehaviour
{
    [SerializeField] private ThirdPersonController player;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            player.canInteract(this.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            player.cannotInteract();
        }
    }
}
