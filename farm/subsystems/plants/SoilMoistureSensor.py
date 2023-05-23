
from .library.grove_moisture_sensor import GroveMoistureSensor
from interfaces.sensors import ISensor, AReading
from time import sleep


class SoilMoistureSensor(ISensor):
    #readings on the sensor seem to be flipped so will subtract from this value instead of just taking the raw reading
    SENSOR_CORRECTION_VAL = 2200
    def __init__(self, gpio: int, model: str, type: AReading.Type):
        """SoilMoistureSensor handles the moisture sensor and allows user to read its value.

        Args:
            gpio (int): The gpio pin the sensor is in
            model (str): The model of the moisture sensor
            type (AReading.Type): The type of AReading object being used
        """
        super().__init__(gpio, model, type)

        self._sensor_model = model or "Soil-Moisture-Sensor"
        self.reading_type = type
        self.sensor = GroveMoistureSensor(gpio)


    def read_sensor(self) -> AReading:
        """Reads soil moisture sensor and returns the mV value that was read by it.
        The higher the value the wetter the soil.

        Returns:
            AReading: Type MOISTURE, returns an AReading object with the information about the moisture as well as the current value that was read.
        """
        #Currently returning the voltage reading from the 
        self.value = SoilMoistureSensor.SENSOR_CORRECTION_VAL - self.sensor.moisture
        temp = AReading(AReading.Type.MOISTURE, AReading.Unit.MOISTURE, {"value":  self.value})
        return temp

    def is_soil_moist(self) -> str:
        """Returns the moisture status of the soil

        Returns:
            str: Whether the soil is dry, moist, or wet
        """
            
        #taken from the main in grove_moisture_sensor.py module
        result = 'Not Detected'
        m : int  = self.sensor.moisture
        if 0 <= m and m < 300:
            result = 'Dry'
        elif 300 <= m and m < 600:
            result = 'Moist'
        else:
            result = 'Wet'
        return result
        
        
        

        
if __name__ == "__main__":
    temp = SoilMoistureSensor(2, "Soil-Moisture-Sensor",AReading.Type.MOISTURE)
    
    while True:
        temp.sensor.moisture
        print(temp.read_sensor())
        sleep(1)
        


