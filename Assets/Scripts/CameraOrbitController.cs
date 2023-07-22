using UnityEngine;
using System.Collections;


public class CameraOrbitController : MonoBehaviour
{
    public enum EMouse { MouseLeftButton, MouseMiddleButton, MouseRightButton }

    [Header("Main Setting")]
    public Transform TargetCamera;
    public Transform TargetPlayer;

    [Header("Distance Setting")]
    public float distance = 5f;
    public float minDistance = 1f; //Min distance of the camera from the target
    public float maxDistance = 10f;
    public int yMinLimit = 10; //Lowest vertical angle in respect with the target.
    public int yMaxLimit = 80;

    [Header("Mouse Setting")]
    public EMouse MouseInput;
    public int mouseZoomSpeed = 2;
    public float mouseXSpeed = 1000.0f;
    public float mouseYSpeed = 1000.0f;

    [Header("Touch Setting")]
    public float touchZoomSpeed = 1;
    public float touchXSpeed = 200.0f;
    public float touchYSpeed = 100.0f;


    private float lastDist = 0;
    private float curDist = 0;


    private float x = 0.0f;
    private float y = 0.0f;
    private Touch touch;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }



    void Update()
    {

        if (TargetPlayer && TargetCamera)
        {

            //Zooming with touch
            if (Input.touchCount > 1 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
            {
                //Two finger touch does pinch to zoom
                var touch1 = Input.GetTouch(0);
                var touch2 = Input.GetTouch(1);
                curDist = Vector2.Distance(touch1.position, touch2.position);
                if (curDist > lastDist)
                {
                    distance -= Vector2.Distance(touch1.deltaPosition, touch2.deltaPosition) * touchZoomSpeed / 10;
                }
                else
                {
                    distance += Vector2.Distance(touch1.deltaPosition, touch2.deltaPosition) * touchZoomSpeed / 10;
                }
                lastDist = curDist;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") != 0)  //ELSE Zooming with mouse
            {
                distance += Input.GetAxis("Mouse ScrollWheel") * mouseZoomSpeed;
                distance = Mathf.Clamp(distance, minDistance, maxDistance);
            }

            //Rotate with touch
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //One finger touch does orbit
                touch = Input.GetTouch(0);
                x += touch.deltaPosition.x * touchXSpeed * Time.deltaTime;
                y -= touch.deltaPosition.y * touchYSpeed * Time.deltaTime;
            }
            else
            {
                KeyCode MouseButton = KeyCode.Mouse0;

                if (MouseInput == EMouse.MouseLeftButton)
                {
                    MouseButton = KeyCode.Mouse0;
                }
                if (MouseInput == EMouse.MouseRightButton)
                {
                    MouseButton = KeyCode.Mouse1;
                }
                if (MouseInput == EMouse.MouseMiddleButton)
                {
                    MouseButton = KeyCode.Mouse2;
                }

                if (Input.GetKey(MouseButton))
                {
                    x += Input.GetAxis("Mouse X") * mouseXSpeed * Time.deltaTime;
                    y -= Input.GetAxis("Mouse Y") * mouseYSpeed * Time.deltaTime;
                }
            }

            y = ClampAngle(y, yMinLimit, yMaxLimit);
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 vTemp = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * vTemp + TargetPlayer.position;
            TargetCamera.position = position;
            TargetCamera.rotation = rotation;
        }

    }



    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}