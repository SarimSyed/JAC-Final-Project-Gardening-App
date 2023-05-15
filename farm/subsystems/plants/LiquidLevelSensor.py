from interfaces.sensors import ISensor, AReading
from .library.grove_water_sensor import GroveWaterSensor
from time import sleep


class LiquidLevelSensor(ISensor):
    def __init__(self, gpio: int, model: str, type: AReading.Type):
        """Liquid level sensor detects the level of water detected by the sensor

        Args:
            gpio (int): gpio pin number
            model (str): the model of sensor being used
            type (AReading.Type.WATER_LEVEL): Type of AReading object being used to stores and format data
        """
        super().__init__(gpio, model, type)

        #self.gpio = GPIO(gpio)
        
        self._sensor_model = model or "Liquid-Level-Sensor"
        self.sensor = GroveWaterSensor(gpio)
        self.reading_type = type or AReading.Type.WATER_LEVEL
        

    def read_sensor(self) -> AReading:
        """Read sensor and return what percentage of the sensor is submerged.

        Returns:
            AReading: AReading object of type WATER_LEVEL that stores what percentage of the water sensor is submerged.
        """
        self.value = self.sensor.value/10 
        return AReading(self.reading_type, AReading.Unit.WATER_LEVEL, {"value":  self.value}) 


if __name__ == "__main__":
    #A0 port = pin 1
    temp = LiquidLevelSensor(1, "Water-Level-Sensor", AReading.Type.WATER_LEVEL )
    while True:
        print(temp.read_sensor())
        sleep(1)
        
