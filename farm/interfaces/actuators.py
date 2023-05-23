from abc import ABC, abstractmethod
from enum import Enum
import json

"""
We are using the 'ACommand' and 'IActuator' classes from Mauricio Andres Buschinelli: https://github.com/maujac.
"""
class ACommand:
    """Class for actuator commands. Defines all possible command types via enums.
    """

    class Type(str, Enum):
        """Enum defining types of actuators that can be targets for a command
        """
        FAN = 'fan'
        LIGHT_PULSE = 'light-pulse'
        BUZZER = 'buzzer'
        LED = 'light'
        DOORLOCK = 'doorlock'


    def __init__(self, target: Type, raw_message_body: str) -> None:
        """Builds an actuator command from a cloud message
        :param Type target: Type actuator targeted by command.
        :param str raw_message_body : Body of the message as received from cloud gateway
        """
        self.target_type = target
        # Parse the C2D message body as a dictionary. The items and structure inside
        # the body are left as an implementation detail of each specific actuator.
        self.data: dict = json.loads(raw_message_body)

        print(f"{self.target_type}: {self.data}")

    def __repr__(self) -> str:
        return f'Command for {self.target_type} as {self.data}'


class IActuator(ABC):

    # Properties to be initialized in constructor of implementation classes
    _current_state: dict
    type: ACommand.Type

    @abstractmethod
    def __init__(self, gpio: int, type: ACommand.Type, initial_state: dict) -> None:
        """Constructor for Actuator class. Must define interface's class properties
        :param ACommand.Type type: Type of command the actuator can respond to.
        :param dict initial_state: initializes 'current_state' property of a new actuator.
        Should minimally include the key "value" with corresponding value of initial state
        """
        pass

    @abstractmethod
    def validate_command(self, command: ACommand) -> bool:
        """Validates that a command can be used with the specific actuator.
        :param ACommand command: the command to be validated.
        :return bool: True if command can be consumed by the actuator.
        """
        pass

    @abstractmethod
    def control_actuator(self, data: dict) -> bool:
        """Sets the actuator to the value passed as argument.
        :param dict value: dictionary containing keys and values with command information.
        :return bool: True if the state of the actuator changed, false otherwise.
        """
        pass