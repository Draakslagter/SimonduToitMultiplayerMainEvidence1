using Unity.VisualScripting;
using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDrownable>() == null) return;
        var drownable = other.gameObject.GetComponent<IDrownable>();
        drownable.SendData();
        Destroy(other.gameObject);
    }
}
