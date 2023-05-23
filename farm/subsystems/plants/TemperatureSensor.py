from interfaces.sensors import ISensor, AReading
from .library.grove_temperature_humidity_aht20 import GroveTemperatureHumidityAHT20 as Sensor




class TemperatureSensor(ISensor):
    SENSOR_BUS = 4
    SENSOR_ADDRESS = 0x38

    def __init__(self, gpio: int, model: str, type: AReading.Type):
        super().__init__(gpio, model, type)

        self.reading_type: AReading.Type = AReading.Type.TEMPERATURE
        self.reading_unit = AReading.Unit.CELCIUS
        self.sensor = Sensor(bus= TemperatureSensor.SENSOR_BUS, address= TemperatureSensor.SENSOR_ADDRESS)


    def read_sensor(self) -> AReading:
        (temp, humid) = self.sensor.read()
        self.value = round(temp, 2)
        return AReading(AReading.Type.TEMPERATURE, AReading.Unit.CELCIUS, {"value":  self.value})
        
    
if __name__ == "__main__":
    temp = TemperatureSensor(100, "AHT20", AReading.Type.HUMIDITY )
    while True:
        print(temp.read_sensor())