using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO.Ports;
using System.Threading;

enum Modes
{
    E_MAIN_MODE = 0,
    E_STIB_MODE,
    E_VILLO_MODE,
    E_SCOOTY_MODE,
    E_SNCB_MODE,
    E_METEO_MODE,
    E_TRAFIC_MODE,
    E_SELECT_TRAM
}

enum TypeButtons
{
    E_CHOICE_TYPE,
    E_BACK_TYPE,
}

public class Physical_Buttons_Management : MonoBehaviour
{

    private const int C_N_OF_STIB_CHOICES = 10;
    private const int C_N_OF_BUTTONS_CHOICE = 6;
    private const int C_N_OF_BUTTONS = 1 + C_N_OF_BUTTONS_CHOICE;
    private const int C_BUTTON_BACK = C_N_OF_BUTTONS - 1;


    Btn_mgmt Buttons_Manager;

    private bool _looping;
    private SerialPort _port;
    private Thread portReadingThread;
    private uint ui_Data;
    private bool Data_Received = false;
    
    private Modes mode = Modes.E_MAIN_MODE;
    private TypeButtons button;

    /*
    private float x_origin_buttons = 655.5f;
    private float x_origin_images = 685;
    private float x_distance = 61.5f;
    private float y_origin_position = -797;
	//resize 0.6 0.6 info et 0.54 0.54 cover
	*/
	private float x_origin_buttons = 639.5f;
    private float x_origin_images = 667;
    private float x_distance = 58.5f;
    private float y_origin_position = 23;
	
    public uint[] ui_tram_lines = new uint[C_N_OF_STIB_CHOICES];
    public Sprite[] img_Tram_Lines = new Sprite[C_N_OF_STIB_CHOICES];
    public Sprite[] img_2_STIB_choices = new Sprite[(C_N_OF_STIB_CHOICES / 2)];

    public Sprite[] img_main_menu= new Sprite[C_N_OF_BUTTONS_CHOICE];
    public Image[] img_Info_button = new Image[C_N_OF_BUTTONS];
    public Image[] img_Cover_button = new Image[C_N_OF_BUTTONS];

    // Start is called before the first frame update
    void Start()
    {
        uint uc_i;
        Buttons_Manager = GameObject.Find("CameraFilmingScene/Canvas").GetComponent<Btn_mgmt>();

        Place_Objects();

        for (uc_i = 0; uc_i < C_N_OF_BUTTONS_CHOICE; uc_i++)
        {
            img_Info_button[uc_i].sprite = img_main_menu[uc_i];
            img_Info_button[uc_i].enabled = true;
            img_Cover_button[uc_i].enabled = true;
        }
        /*Unhighlight back button*/
        img_Cover_button[uc_i].enabled = false;
        img_Info_button[uc_i].enabled = false;

        Init_port();
    }

    // Update is called once per frame
    void Update()
    {
        if (Data_Received)
        {
            Handle_Information();
            Data_Received = false;
        }
    }

    private void OnDestroy()
    {
        _looping = false;  // This is a necessary command to stop the thread.
                           // if you comment this line, Unity gets frozen when you stop the game in the editor.            

        _port.Close(); 
        portReadingThread.Join(1);
        portReadingThread.Abort();
    }

    void Place_Objects()
    {
        uint ui_i;
        float x,y;

        Vector3 temp = new Vector3(0, 0, 0);

        for (ui_i = 0; ui_i < C_N_OF_BUTTONS; ui_i++)
        {
            x = x_origin_buttons + (float)(ui_i) * x_distance;
            y = y_origin_position;
            x -= img_Cover_button[ui_i].rectTransform.position.x;
            y -= img_Cover_button[ui_i].rectTransform.position.y;
            temp.x = x;
            temp.y = y;
            img_Cover_button[ui_i].transform.position += temp;

        }

        for (ui_i = 0; ui_i < C_N_OF_BUTTONS; ui_i++)
        {
            x = x_origin_images + (float)(ui_i) * x_distance;
            x -= img_Info_button[ui_i].rectTransform.position.x;
            temp.x = x;
            img_Info_button[ui_i].transform.position += temp;
        }

    }

    void Handle_Information()
    {
        uint uc_i;
        Get_Type_Button_pressed(ui_Data);

        switch (mode)
        {
            case Modes.E_MAIN_MODE:
                if (button == TypeButtons.E_CHOICE_TYPE)
                    Get_mode_From_button();
                break;

            case Modes.E_STIB_MODE:

                if (button == TypeButtons.E_CHOICE_TYPE)
                {
                    if(ui_Data < C_N_OF_STIB_CHOICES/2)
                    {
                        Show2tramLines((uint)(ui_Data));
                        mode = Modes.E_SELECT_TRAM;
                    }
                }
                break;

            case Modes.E_SELECT_TRAM:

                if (button == TypeButtons.E_CHOICE_TYPE)
                {
                    /*send the name of the image contained into the sprite. 
                     * Name of image correspond to the number of tram/bus/metro*/
                    if (ui_Data < 2)
                    {
                        Buttons_Manager.StibLineButton(img_Info_button[ui_Data].sprite.name);
                        GoToMainMode();
                    }

                }
                else if (button == TypeButtons.E_BACK_TYPE)
                {
                    mode = Modes.E_STIB_MODE;
                    for (uc_i = 0; uc_i < (C_N_OF_STIB_CHOICES / 2); uc_i++)
                    {
                        img_Info_button[uc_i].sprite = img_2_STIB_choices[uc_i];
                        img_Info_button[uc_i].enabled = true;
                        img_Cover_button[uc_i].enabled = true;
                    }

                    img_Info_button[C_BUTTON_BACK].enabled = true;
                    img_Cover_button[C_BUTTON_BACK].enabled = true;

                    return;
                }

                break;
            case Modes.E_VILLO_MODE:
                if (button == TypeButtons.E_CHOICE_TYPE)
                    Get_mode_From_button();
                break;

            case Modes.E_SCOOTY_MODE:
                if (button == TypeButtons.E_CHOICE_TYPE)
                    Get_mode_From_button();
                break;

            case Modes.E_SNCB_MODE:
                if (button == TypeButtons.E_CHOICE_TYPE)
                    Get_mode_From_button();
                break;

            case Modes.E_METEO_MODE:
                Get_mode_From_button();
                break;

            case Modes.E_TRAFIC_MODE:
                if (button == TypeButtons.E_CHOICE_TYPE)
                    Get_mode_From_button();
                break;
        }
        if (button == TypeButtons.E_BACK_TYPE)
        {
            GoToMainMode();
        }
    }

    public void GoToMainMode()
    {
        uint uc_i;
        mode = Modes.E_MAIN_MODE;

        for (uc_i = 0; uc_i < C_N_OF_BUTTONS_CHOICE; uc_i++)
        {
            img_Info_button[uc_i].enabled = true;
            img_Info_button[uc_i].sprite = img_main_menu[uc_i];
            img_Cover_button[uc_i].enabled = true;
        }

        img_Cover_button[C_BUTTON_BACK].enabled = false;
        img_Info_button[C_BUTTON_BACK].enabled = false;

    }

    void Show2tramLines(uint ui_tram_line)
    {
        uint uc_i;
        for (uc_i = 0; uc_i < C_N_OF_BUTTONS_CHOICE; uc_i++)
        {
            if (uc_i < 2)
            {
                img_Info_button[uc_i].sprite = img_Tram_Lines[uc_i + (2 * ui_tram_line)];
                img_Info_button[uc_i].enabled = true;
                img_Cover_button[uc_i].enabled = true;
            }
            else
            {
                img_Info_button[uc_i].enabled = false;
                img_Cover_button[uc_i].enabled = false;
            }
        }

    }

    void Get_mode_From_button()
    {
        uint uc_i;

        switch (ui_Data)
        {
            case 0:
                mode = Modes.E_STIB_MODE;
                Buttons_Manager.Stib_Button();
                for (uc_i = 0; uc_i < C_N_OF_BUTTONS_CHOICE; uc_i++)
                {
                    if(uc_i < C_N_OF_STIB_CHOICES / 2)
                    {
                        img_Info_button[uc_i].sprite = img_2_STIB_choices[uc_i];
                        img_Info_button[uc_i].enabled = true;
                        img_Cover_button[uc_i].enabled = true;
                    }
                    else
                    {
                        img_Info_button[uc_i].enabled = false;
                        img_Cover_button[uc_i].enabled = false;
                    }
                }


                img_Cover_button[C_BUTTON_BACK].enabled = true;
                img_Info_button[C_BUTTON_BACK].enabled = true;
                break;
            case 1:
                mode = Modes.E_VILLO_MODE;
                Buttons_Manager.VilloButton();

                break;
            case 2:
                mode = Modes.E_SCOOTY_MODE;
                Buttons_Manager.ScootyButton();
                break;
            case 3:
                mode = Modes.E_SNCB_MODE;
                Buttons_Manager.Show_GC();
                break;
            case 4:
                mode = Modes.E_METEO_MODE;
                Buttons_Manager.MeteoButton();
                break;
            case 5:
                mode = Modes.E_TRAFIC_MODE;
                Buttons_Manager.TraficButton();
                break;
        }

        //Update_Led_Buttons();
    }


    void Get_Type_Button_pressed(uint pressed_button)
    {
        uint uc_i;
        //TODO: Mofifier ici pour avoir de meilleures choses avec les boutons
        switch (pressed_button)
        {
            case C_BUTTON_BACK:
                button = TypeButtons.E_BACK_TYPE;
                for (uc_i = 0; uc_i < C_N_OF_BUTTONS_CHOICE; uc_i++)
                {
                    img_Info_button[uc_i].sprite = img_main_menu[uc_i];
                    img_Info_button[uc_i].enabled = true;
                }
                break;
            default:
                button = TypeButtons.E_CHOICE_TYPE;
                break;
        }
    }

    void Init_port()
    {
        // COM number larger than 9, add prefix \\\\.\\. 
        string[] str_port_name = SerialPort.GetPortNames();

        foreach (string port in str_port_name)
        {
            _port = new SerialPort()
            {
                PortName = port,
                BaudRate = 9600,
                ReadTimeout = 500
            };

            _port.Open();
            _port.DiscardInBuffer();
            _port.DiscardOutBuffer();

            if (_port.IsOpen)
            {
                _looping = true;
                portReadingThread = new Thread(Read_Bus);
                portReadingThread.Start();
                break;
            }

        }
    }

    void Read_Bus()
    {
        // Start reading the data coming through the serial port.
        _port.DiscardInBuffer();
        bool is_Open = true;
        int BytesToRead = 0;
        while (_looping)
        {
            try
            { is_Open = _port.IsOpen; }
            catch
            { }

            if (is_Open)
            {
                /*try
                { BytesToRead = _port.BytesToRead; }
                catch { }*/
                if (BytesToRead >= 0)
                {

                    try
                    {
                        ui_Data = (uint)_port.ReadChar(); // blocking call
                        if (ui_Data <= 10)//parce qu'on recoit 63. modifier
                        {
                            Data_Received = true;
                        }
                    }
                    catch
                    {}
                }
            }
            else
            {
                try
                { _port.Open(); }
                catch
                { is_Open = false; }
            }
            Thread.Sleep(10);
        }
    }    
}