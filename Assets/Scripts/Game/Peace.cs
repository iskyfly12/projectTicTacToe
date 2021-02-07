using UnityEngine;

public enum PeaceType { None = 0, Cross = 1, Nought = -1 }

public class Peace : MonoBehaviour
{
    [SerializeField] private float heightSpaw;
    [SerializeField] private Quaternion rotationSpawn;

    void OnEnable()
    {
        transform.position = new Vector3(transform.position.x, heightSpaw, transform.position.z);
        transform.rotation = rotationSpawn;
    }
}
