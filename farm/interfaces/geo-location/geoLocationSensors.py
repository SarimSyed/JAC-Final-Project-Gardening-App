from abc import ABC, abstractmethod
from enum import Enum


class GeoLocationReading:
    """Class for sensor readings. Defines possible types of readings and reading units using enums.
    """

    class Type(str, Enum):
        """Enum defining all possible types of readings that sensors might make.
        """
        # Add new reading types here.
        TEMPERATURE = 'temperature'
        HUMIDITY = 'humidity'
        GPSLOCATION = 'gps-location'
        PITCH = 'pitch'
        ROLL_ANGLE = 'roll-angle'
        VIBRATION = 'vibration'

    class Unit(str, Enum):
        """Enum defining all possible units for sensor measuremens.
        """
        # Add new reading units here.
        CELCIUS = '°C'
        HUMIDITY = '% HR'
        LOCATION = 'loc'
        PITCH = 'Hz'
        ROLL_ANGLE = '°'
        VIBRATION = 'm/s2'

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

    def export_json(self) -> str:
        """Exports a reading as a json encoded string
        :return str: json string representation of the reading
        """
        return {"value": self.value, "unit": self.reading_unit.value}.__str__()


class ISensor(ABC):
    """Interface for all sensors.
    """

    # Properties to be initialized in constructor of implementation classes
    _sensor_model: str
    reading_type: GeoLocationReading.Type

    @abstractmethod
    def __init__(self, gpio: int,  model: str, type: GeoLocationReading.Type):
        """Constructor for Sensor  class. May be called from childclass.
        :param str model: specific model of sensor hardware. Ex. AHT20 or LTR-303ALS-01
        :param ReadingType type: Type of reading this sensor produces. Ex. 'TEMPERATURE'
        """

    @abstractmethod
    def read_sensor(self) -> GeoLocationReading:
        """Takes a reading form the sensor
        :return list[AReading]: List of readinds measured by the sensor. Most sensors return a list with a single item.
        """
        pass