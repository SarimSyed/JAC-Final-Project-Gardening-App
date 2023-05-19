from subsystems.plants.PlantController import PlantSystem
from subsystems.geoLocation.GeoLocation import GeoLocation
from subsystems.security.Security import Security
from interfaces.sensors import AReading
from interfaces.actuators import ACommand
from interfaces.subsystem import ISubsystem

class SubsystemController:
    """This class will manage all the subsystems created and allow them to be manipulated from one class instead of 3 instantiations
    """
    def __init__(self) -> None:
        """Instantiates a new instance of the SubsystemController class.
        """

        self.plants : PlantSystem = PlantSystem()
        self.security: Security = Security()
        self.geolocation : GeoLocation = GeoLocation()
    
    def read_sensors(self) -> list[AReading]:
        """Reads all subsystem sensors and returns list of all gathered data

        Returns:
            list[AReading]: List of all sensor readings
        """
        # return self.geolocation.read_sensors()
        return self.security.read_sensors() + self.plants.read_sensors() + self.geolocation.read_sensors()
    
    def control_actuator(self, subsystem : ISubsystem, command: ACommand) -> bool:
        """Controls the specified actuator in the subsystem.

        Args:
            subsystem (_type_): The subsystem with the actuator.
            command (ACommand): The command to control the specified actuator.

        Returns:
            bool: True if the state of the actuator changed; otherwise false.
        """

        return subsystem.control_actuators(command)