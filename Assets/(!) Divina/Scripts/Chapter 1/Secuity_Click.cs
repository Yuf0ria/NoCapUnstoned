using UnityEngine;

public class Secuity_Click : MonoBehaviour
{
    public void securityclick (Security_Research sr)
    {
        sr.addprogression();
        gameObject.SetActive(false);
    }
}
