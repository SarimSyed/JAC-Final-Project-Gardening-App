from grove.grove_moisture_sensor import GroveMoistureSensor
from grove.adc import ADC
from sensors import ISensor, AReading

sensor = 1  # The sensor is connected to GrovePi port D0

water = GroveMoistureSensor(sensor)
adc = ADC()

class SoilMoistureSensor(ISensor):
    def __init__(self, gpio: int, model: str, type: AReading.Type):
        super().__init__(gpio, model, type)

        self._sensor_model = model or "Soil-Moisture-Sensor"
        self.reading_type


