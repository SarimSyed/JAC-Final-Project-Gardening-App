from subsystems.plants.PlantController import PlantSystem
from subsystems.geoLocation.GeoLocation import GeoLocation
from subsystems.security.Security import Security
from interfaces.sensors import AReading
from interfaces.actuators import ACommand


class SubsystemController:
    """This class will manage all the subsystems created and allow them to be manipulated from one class instead of 3 instantiations
    """
    def __init__(self) -> None:

        self.plants : PlantSystem = PlantSystem()
        self.security: Security = Security()
        self.geolocation : GeoLocation = GeoLocation()
    
    def read_sensors(self) -> list[AReading]:
        """Reads all subsystem sensors and returns list of all gathered data

        Returns:
            list[AReading]: List of all sensor readings
        """
        return self.security.read_sensors()
        # return self.plants.read_sensors() + self.security.read_sensors() + self.geolocation.read_sensors()
    
    def control_actuator(self, subsystem, command: ACommand) -> bool:
        #property is the desired property that was retrieved and will be used to control actuator
        #With an if statement make the appropriate actuator work

        return subsystem.control_actuators(command)