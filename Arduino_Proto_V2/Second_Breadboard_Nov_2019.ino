#define C_N_BUTTONS 7
#define C_BUTTON_MASK (1<< (C_N_BUTTONS))-1

/*
1 0X01  B0000 0001
2 0X03  B0000 0011
3 0X07  B0000 0111
4 0X0F  B0000 1111
*/


void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  DDRB = 0x00;
  DDRD = 0x00;
}
unsigned char HW_Button[C_N_BUTTONS];
unsigned char Port_State;
unsigned char uc_SW_Port_State;
unsigned char Last_SW_Port_State;
unsigned char Clicked_Button;
void loop() {
  
  
  uc_SW_Port_State =  get_Button_state(C_N_BUTTONS,HW_Button) & C_BUTTON_MASK;
  if( (uc_SW_Port_State != Last_SW_Port_State) && (uc_SW_Port_State != C_BUTTON_MASK) )
  {
    Clicked_Button = get_button_from_port(uc_SW_Port_State);
    Serial.write(Clicked_Button);
    delay(100);
  }
  Last_SW_Port_State = uc_SW_Port_State;
}

void refresh_HW_Buttons()
{
  HW_Button[0] = (PIND)&(1<<2);
  HW_Button[1] = (PIND)&(1<<3);
  HW_Button[2] = (PIND)&(1<<4);
  HW_Button[6] = (PIND)&(1<<5);
  HW_Button[5] = (PIND)&(1<<6);
  HW_Button[4] = (PIND)&(1<<7);
  HW_Button[3] = (PINB)&(1<<0);
}
//get all button state in a single byte 
unsigned char get_Button_state (unsigned char lenght, unsigned char* ptr_ports)
{
  refresh_HW_Buttons();
  unsigned char i;
  unsigned char* ptr_data = ptr_ports;
  unsigned char SW_Port_state = 0X00;
  for(i=0;i<C_N_BUTTONS;i++)
  {
    if(ptr_ports[i])
    {
      SW_Port_state |= 1<<i;
    }
  }
  return SW_Port_state;
  delay(1000);
}

unsigned char get_button_from_port(unsigned char port)
{
  unsigned char uc_i;
  unsigned char uc_bit_checked;
  for(uc_i = 0; uc_i < C_N_BUTTONS ; uc_i++)
  {
    uc_bit_checked = (port >> uc_i) & 0x01;
    if( uc_bit_checked == 0)
    {
      return uc_i;
    }
  }
}
