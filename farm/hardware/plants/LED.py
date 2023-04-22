from grove.grove_ws2813_rgb_led_strip import GroveWS2813RgbStrip
from actuators import IActuator, ACommand

class Led(IActuator):

    ON = "on"
    OFF = "off"
    LIGHT_ON = 'light-on'
    LIGHT_OFF = 'light-off'
    LIGHT_BRIGHT = 'max-brightness'
    LIGHT_MEDIUM = 'mid-brightness'
    NUM_OF_LEDS = 10

    def __init__(self, gpio: int, type: ACommand.Type, initial_state: dict) -> None:
        super().__init__(gpio, type, initial_state)

        self.type = type or ACommand.Type.LED
        self._current_state = initial_state
        self.led = GroveWS2813RgbStrip(gpio, count= Led.NUM_OF_LEDS)
        self.brightness = Led.LIGHT_BRIGHT

    def control_actuator(self, data: dict) -> bool:
        
        data_value = data["value"]

        if(self._current_state == data_value):
            return False
        
        if data_value == Led.LIGHT_OFF:
            self.turn_off()
            return True
        if(data_value == Led.LIGHT_MEDIUM and self._current_state == Led.LIGHT_ON):
            self.set_med_brightness()
        elif(data_value == Led.LIGHT_MEDIUM and self._current_state != Led.LIGHT_ON):
            self.brightness = Led.LIGHT_MEDIUM
        
        if(data_value == Led.LIGHT_BRIGHT and self._current_state == Led.LIGHT_ON):
            self.set_max_brightness()
        elif(data_value == Led.LIGHT_MEDIUM and self._current_state != Led.LIGHT_ON):
            self.brightness = Led.LIGHT_BRIGHT

    
    def validate_command(self, command: ACommand) -> bool:
        
        cmd = str(command.data.get("value")).lower()
        
        return command.target_type == self.type and (cmd == Led.LIGHT_BRIGHT or cmd == Led.LIGHT_MEDIUM or cmd == Led.LIGHT_OFF or cmd == Led.LIGHT_ON)

    def set_max_brightness(self):
        for i in range(self.led.numPixels()):
            self.led.setPixelColorRGB( red=255, blue=255, green=255, n=i)

    def set_med_brightness(self):
        for i in range(self.led.numPixels()):
            self.led.setPixelColorRGB( red=100, blue=100, green=100, n=i)

    def turn_off(self):
        for i in range(self.led.numPixels()):
            self.led.setPixelColorRGB( red=0, blue=0, green=0, n=i)



# # Connect the LED strip to a digital port, e.g. D18
# led_strip = GroveWS2813RgbStrip(18, count=10)

# # Set the color of all pixels to red
# for i in range(led_strip.numPixels()):
#     led_strip.setPixelColorRGB( red=0, blue=0, green=0, n=i)

# # Update the LED strip with the new color values
# led_strip.show()

if __name__ == "__main__":
    led = Led(18, ACommand.Type.LED, initial_state=)
