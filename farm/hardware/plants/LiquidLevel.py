from sensors import ISensor, AReading
from grove.adc import ADC
from grove.gpio import GPIO
from time import sleep


class WaterSensor(ISensor):
    def __init__(self, gpio: int, model: str, type: AReading.Type.WATER_LEVEL):
        super().__init__(gpio, model, type)

        #self.gpio = GPIO(gpio)
        self.adc = ADC(0x03)
        self._sensor_model = model or "Liquid-Level-Sensor"
        self.reading_type = type or AReading.Type.WATER_LEVEL
        

    def read_sensor(self) -> AReading:
        self.value = self.adc.read(2)
        return AReading(self.reading_type, AReading.Unit.WATER_LEVEL, self.value) 


if __name__ == "__main__":
    temp = WaterSensor(4, "Water-Level-Sensor", AReading.Type.WATER_LEVEL )
    while True:
        print(temp.read_sensor().value)
        sleep(0.5)
        
