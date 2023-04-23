from Fan import Fan
from LED import Led
from HumiditySensor import HumiditySensor
from TemperatureSensor import TemperatureSensor
from LiquidLevelSensor import LiquidLevelSensor
from SoilMoistureSensor import SoilMoistureSensor
from sensors import AReading
from actuators import ACommand
from time import sleep


if __name__ == "__main__":
    on = '{"value" : "on"}'
    off = '{"value" : "off"}'
    err = '{"value" : "blink"}'
    
    liqudSensor = LiquidLevelSensor(4, "Water-Level-Sensor", AReading.Type.WATER_LEVEL )
    soilSensor = SoilMoistureSensor(1, "Soil-Moisture-Sensor",AReading.Type.MOISTURE)
    led = Led(18, ACommand.Type.LED, initial_state={"value": Led.LIGHT_ON})

    
    fan = Fan(5, ACommand.Type.FAN, initial_state={"value" : Fan.FAN_ON})
    humid = HumiditySensor(6, "AHT20", AReading.Type.HUMIDITY )
    temperatureSensor = TemperatureSensor(1, "AHT20", AReading.Type.HUMIDITY )
    while True:
        print("Actuators:\n")
        sleep(1)
        fan.control_actuator({"value": Fan.FAN_OFF})
        sleep(1)
        fan.control_actuator({'value' : Fan.FAN_ON})

        led.control_actuator({"value": Led.LIGHT_OFF})
        sleep(2)
        led.control_actuator({'value':Led.LIGHT_BRIGHT})

        led.control_actuator({"value": Led.LIGHT_ON})
        sleep(2)
        led.control_actuator({'value': Led.LIGHT_MEDIUM})
        sleep(2)

        print("Sensors :\n")
        print(temperatureSensor.read_sensor())
        print(humid.read_sensor())
        print(soilSensor.read_sensor())
        print(liqudSensor.read_sensor())
