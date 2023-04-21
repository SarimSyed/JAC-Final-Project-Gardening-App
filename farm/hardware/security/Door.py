import seeed_python_reterminal.core as rt
from time import sleep
from gpiozero import Button
from sensors import ISensor, AReading


class Door(ISensor):
     """ The door of the container used in the security subsystem.
     Args:
         ISensor (_type_): Implements the ISensor interface.
     """
     OPEN = "open"
     CLOSED = "closed"

     def __init__(self, gpio: int, type: AReading.Type = AReading.Type.DOOR, model: str = "SEC-100 Magnetic Door Sensor Reed Switch"):
        """Constructor for the Door class. Defines the interface's properties. 
        :param str model: specific model of sensor hardware. Ex. AHT20 or LTR-303ALS-01
        :param ReadingType type: Type of reading this sensor produces. Ex. 'TEMPERATURE'
        """

        #Inizialize button
        self._button = Button(gpio)

        self._sensor_model = model
        self.reading_type = type

     def read_sensor(self) -> list[AReading]:
        """Takes a reading from the door state sensor
        :return list[AReading]: List of readings measured by the sensor. Most sensors return a list with a single item.
        """

        #Get the sensor reading and returns appropriate value
        if(self._button.is_held == True):
               door_value = Door.CLOSED
               return [AReading(AReading.Type.DOOR, 
                                AReading.Unit.NONE, door_value)]
        else:
               door_value = Door.OPEN
               return [AReading(AReading.Type.DOOR,
                                AReading.Unit.NONE, door_value)]
        
if __name__ == "__main__":
     door = Door(5)
     while True:
          print(door.read_sensor())
          sleep(1)