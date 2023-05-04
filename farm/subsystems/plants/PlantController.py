from Fan import Fan
from LED import Led
from HumiditySensor import HumiditySensor
from TemperatureSensor import TemperatureSensor
from LiquidLevelSensor import LiquidLevelSensor
from SoilMoistureSensor import SoilMoistureSensor
from interfaces.sensors import AReading
from actuators import ACommand
from time import sleep


if __name__ == "__main__":

    #Might need to change the gpio pin numbers for some as my device has been wonky and ive had to use
    #workarounds to make them work

    on = '{"value" : "on"}'
    off = '{"value" : "off"}'
    err = '{"value" : "blink"}'
    
    liqudSensor = LiquidLevelSensor(1, "Water-Level-Sensor", AReading.Type.WATER_LEVEL )
    soilSensor = SoilMoistureSensor(2, "Soil-Moisture-Sensor",AReading.Type.MOISTURE)
    
    #LED
    led = Led(18, ACommand.Type.LED, initial_state={"value": Led.LIGHT_ON})
    fake_msg = '{"value": "light-off"}'
    test_off_cmd = ACommand(ACommand.Type.LED, fake_msg)
    fake_msg = '{"value": "light-on"}'
    test_on_cmd = ACommand(ACommand.Type.LED, fake_msg)

    #Fan
    fan = Fan(5, ACommand.Type.FAN, initial_state={"value" : Fan.FAN_ON})
    fan_on_msg = '{"value": "on"}'
    fan_off_msg = '{"value": "off"}'
    fan_on_cmd = ACommand(ACommand.Type.FAN, fan_on_msg)
    fan_off_cmd = ACommand(ACommand.Type.FAN, fan_off_msg)
    
    humid = HumiditySensor(6, "AHT20", AReading.Type.HUMIDITY )
    temperatureSensor = TemperatureSensor(1, "AHT20", AReading.Type.HUMIDITY )
    while True:
        sleep(1)


        test = led.validate_command(test_off_cmd)
        if test:
            led.control_actuator({"value": Led.LIGHT_OFF})
        sleep(2)
        # led.control_actuator({'value':Led.LIGHT_BRIGHT})
        if led.validate_command(test_on_cmd):
            led.control_actuator({"value": Led.LIGHT_ON})

        if fan.validate_command(fan_off_cmd):
            fan.control_actuator({"value": Fan.FAN_OFF})
        sleep(2)
        if fan.validate_command(fan_on_cmd):
            fan.control_actuator({"value" : Fan.FAN_ON})
        sleep(2)


        print("Sensors :\n")
        print(temperatureSensor.read_sensor())
        print(humid.read_sensor())
        print(soilSensor.read_sensor())
        print(liqudSensor.read_sensor())
        
