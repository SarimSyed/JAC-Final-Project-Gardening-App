from sensors import ISensor, AReading
from grove.grove_temperature_humidity_aht20 import GroveTemperatureHumidityAHT20 as Sensor


class HumiditySensor(ISensor):
    SENSOR_ADDRESS = 0x38
    def __init__(self, gpio: int, model: str, type: AReading.Type):
        super().__init__(gpio, model, type)

        self.reading_type = type or AReading.Type.HUMIDITY
        self._sensor_model = model or "AHT20"
        self.sensor = Sensor(address= HumiditySensor.SENSOR_ADDRESS, bus=gpio)

    def read_sensor(self) -> AReading:
        (temp, humid) = self.sensor.read()
        self.value = humid
        return AReading(AReading.Type.HUMIDITY, AReading.Unit.HUMIDITY, humid)


if __name__ == "__main__":
    temp = HumiditySensor(6, "AHT20", AReading.Type.HUMIDITY )
    while True:
        print(temp.read_sensor().value)