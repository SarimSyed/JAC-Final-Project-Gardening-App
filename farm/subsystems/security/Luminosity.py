import seeed_python_reterminal.core as rt
from time import sleep
from farm.interfaces.security.securitySensors import ISensor, SecurityReading


class Luminosity(ISensor):
    """ The luminosity in the container used in the security subsystem.
    Args:
        ISensor (_type_): Implements the ISensor interface.
    """

    def __init__(self, type: SecurityReading.Type = SecurityReading.Type.LUMINOSITY, model: str = "ReTerminal"):
        """Constructor for the Luminosity class. Defines the interface's properties. 
        :param str model: specific model of sensor hardware. Ex. AHT20 or LTR-303ALS-01
        :param ReadingType type: Type of reading this sensor produces. Ex. 'TEMPERATURE'
        """

        self._sensor_model = model
        self.reading_type = type
        self._dangervalue = 30

    def read_sensor(self) -> SecurityReading:
        """Takes a reading from the reterminal luminosity sensor
        :return list[AReading]: List of readings measured by the sensor. Most sensors return a list with a single item.
        """

        #Get the sensor reading and returns value

        light_value = rt.illuminance
        if(light_value >= self._dangervalue):
            return SecurityReading(SecurityReading.Type.LUMINOSITY, {"value": SecurityReading.Response.DETECTED.value})
        else:
            return SecurityReading(SecurityReading.Type.LUMINOSITY, {"value": SecurityReading.Response.NOT_DETECTED.value})

        
if __name__ == "__main__":
    light = Luminosity()
    while True:
        print(light.read_sensor())
        sleep(1)