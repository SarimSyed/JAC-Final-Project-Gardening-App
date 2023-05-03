import time
from grove.adc import ADC
from grove import grove_loudness_sensor

import math
import sys

import seeed_python_reterminal.core as rt
from time import sleep
from farm.interfaces.security.securitySensors import ISensor, SecurityReading


class Noise(ISensor):
    """ The noise sensor used in the security subsystem.
    Args:
        ISensor (_type_): Implements the ISensor interface.
    """

    ADDRESS = 0x04
    LOW_VALUES = [80,220]
    MED_VALUES = [100,180]
    HIGH_VALUES = [135,155]

    def __init__(self, type: SecurityReading.Type = SecurityReading.Type.NOISE, model: str = "Grove-Loudness Sensor V0.9b"):
        """Constructor for the Noise class. Defines the interface's properties. 
        :param str model: specific model of sensor hardware. Ex. AHT20 or LTR-303ALS-01
        :param ReadingType type: Type of reading this sensor produces. Ex. 'TEMPERATURE'
        """
        self._sensor_model = model
        self.reading_type = type

        #Sensitivity
        self._sensitivity = SecurityReading.Sensitivity.MEDIUM.value

        #Inizialize adc
        self.noise_adc = ADC(Noise.ADDRESS)

    def read_sensor(self) -> SecurityReading:
        """Takes a reading from the noise sensor
        :return list[AReading]: List of readings measured by the sensor. Most sensors return a list with a single item.
        """

        #Get the sensor reading
        noise_value = self.noise_adc.read(0)
        #Low sensitivity
        if(self._sensitivity == SecurityReading.Sensitivity.LOW.value):
            if(noise_value < Noise.LOW_VALUES[0] or noise_value > Noise.LOW_VALUES[1]):
                return SecurityReading(SecurityReading.Type.NOISE, {"value": SecurityReading.Response.DETECTED.value})
            else:
                return SecurityReading(SecurityReading.Type.NOISE, {"value": SecurityReading.Response.NOT_DETECTED.value})
        
        #Medium sensitivity
        elif(self._sensitivity == SecurityReading.Sensitivity.MEDIUM.value):
            if(noise_value < Noise.MED_VALUES[0] or noise_value > Noise.MED_VALUES[1]):
                return SecurityReading(SecurityReading.Type.NOISE, {"value": SecurityReading.Response.DETECTED.value})
            else:
                return SecurityReading(SecurityReading.Type.NOISE, {"value": SecurityReading.Response.NOT_DETECTED.value})
        
        #High sensitivity
        else:
            if(noise_value < Noise.HIGH_VALUES[0] or noise_value > Noise.HIGH_VALUES[1]):
                return SecurityReading(SecurityReading.Type.NOISE, {"value": SecurityReading.Response.DETECTED.value})
            else:
                return SecurityReading(SecurityReading.Type.NOISE, {"value": SecurityReading.Response.NOT_DETECTED.value})
        
if __name__ == "__main__":
    noise = Noise()
    while True:
        print(noise.read_sensor())
        sleep(1)