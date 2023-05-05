from time import sleep
from gpiozero import Button
from interfaces.sensors import ISensor, AReading


class Door(ISensor):
     """ The door of the container used in the security subsystem.
     Args:
         ISensor (_type_): Implements the ISensor interface.
     """
     OPEN = "open"
     CLOSED = "closed"

     def __init__(self, gpio: int, model:str = "SEC-100 Magnetic Door Sensor Reed Switch", type:AReading.Type = AReading.Type.DOOR):
        """Constructor for the Door class. Defines the interface's properties. 
        :param str model: specific model of sensor hardware. Ex. AHT20 or LTR-303ALS-01
        :param ReadingType type: Type of reading this sensor produces. Ex. 'TEMPERATURE'
        """

        #Inizialize button
        self.button = Button(gpio)

        self._sensor_model = model 
        self.reading_type = type

     def read_sensor(self) -> AReading:
        """Takes a reading from the door state sensor
        :return list[AReading]: List of readings measured by the sensor. Most sensors return a list with a single item.
        """

        #Get the sensor reading and returns appropriate value
        if(self.button.is_held == True):
               door_value = Door.CLOSED
               return AReading(AReading.Type.DOOR, AReading.Unit.NONE, {"value": door_value})
        else:
               door_value = Door.OPEN
               return AReading(AReading.Type.DOOR,  AReading.Unit.NONE, {"value": door_value})
        
if __name__ == "__main__":
     
     door = Door(16)
     while True:
          print(door.read_sensor())
          sleep(1)