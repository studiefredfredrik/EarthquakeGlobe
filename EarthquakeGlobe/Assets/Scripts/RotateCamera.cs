using UnityEngine;

public class RotateCamera : MonoBehaviour {
    private float angle;
    private float originalSpeed;
    private float xDeg;
    private float yDeg;
    private Quaternion fromRotation;
    private Quaternion toRotation;
    private float posY;

    public float speed;         
    public float radius;
    public Transform lookAtTarget;
    public Transform transformTarget;
    public float friction;
    public float lerpSpeed;
    public float scrollSpeed;

    void Start()
    {
        originalSpeed = speed;
    }

    void Update()
    {
        radius -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * friction; // Mouse scrollwheel-zoom

        if (Input.touchCount == 2) // Mobile pinch-zoom
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            radius += deltaMagnitudeDiff / 1000;
            speed = 0;
        }
        else if(Input.GetMouseButton(0)) // Change camera rotation by mouse-drag
        { 
            angle -= Input.GetAxis("Mouse X") * lerpSpeed * friction;
            posY -= Input.GetAxis("Mouse Y") * lerpSpeed * friction;
            radius += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * friction;
            speed = 0;
        }
        else // Continue normal rotation
        {
            speed = originalSpeed;
            var trueSpeed = speed;
            angle += trueSpeed * Time.deltaTime;
        }

        // Apply movement
        Vector3 pointPos =
            Quaternion.AngleAxis(angle, -Vector3.up) *
            Quaternion.AngleAxis(posY, -Vector3.right) *
            new Vector3(0, 0, radius);
        transform.position = pointPos; 
        transform.LookAt(lookAtTarget);

    }
}
