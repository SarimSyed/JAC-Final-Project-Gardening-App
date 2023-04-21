from sensors import ISensor, AReading
from grove.grove_temperature_humidity_aht20 import GroveTemperatureHumidityAHT20 as Sensor





class HumiditySensor(ISensor) :


    SENSOR_BUS = 4
    SENSOR_ADDRESS = 0x38
    def __init__(self, gpio: int, model: str, type: AReading.Type) -> None:
        super().__init__(gpio, model, type)
        
        self.reading_type: AReading.Type = AReading.Type.HUMIDITY
        self.reading_unit = AReading.Unit.HUMIDITY
        self.sensor = Sensor(bus= HumiditySensor.SENSOR_BUS, address= HumiditySensor.SENSOR_ADDRESS)

        


    def read_sensor(self) -> list[AReading]:
        
        #Return humidity
        (temp, humid) = self.sensor.read()
        self.value = humid                                                                                    

        return humid
    
if __name__ == "__main__":
    temp = HumiditySensor(6, "AHT20", AReading.Type.HUMIDITY )
    while True:
        print(temp.read_sensor())
