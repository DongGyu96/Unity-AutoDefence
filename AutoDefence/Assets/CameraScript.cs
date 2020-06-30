using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Vector3 originPosition;
    private Quaternion originQuaternion;
    private float originFOV;

    [SerializeField] private float cameraMoveSpeed = 100f;
    [SerializeField] private float cameraZoomSpeed = 10f;
    [SerializeField] private float cameraRotateSpeed = 3f;

    private float pitch;
    private float yaw;

    private float ratio = 1f;
    

    // Start is called before the first frame update
    void Start()
    {
        originPosition = transform.position;
        originQuaternion = transform.rotation;
        originFOV = gameObject.GetComponent<Camera>().fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            ratio = 2f;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            ratio = 1f;
        }
        float keyHorizontal = Input.GetAxis("Horizontal");
        float keyVertical = Input.GetAxis("Vertical");

        transform.Translate(transform.right * cameraMoveSpeed * Time.smoothDeltaTime * keyHorizontal * ratio, Space.World);
        transform.Translate(transform.forward * cameraMoveSpeed * Time.smoothDeltaTime * keyVertical * ratio, Space.World);

        Zoom();

        Rotate();

        UpAndDown();

        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            transform.position = originPosition;
            transform.rotation = originQuaternion;
            pitch = 0f;
            yaw = 0f;
            gameObject.GetComponent<Camera>().fieldOfView = originFOV;
        }
    }

    void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * cameraZoomSpeed;
        if (distance != 0)
        {
            gameObject.GetComponent<Camera>().fieldOfView += distance;
        }
    }

    void Rotate()
    {
        if (Input.GetMouseButton(1))
        {
            //Vector3 rot = transform.rotation.eulerAngles; // 현재 카메라의 각도를 Vector3로 반환
            //rot.y += Input.GetAxis("Mouse X") * cameraRotateSpeed; // 마우스 X 위치 * 회전 스피드
            //rot.x += -1 * Input.GetAxis("Mouse Y") * cameraRotateSpeed; // 마우스 Y 위치 * 회전 스피드
            //Quaternion q = Quaternion.Euler(rot); // Quaternion으로 변환
            //q.z = 0;
            //transform.rotation = Quaternion.Slerp(transform.rotation, q, 2f); // 자연스럽게 회전

            yaw += cameraRotateSpeed * Input.GetAxis("Mouse X");
            pitch -= cameraRotateSpeed * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        }
    }

    void UpAndDown()
    {
        if(Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.KeypadPlus))
        {
            transform.Translate(Vector3.up * cameraMoveSpeed * Time.smoothDeltaTime * ratio, Space.World);
        }
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.KeypadMinus))
        {
            transform.Translate(-Vector3.up * cameraMoveSpeed * Time.smoothDeltaTime * ratio, Space.World);
        }
    }
}
