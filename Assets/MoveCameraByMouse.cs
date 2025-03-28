using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;

/// <summary>
/// 修改记录：史苏洋 2022/4/11 增加相机自动旋转方法
/// </summary>
public class MoveCameraByMouse : MonoBehaviour
{
    public static MoveCameraByMouse mainGodCam;
    public Transform target;
    public float x;//x轴初始角度
    public float y;//y轴初始角度
    public float xSpeed = 10;//x轴转速
    public float ySpeed = 10;//y轴转速
    public float yMinLimit = 10;//y轴最小角度
    public float yMaxLimit = 90;//y轴最大角度
    public float distance = 100;//变换后的距离


    public bool needDamping = true;//是否有阻力
    public float damping = 3.0f;//阻力值（越小越平滑）
    float m_distance;//当前的距离


    Quaternion m_rotation;//当前的角度
    Quaternion rotation;//变换后的角度
    Vector3 position;//变换后的位置
    Vector3 m_position;//当前的位置


    public bool RotateEnabled = true;//是否允许旋转


    public bool autoRoat;
    public float autotime;
    public float direction = 1;

    float time;
    void Start()
    {
        time = 300;
        m_distance = (transform.position - target.position).magnitude;//获取摄像机初始距离
    }
    private void OnEnable()
    {
        mainGodCam = this;
    }

    // Update is called once per frame  
    void Update()
    {

        MouseRotate();

        time -= Time.deltaTime;
        if (time < 0)
        {
            SceneManager.LoadScene(0);
        }




    }



    public void MouseRotate()
    {
        if (!RotateEnabled)
            return;
        if (target)
        {

            if (UserInput.MianUserInput.mouse.leftButton.isPressed)
            {
                Vector2 v2 = UserInput.MianUserInput.mouse.delta.ReadValue();
                x += v2.x * xSpeed * 0.02f;
                y -= v2.y * ySpeed * 0.02f;
                Debug.Log("鼠标");
                time = 300;
            }
            else if (Input.touches.Length > 0)
            {
                Debug.Log("触摸");

                Vector2 v3 = Input.touches[0].deltaPosition;
                x += v3.x * xSpeed * 0.02f;
                y -= v3.y * ySpeed * 0.02f;
                time = 300;
            }
            else
            {
                if (autoRoat)
                {

                    x = x + (360 / autotime) * Time.deltaTime * direction;

                }
            }

            while (x < 0)
            {
                x = x + 360;
            }
            while (x > 360)
            {
                x = x - 360;
            }
            //y = ClampAngle(y, yMinLimit, yMaxLimit);
            rotation = Quaternion.Euler(y, x, 0);
            m_rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);

            position = target.position - rotation * Vector3.forward * distance;
            m_position = target.position - m_rotation * Vector3.forward * m_distance;
            if (needDamping)
            {
                transform.rotation = m_rotation;
                transform.position = m_position;
            }
            else
            {
                transform.rotation = rotation;
                transform.position = position;
            }
        }
    }







    /// <summary>
    /// 切换摄像机为正交或透视
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="IsOrthogonality"></param>
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }



}

