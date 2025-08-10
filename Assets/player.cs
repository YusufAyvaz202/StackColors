using System.Collections;
using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var input = Input.GetAxis("Horizontal");
        
        transform.position += new Vector3(input, 0, 0) * (Time.deltaTime * 5f);
    }
}
