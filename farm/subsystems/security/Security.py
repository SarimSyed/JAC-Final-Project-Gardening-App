from time import sleep

from interfaces.sensors import ISensor, AReading
from interfaces.actuators import IActuator, ACommand
from .Buzzer import Buzzer
from .Door import Door
from .DoorLock import DoorLock
from .Luminosity import Luminosity
from .Motion import Motion
from .Noise import Noise
from interfaces.subsystem import ISubsystem
from Constants.Pins import SecurityGPIOPins as Pins

class Security(ISubsystem):

    def __init__(self) -> None:
        self._sensors: list[ISensor] = self._initialize_sensors()
        self._actuators: list[IActuator] = self._initialize_actuators()

    def _initialize_sensors(self) -> list[ISensor]:
        """Initializes all sensors and returns them as a list. Intended to be used in class constructor.

        :return List[ISensor]: List of initialized sensors.
        """

        return [
            # Instantiate each sensor inside this list, separate items by comma.
            Door(Pins.DOOR_SENSOR),
            Luminosity(),
            Motion(Pins.MOTION_SENSOR),
            Noise()
        ]
        

    def _initialize_actuators(self) -> list[IActuator]:
        """Initializes all actuators and returns them as a list. Intended to be used in class constructor

        :return list[IActuator]: List of initialized actuators.
        """
        return [
            # Instantiate each actuator inside this list, separate items by comma.
            Buzzer({'value': 'off'}),
            DoorLock(Pins.DOOR_LOCK, {'value': 'lock'})
        ]

    def read_sensors(self) -> list[AReading]:
        """Reads data from all initialized sensors. 

        :return list[AReading]: a list containing all readings collected from sensors.
        """
        readings: list[AReading] = []
        print("\n-----------------------SECURITY SENSORS-----------------------")
        for x in range(len(self._sensors)):
            print(self._sensors[x].read_sensor())
            readings.append(self._sensors[x].read_sensor())    
        return readings

    def control_actuators(self,  command: ACommand) -> bool:
        """Controls actuators according to a list of commands. Each command is applied to it's respective actuator.

        :param list[ACommand] commands: List of commands to be dispatched to corresponding actuators.
        """
        if (command.target_type == ACommand.Type.BUZZER):
            for x in range(len(self._actuators)):
                if(self._actuators[x].type == ACommand.Type.BUZZER and self._actuators[x].validate_command(command)):
                    return self._actuators[x].control_actuator(command.data)

        if (command.target_type == ACommand.Type.DOORLOCK):
            for x in range(len(self._actuators)):
                if(self._actuators[x].type == ACommand.Type.DOORLOCK and self._actuators[x].validate_command(command)):
                    return self._actuators[x].control_actuator(command.data)
        return False


if __name__ == "__main__":
    """This script is intented to be used as a module, however, code below can be used for testing.
    """

    device_manager = Security()

    while True:
        #Read sensors
        device_manager.read_sensors()

        #Send command to perform action on the actuator

        print("BUZZER: ")
        fake_buzzer_message_body_on = '{"value": "on"}'
        fake_buzzer_command_on = ACommand(
            ACommand.Type.BUZZER, fake_buzzer_message_body_on)
        device_manager.control_actuators(fake_buzzer_command_on)

        sleep(1)
        
        print("BUZZER: ")
        fake_buzzer_message_body_off = '{"value": "off"}'
        fake_buzzer_command_off = ACommand(
            ACommand.Type.BUZZER, fake_buzzer_message_body_off)
        device_manager.control_actuators(fake_buzzer_command_off)

        sleep(1)
        
        print("DOOR LOCK: ")
        fake_unlock_message_body = '{"value": "unlock"}'
        fake_unlock_command = ACommand(
            ACommand.Type.DOORLOCK, fake_unlock_message_body)
        device_manager.control_actuators(fake_unlock_command)

        sleep(5)

        print("DOOR LOCK: ")
        fake_lock_message_body = '{"value": "lock"}'
        fake_lock_command = ACommand(
            ACommand.Type.DOORLOCK, fake_lock_message_body)
        device_manager.control_actuators(fake_lock_command)
        
        sleep(5)
