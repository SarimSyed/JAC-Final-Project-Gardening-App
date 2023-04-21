import time
from grove.adc import ADC
from grove import grove_loudness_sensor

import math
import sys

import seeed_python_reterminal.core as rt
from time import sleep
from sensors import ISensor, AReading


class Noise(ISensor):
    """ The noise sensor used in the security subsystem.
    Args:
        ISensor (_type_): Implements the ISensor interface.
    """

    ADDRESS = 0x04

    def __init__(self, type: AReading.Type = AReading.Type.NOISE, model: str = "Grove-Loudness Sensor V0.9b"):
        """Constructor for the Noise class. Defines the interface's properties. 
        :param str model: specific model of sensor hardware. Ex. AHT20 or LTR-303ALS-01
        :param ReadingType type: Type of reading this sensor produces. Ex. 'TEMPERATURE'
        """
        self._sensor_model = model
        self.reading_type = type

        #Inizialize adc
        self.noise_adc = ADC(Noise.ADDRESS)

    def read_sensor(self) -> list[AReading]:
        """Takes a reading from the noise sensor
        :return list[AReading]: List of readings measured by the sensor. Most sensors return a list with a single item.
        """

        #Get the sensor reading and returns value

        noise_value = self.noise_adc.read(0)
        return [AReading(AReading.Type.NOISE, 
                         AReading.Unit.NOISE, noise_value)]
        
if __name__ == "__main__":
    noise = Noise()
    while True:
        print(noise.read_sensor())
        sleep(1)


# class GroveLoudnessSensor:

#     def __init__(self):
#         self.adc = ADC(0x04)

#     @property
#     def value(self):
#         return self.adc.read(0)

# Grove = GroveLoudnessSensor


# def main():

#     sensor = GroveLoudnessSensor()

#     print('Detecting loud...')
#     while True:
#         value = sensor.value
#         if value > 10:
#             print("Loud value {}, Loud Detected.".format(value))
#             time.sleep(.5)

# if __name__ == '__main__':
#     main()