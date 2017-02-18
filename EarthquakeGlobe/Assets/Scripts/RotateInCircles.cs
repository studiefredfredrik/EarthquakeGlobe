using UnityEngine;

public class RotateInCircles : MonoBehaviour {

    private float angle = 0;
    public float speed = 2;        // Note to self: 2*PI is 360 degrees
    public float radius = 5;
    public Transform lookAtTarget;
    public Transform transformTarget;

    void Update()
    {
        var trueSpeed = (2 * Mathf.PI) / (1 / speed);
        angle += trueSpeed * Time.deltaTime;            
        var x = Mathf.Cos(angle) * radius;
        var z = Mathf.Sin(angle) * radius;
        var y = transform.position.y;
        transform.position = new Vector3(x, y, z);
        transform.LookAt(lookAtTarget);
    }
}
