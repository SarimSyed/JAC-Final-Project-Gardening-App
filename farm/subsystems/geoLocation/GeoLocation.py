from time import sleep
from .Buzzer import Buzzer
from .GPSLocation import GPSLocation
from .Pitch import Pitch
from .RollAngle import RollAngle
from .Vibration import Vibration
from interfaces.subsystem import ISubsystem
from interfaces.actuators import ACommand, IActuator
from interfaces.sensors import AReading, ISensor

class GeoLocation(ISubsystem):
    """The Geo-Location subsystem of the container farm.
    """    
    def __init__(self) -> None:
        """Initalizes the sensors and actuators in the Geo-Location subsystem.
        """
        self._sensors: list[ISensor] = self._initialize_sensors()
        self._actuators: list[IActuator] = self._initialize_actuators()

    def _initialize_sensors(self) -> list[ISensor]:
        """Initializes all sensors in the Geo-Location subsystem and returns them as a list. Intended to be used in class constructor.

        :return List[ISensor]: List of initialized sensors.
        """

        return [
            GPSLocation("GPS (Air530)", AReading.Type.GPSLOCATION),
            Pitch("LIS3DHTR", AReading.Type.PITCH),
            RollAngle("LIS3DHTR", AReading.Type.ROLL_ANGLE),
            Vibration("LIS3DHTR", AReading.Type.VIBRATION)
        ]
        
    def _initialize_actuators(self) -> list[IActuator]:
        """Initializes all actuators in the Geo-Location subsystem and returns them as a list. Intended to be used in class constructor

        :return list[IActuator]: List of initialized actuators.
        """

        return [
            Buzzer(ACommand.Type.BUZZER, {'value': 'off'})
        ]

    def read_sensors(self) -> list[AReading]:     
        """Reads all the sensors inside the Geo-Location subsystem.
        """  

        readings: list[AReading] = []
        print("\n---------------------GEO-LOCATION SENSORS---------------------")

        for sensor in self._sensors:
            # Get the sensor reading
            reading_sensor_value = sensor.read_sensor()

            # GPS 
            if sensor.reading_type == AReading.Type.GPSLOCATION:
                print(f"Address: {reading_sensor_value}\n")
            # Pitch
            if sensor.reading_type == AReading.Type.PITCH:
                print(f"Pitch: {reading_sensor_value}\n")
            # Roll angle
            elif sensor.reading_type == AReading.Type.ROLL_ANGLE:
                print(f"Roll Angle: {reading_sensor_value}\n")
            # Vibration
            elif sensor.reading_type == AReading.Type.VIBRATION:
                print(f"Vibration: {reading_sensor_value}\n")

            # Add the reading to the list of readings
            readings.append(reading_sensor_value)

        return readings

    def control_actuators(self, command: ACommand) -> bool:
        """Controls the actuators inside the Geo-Location subsystem.

        Args:
            command (ACommand): The command to control the actuator.
        """
        
        for actuator in self._actuators:
            # If the actuator is an instance of the Buzzer
            if command.target_type == ACommand.Type.BUZZER and actuator.type == ACommand.Type.BUZZER:
                # Validate command
                if actuator.validate_command(command):
                    # Control actuator with command data
                    return actuator.control_actuator(command.data)
                
        return False


if __name__ == "__main__":
    geo_location_subsystem = GeoLocation()

    while True:
        # Readings
        print(f"Geo-Location readings\n")

        geo_location_subsystem.read_sensors()

        sleep(2)

        # Control actuators
        fake_fan_message_body = '{"value": "on"}'
        fake_fan_command = ACommand(
            ACommand.Type.BUZZER, fake_fan_message_body)

        geo_location_subsystem.control_actuators(fake_fan_command)

        sleep(2)

        fake_fan_message_body_off = '{"value": "off"}'
        fake_fan_command_off = ACommand(
            ACommand.Type.BUZZER, fake_fan_message_body_off)

        geo_location_subsystem.control_actuators(fake_fan_command_off)

        sleep(2)