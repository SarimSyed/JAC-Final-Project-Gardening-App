from time import sleep
from Buzzer import Buzzer
from GPSLocation import GPSLocation
from Pitch import Pitch
from RollAngle import RollAngle
from Vibration import Vibration

from actuators import ACommand
from sensors import AReading

class GeoLocation:
    """The Geo-Location subsystem of the container farm.
    """    
    def __init__(self) -> None:
        """Initalizes the sensors and actuators in the Geo-Location subsystem.
        """
        self.gps_location = GPSLocation("GPS (Air530)", AReading.Type.GPSLOCATION)
        self.buzzer = Buzzer(ACommand.Type.BUZZER, {'value': 'off'})
        self.pitch_accelerometer = Pitch("LIS3DHTR", AReading.Type.PITCH)
        self.roll_angle_accelerometer = RollAngle("LIS3DHTR", AReading.Type.ROLL_ANGLE)
        self.vibration_accelerometer = Vibration("LIS3DHTR", AReading.Type.VIBRATION)

    def read_sensors(self) -> None:     
        """Reads all the sensors inside the Geo-Location subsystem.
        """  

        # GPS 
        gps_reading = self.gps_location.read_sensor()
        print(f"Address: {gps_reading}\n")

        # Pitch
        pitch_levels = self.pitch_accelerometer.read_sensor()
        print(f"Pitch: {pitch_levels}\n")

        # Roll angle
        roll_angle = self.roll_angle_accelerometer.read_sensor()
        print(f"Roll Angle: {roll_angle}\n")

        # Vibration
        vibration_levels = self.vibration_accelerometer.read_sensor()
        print(f"Vibration: {vibration_levels}\n")

    def control_actuators(self, command: ACommand) -> None:
        """Controls the actuators inside the Geo-Location subsystem.

        Args:
            command (ACommand): The command to control the actuator.
        """
        
        # Buzzer
        if self.buzzer.validate_command(command):
            self.buzzer.control_actuator(command.data)



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