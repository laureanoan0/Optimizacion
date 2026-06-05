
using UnityEngine;

public class CameraBehavior: IUpdateable
{
    private float sensX = 400;
    private float sensY = 400;

    private Transform orientation;
    private Transform transform;
    private Transform playerTransform;


    private float xRotation;
    private float yRotation;


    public CameraBehavior(Transform playerTransform, Transform cameraPos)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        this.transform = cameraPos;
        this.orientation = cameraPos;
        this.playerTransform = playerTransform;

        UpdateManager.Instance.Register(this);
    }

    public void CustomUpdate(float time)
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * time * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * time * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.position = playerTransform.position;
    }

    public void Destroy()
    {
        UpdateManager.Instance.Unregister(this);
    }
}

