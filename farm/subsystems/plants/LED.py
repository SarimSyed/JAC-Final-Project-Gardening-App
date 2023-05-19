from grove.grove_ws2813_rgb_led_strip import GroveWS2813RgbStrip
from interfaces.actuators import IActuator, ACommand
from time import sleep

class Led(IActuator):

    ON = "on"
    OFF = "off"
    LIGHT_ON = 'on'
    LIGHT_OFF = 'off'
    LIGHT_BRIGHT = 'max-brightness'
    LIGHT_MEDIUM = 'mid-brightness'
    NUM_OF_LEDS = 10

    def __init__(self, gpio: int, type: ACommand.Type, initial_state: dict) -> None:
        """Create an instance of this class which will allow the LED to be manipulated

        Args:
            gpio (int): The gpio pin the LED is connected to ()
            type (ACommand.Type): The ACommand object type that should be used
            initial_state (dict): What state to the LED should be in from the start (defaults to off)
        """
        super().__init__(gpio, type, initial_state)

        self.type :ACommand.Type = type or ACommand.Type.LED
        self._current_state = initial_state

        self.led :GroveWS2813RgbStrip = GroveWS2813RgbStrip(gpio, count= Led.NUM_OF_LEDS)
        
        
        self.brightness : str = Led.LIGHT_BRIGHT
        


    def control_actuator(self, data: dict) -> bool:
        

        
        data_value = data["value"]

        if(self._current_state == data):
            return False
        
        if data_value == Led.LIGHT_OFF:
            self.turn_off()
            self._current_state = data_value
            return True
        
        if(data_value == Led.LIGHT_MEDIUM and self._current_state == Led.LIGHT_ON):
            self.set_med_brightness()
        elif(data_value == Led.LIGHT_MEDIUM and self._current_state != Led.LIGHT_ON):
            self.brightness = Led.LIGHT_MEDIUM
        
        if(data_value == Led.LIGHT_ON and self.brightness == Led.LIGHT_BRIGHT):
            self.set_max_brightness()
            self._current_state = data_value
        elif(data_value == Led.LIGHT_ON and self.brightness != Led.LIGHT_BRIGHT):
            self.set_med_brightness()
            self._current_state = data_value

        if(data_value == Led.LIGHT_BRIGHT and self._current_state == Led.LIGHT_ON):
            self.set_max_brightness()
            self.brightness = Led.LIGHT_BRIGHT
        elif(data_value == Led.LIGHT_MEDIUM and self._current_state != Led.LIGHT_ON):
            self.brightness = Led.LIGHT_MEDIUM
            self._current_state = data_value

        return True

    
    def validate_command(self, command: ACommand) -> bool:
        
        cmd = str(command.data.get("value")).lower()
        
        return command.target_type == self.type and (cmd == Led.LIGHT_BRIGHT or cmd == Led.LIGHT_MEDIUM or cmd == Led.LIGHT_OFF or cmd == Led.LIGHT_ON)

    def set_max_brightness(self):
        for i in range(self.led.numPixels()):
            self.led.setPixelColorRGB( red=255, blue=255, green=255, n=i)
            self.led.show()

    def set_med_brightness(self):
        for i in range(self.led.numPixels()):
            self.led.setPixelColorRGB( red=100, blue=100, green=100, n=i)
            self.led.show()


    def turn_off(self):
        for i in range(self.led.numPixels()):
            self.led.setPixelColorRGB( red=0, blue=0, green=0, n=i)
            self.led.show()






if __name__ == "__main__":
    print("initialize")
    led = Led(18, ACommand.Type.LED, initial_state={"value": Led.LIGHT_ON})
    print("values")
    fake_msg = '{"value": "off"}'
    test_off_cmd = ACommand(ACommand.Type.LED, fake_msg)
    fake_msg = '{"value": "on"}'
    test_on_cmd = ACommand(ACommand.Type.LED, fake_msg)
    print("loop")
    while True:
        print("loop")
        test = led.validate_command(test_off_cmd)
        if test:
            led.control_actuator({"value": Led.LIGHT_OFF})
        sleep(2)

        if led.validate_command(test_on_cmd):
            led.control_actuator({"value": Led.LIGHT_ON})

        sleep(2)

        
