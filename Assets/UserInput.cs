using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;


    public class UserInput : MonoBehaviour
    {
        // Start is called before the first frame update
        //所有用户的输入
        public static UserInputDevice MianUserInput { get; private set; } = new UserInputDevice(Mouse.current, Keyboard.current);
        //用户
        public MouseData _md;
        public Vector2 _oldmousPos = new Vector2();
        //键盘状态NewInputSystem;
        private KeyboardState m_keyboardState;
        void Start()
        {
            init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        void init()
        {
            // InputSystem.FlushDisconnectedDevices();
            m_keyboardState = new KeyboardState();//初始化键盘
            Mouse m = InputSystem.AddDevice<Mouse>();
            Keyboard k = InputSystem.AddDevice<Keyboard>();
            MianUserInput = new UserInputDevice(m, k);//所有的外接设备
        }
        /// <summary>
        /// 处理键盘数据
        /// </summary>
        /// <param name="data"></param>
        public void getKeyboardData(KeyData _kd)
        {
            if (_kd == null)
            {
                return;
            }
            Key k = (Key)_kd.key;
            m_keyboardState.Set(k, _kd.isDown);//newInput的键盘修改
            InputSystem.QueueStateEvent(MianUserInput.keyboard, m_keyboardState);//向inputSystem提交修
            _kd = null;
        }
        public void getMouseData()
        {
            
        }
        private void Update()
        {
            //getKeyboardData();
            getMouseData();
        }
        public void OnDisable()
        {
            InputSystem.RemoveDevice(MianUserInput.mouse);
            InputSystem.RemoveDevice(MianUserInput.keyboard);
        }
    }

public class UserInputDevice
{
    public UserInputDevice(Mouse m, Keyboard k)
    {
        mouse = m;
        keyboard = k;
        mouse.MakeCurrent();
        keyboard.MakeCurrent();
    }
    public Mouse mouse;
    public Keyboard keyboard;
}
public class KeyData
{
    public int key;
    public bool isDown;
}
public class MouseData
{
    public bool left = false;
    public bool right = false;
    public bool middle = false;
    public int x = 0;
    public int y = 0;
    public int deltaX = 0;
    public int deltaY = 0;
    public float wheelX = 0.0f;
    public float wheelY = 0.0f;
}
