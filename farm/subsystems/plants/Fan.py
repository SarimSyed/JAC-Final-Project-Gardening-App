from interfaces.actuators import IActuator, ACommand
from gpiozero import OutputDevice as Relay
from time import sleep


class Fan(IActuator):

    FAN_OFF = "off"
    FAN_ON = "on"

    def __init__(self, gpio: int, type: ACommand.Type, initial_state: dict) -> None:
        super().__init__(gpio, type, initial_state)

        self.type = type or ACommand.Type.LED
        self._current_state = initial_state
        
        self.isOn : bool = str(initial_state['value']).lower() == Fan.FAN_ON 
        
        self.relay : Relay = Relay(initial_value=self.isOn, pin=gpio)

    def relay_control(self) -> None:
        if self._current_state['value'] == Fan.FAN_ON:
            self.relay.on()
        elif self._current_state['value'] == Fan.FAN_OFF:
            self.relay.off()

    def control_actuator(self, data: dict) -> bool:

        
        data_value = data["value"]
        # state unchanged so we just return right away
        if(self._current_state["value"] == data_value):
            return False
        
        self._current_state = data
        self.relay_control()
            
        return True 



    def validate_command(self, command: ACommand) -> bool:
        
        
        cmd = str(command.data.get("value")).lower()
        return command.target_type == self.type and (cmd == Fan.FAN_OFF or cmd == Fan.FAN_ON)



if __name__ == '__main__':
    
    on = '{"value" : "on"}'
    off = '{"value" : "off"}'
    err = '{"value" : "blink"}'
    
    fan = Fan(5, ACommand.Type.FAN, initial_state={"value" : Fan.FAN_ON})

    while True:
        
        fan.control_actuator({"value": Fan.FAN_OFF})
        sleep(2)
        fan.control_actuator({'value' : Fan.FAN_ON})
        sleep(2)