using UnityEngine;
using System.Collections;

public class MoveBetweenPositions : MonoBehaviour {

    public static Vector3[] positions;
    public static int currentPosition;
    public float speed = 1.5f;

    private bool myBool = false;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate()
    {
        if (myBool == true)
        {
            transform.position = Vector3.Lerp(transform.position, positions[currentPosition], speed / 150);
        }
    }
}
