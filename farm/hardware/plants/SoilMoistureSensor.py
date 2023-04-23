from grove.grove_moisture_sensor import GroveMoistureSensor
from grove.adc import ADC
from sensors import ISensor, AReading
from grove.i2c import i2c_msg
import RPi.GPIO as gpio 
from time import sleep

sensor = 6  # The sensor is connected to GrovePi port D0




class SoilMoistureSensor(ISensor):
    def __init__(self, gpio: int, model: str, type: AReading.Type):
        super().__init__(gpio, model, type)

        self._sensor_model = model or "Soil-Moisture-Sensor"
        self.reading_type = type
        self.sensor = GroveMoistureSensor(sensor)
        temp = "breakpoint"

    def read_sensor(self) -> AReading:
        
        
        
        return self.sensor.moisture

if __name__ == "__main__":
    temp = SoilMoistureSensor(1, "Soil-Moisture-Sensor",AReading.Type.SOIL_WATER_MOISTURE)
    
    while True:
        print(temp.read_sensor())
        sleep(1)
        


