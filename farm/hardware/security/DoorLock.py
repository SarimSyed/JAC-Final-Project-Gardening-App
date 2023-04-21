from gpiozero import Servo
from time import sleep
import seeed_python_reterminal.core as rt
from time import sleep
from actuators import IActuator, ACommand

class DoorLock(IActuator):
    """A Door lock is a sensor in the security subsytem.
    Args:
        IActuator (IActuator): Implements the interface.
    """
    LOCK = "lock"
    UNLOCK = "unlock"

    def __init__(self, gpio: int, initial_state: dict, type: ACommand.Type = ACommand.Type.DOORLOCK) -> None:
        """Constructor for Doorlock class. Must define interface's class properties
        :param ACommand.Type type: Type of command the actuator can respond to.
        :param dict initial_state: initializes 'current_state' property of a new actuator.
        """

        # Initialize object variables
        self.type = type
        self._current_state = initial_state

        #Inizialize actuator
        self._doorlock_servo = Servo(gpio)

    def validate_command(self, command: ACommand) -> bool:
        """Validates that a command can be used with the specific actuator.
        :param ACommand command: the command to be validated.
        :return bool: True if command can be consumed by the actuator.
        """

        return command.target_type == self.type and str(command.data.get("value")).lower() == DoorLock.UNLOCK or str(command.data.get("value")).lower() == DoorLock.LOCK

    def control_actuator(self, data: dict) -> bool:
        """Sets the actuator to the value passed as argument.
        :param dict value: dictionary containing keys and values with command information.
        :return bool: True if the state of the actuator changed, false otherwise.
        """


        # Get the value from the dictionnary
        data_value = data["value"]

        #Check if current state changes
        isChanged = self._current_state != data_value

        #Change current state
        self._current_state = data_value

        # Unlock door
        if data_value == DoorLock.UNLOCK:
            self._doorlock_servo.min()
        # lock door
        elif data_value == DoorLock.LOCK:
            self._doorlock_servo.max()

        return isChanged




if __name__ == "__main__":
    doorLock = DoorLock(12, {})

    while True:
        fake_unlock_message_body = '{"value": "unlock"}'
        fake_unlock_command = ACommand(
            ACommand.Type.DOORLOCK, fake_unlock_message_body)

        if doorLock.validate_command(fake_unlock_command):
            doorLock.control_actuator({'value': 'unlock'})

        sleep(5)

        fake_lock_message_body = '{"value": "lock"}'
        fake_lock_command = ACommand(
            ACommand.Type.DOORLOCK, fake_lock_message_body)

        if doorLock.validate_command(fake_lock_command):
            doorLock.control_actuator({'value': 'off'})

        sleep(5)

# servo = Servo(12)

# print(servo.value) #0
# print("Start in the middle")
# servo.mid()
# sleep(5)
# print("Go to min")
# servo.min() #close -1
# print(servo.value)
# sleep(5)
# print("Go to max")
# servo.max() #open 1
# print(servo.value)
# sleep(5)
# print("And back to middle")
# servo.mid()
# sleep(5)
# servo.value = None;