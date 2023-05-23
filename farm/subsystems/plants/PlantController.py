from .Fan import Fan
from .LED import Led
from .HumiditySensor import HumiditySensor
from .TemperatureSensor import TemperatureSensor
from .LiquidLevelSensor import LiquidLevelSensor
from .SoilMoistureSensor import SoilMoistureSensor
from interfaces.sensors import AReading, ISensor
from interfaces.actuators import ACommand, IActuator
from time import sleep
import json
from interfaces.subsystem import ISubsystem
from Constants.Pins import PlantGPIOPins as Pins

class PlantSystem(ISubsystem):
    def __init__(self) -> None:
        self._sensors: list[ISensor] = self._initialize_sensors()
        self._actuators : list[IActuator] = self._initialize_actuators()
        self.sensorReadings : list[AReading] = []

    def _initialize_sensors(self)-> list[ISensor]:
        
        return [
        

            LiquidLevelSensor(Pins.LIQUIDLEVEL_SENSOR, "Water-Level-Sensor", AReading.Type.WATER_LEVEL ),
            SoilMoistureSensor(Pins.SOILMOISTURE_SENSOR, "Soil-Moisture-Sensor",AReading.Type.MOISTURE),

            HumiditySensor(Pins.HUMIDITY_AND_TEMPERATURE_SENSOR,"AHT20", AReading.Type.HUMIDITY ),
            TemperatureSensor(Pins.HUMIDITY_AND_TEMPERATURE_SENSOR, "AHT20", AReading.Type.HUMIDITY )
        ]

    def _initialize_actuators(self)-> list[IActuator]:
        return [
            Led(Pins.LED, ACommand.Type.LED, initial_state={"value": Led.LIGHT_OFF}),
            Fan(Pins.FAN, ACommand.Type.FAN, initial_state={"value" : Fan.FAN_OFF})
        ]

    def read_sensors(self) -> list[AReading]:
        #reset list
        print("\n-------------------------PLANTS SENSORS-------------------------")

        
        readings: list[AReading] = []
        for x in range(len(self._sensors)):
            print(self._sensors[x].read_sensor())
            readings.append(self._sensors[x].read_sensor())    
        self.sensorReadings = readings
        return self.sensorReadings
    
    def control_actuators(self, command: ACommand)-> bool:
        if (command.target_type == ACommand.Type.FAN):
            for x in range(len(self._actuators)):
                if(self._actuators[x].type == ACommand.Type.FAN):
                    self._actuators[x].control_actuator(command.data)

        if (command.target_type == ACommand.Type.LED):
            for x in range(len(self._actuators)):
                if(self._actuators[x].type == ACommand.Type.LED):
                    return self._actuators[x].control_actuator(command.data)   

        return False    
        

        

class Sensors(object):
    def __init__(self, sensors : list[str]) -> None:
        self.sensors = sensors


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
        sleep(2)


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
        temp : list = []

        plant = PlantSystem()
        readings = plant.read_sensors()
        for x in range(len(readings)):
            temp.append(readings[x].export_json())
            json_string = json.dumps(temp[x])
            
        
        sensors = Sensors(temp)
        json_string = json.dumps(sensors, default=lambda o: o.__dict__, indent=2)
        print(json_string)



        
