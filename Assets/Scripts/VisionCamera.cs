using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class VisionCamera : MonoBehaviour
{
    public float range = 10f;
    public float fov = 45f;
    public LayerMask layerMask;
    public float lastHitTime;
    public float rotationAngle, speed;
    public float actualAngle, startAngle, maxAngle, minAngle;
    bool reverse = false;
    public Color colorIdle, colorFound;
    public Light2D lightVision;

    // Start is called before the first frame update
    void Awake()
    {
        startAngle = transform.rotation.eulerAngles.z;
        maxAngle = startAngle + rotationAngle / 2f;
        minAngle = startAngle - rotationAngle / 2f;
        actualAngle = startAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isSuperAlert)
            lightVision.color = colorFound;
        else
            lightVision.color = colorIdle;
        Rotation();

        float startAngle = (Mathf.Rad2Deg * Mathf.Atan2(transform.right.y, transform.right.x) - fov / 2) * Mathf.Deg2Rad;
        int nbRay = (int)Mathf.Ceil(fov);
        for (int i = 0; i < nbRay; i++)
        {
            float angle = startAngle + i * fov * Mathf.Deg2Rad / nbRay;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), range, layerMask);
            Debug.DrawRay(transform.position, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * range, Color.green);
            if (hit.collider != null && hit.collider.tag == "Player" && !hit.collider.gameObject.GetComponent<PlayerController>().invisi)
                if (!GameManager.Instance.isSuperAlert)
                    GameManager.Instance.SuperAlert(hit.collider.transform);
        }
    }

    void Rotation()
    {
        if (!reverse)
        {
            actualAngle += speed * Time.deltaTime;
            if (actualAngle > maxAngle)
            {
                reverse = true;
                actualAngle = maxAngle;
            }
        }
        else
        {
            actualAngle -= speed * Time.deltaTime;
            if (actualAngle < minAngle)
            {
                reverse = false;
                actualAngle = minAngle;
            }
        }

        transform.rotation = Quaternion.Euler(0, 0, actualAngle);
    }
}
