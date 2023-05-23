from abc import ABC, abstractmethod
from enum import Enum
import json

"""
We are using the 'AReading' and 'ISensor' classes from Mauricio Andres Buschinelli: https://github.com/maujac.
"""
class AReading:
    """Class for sensor readings. Defines possible types of readings and reading units using enums.
    """

    class Type(str, Enum):
        """Enum defining all possible types of readings that sensors might make.
        """
        # Add new reading types here.
        #Plant
        TEMPERATURE = 'temperature'
        HUMIDITY = 'humidity'
        WATER_LEVEL = 'water-level-sensor'
        MOISTURE = 'soil-moisture'

        #Security
        DOOR = "door"
        LUMINOSITY = "luminosity"
        MOTION = "motion"
        NOISE = "noise"
        
        #Geo-Location
        GPSLOCATION = 'gps-location'
        PITCH = 'pitch'
        ROLL_ANGLE = 'roll-angle'
        VIBRATION = 'vibration'

    class Response(str, Enum):
        DETECTED = "detected"
        NOT_DETECTED = "not detected"

    class Sensitivity(str, Enum):
        LOW = "low"
        HIGH = "high"
        MEDIUM = "medium"
    

    class Unit(str, Enum):
        """Enum defining all possible units for sensor measuremens.
        """
        # Add new reading units here.
        #Plant
        CELCIUS = 'C'
        HUMIDITY = '% HR'
        WATER_LEVEL = "% submerged"
        MOISTURE = "mV"

        #Geo-Location
        LOCATION = 'loc'
        PITCH = '°'
        ROLL_ANGLE = '°'
        VIBRATION = 'm/s2'

        #Security
        NONE = ""

        

    def __init__(self, type: Type, unit: Unit, value: dict) -> None:
        """Create new AReading based on a type of reading its units and value
        :param Type type: Type of reading taken from Type enum.
        :param Unit unit: Readings units taken from Unit enum.
        :param float value: Value of the reading.
        """
        self.reading_type = type
        self.reading_unit = unit
        self.value = value

    def __repr__(self) -> str:
        """String representation of a reading object
        :return str: string representing reading information
        """
        return f"{self.reading_type}: {self.value} {self.reading_unit}"

    def export_json(self) -> dict[Type, dict[str, dict[str, str]]]:
        """Exports a reading as a json encoded string
        :return str: json string representation of the reading
        """
        temp = self.value["value"]
        return {self.reading_type:{"value": temp, "unit": self.reading_unit.value}} # type: ignore



class ISensor(ABC):
    """Interface for all sensors.
    """

    # Properties to be initialized in constructor of implementation classes
    _sensor_model: str
    reading_type: AReading.Type

    @abstractmethod
    def __init__(self, gpio: int,  model: str, type: AReading.Type):
        """Constructor for Sensor  class. May be called from childclass.
        :param str model: specific model of sensor hardware. Ex. AHT20 or LTR-303ALS-01
        :param ReadingType type: Type of reading this sensor produces. Ex. 'TEMPERATURE'
        """

    @abstractmethod
    def read_sensor(self) -> AReading:
        """Takes a reading form the sensor
        :return list[AReading]: List of readinds measured by the sensor. Most sensors return a list with a single item.
        """
        pass