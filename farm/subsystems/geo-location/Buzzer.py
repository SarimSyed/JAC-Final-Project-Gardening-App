import seeed_python_reterminal.core as rt
from time import sleep
from farm.interfaces.actuators import IActuator, ACommand

class Buzzer(IActuator):
    """A Buzzer represents an alarm in the Geo-Location subsytem.

    Args:
        IActuator (IActuator): Implements the interface.
    """

    def __init__(self, type: ACommand.Type, initial_state: dict) -> None:
        """Constructor for Buzzer class. Must define interface's class properties
        :param ACommand.Type type: Type of command the actuator can respond to.
        :param dict initial_state: initializes 'current_state' property of a new actuator.
        """

        # Initialize object variables
        self.type = type or ACommand.Type.BUZZER
        self._current_state = initial_state

    def validate_command(self, command: ACommand) -> bool:
        """Validates that a command can be used with the specific actuator.
        :param ACommand command: the command to be validated.
        :return bool: True if command can be consumed by the actuator.
        """

        return command.target_type == self.type and str(command.data.get("value")).lower() == "on" or str(command.data.get("value")).lower() == "off"

    def control_actuator(self, data: dict) -> bool:
        """Sets the actuator to the value passed as argument.
        :param dict value: dictionary containing keys and values with command information.
        :return bool: True if the state of the actuator changed, false otherwise.
        """

        # Get the previous actuator state
        # current_state = str(self._current_state.get("value"))

        # Get the value from the dictionnary
        data_value = data["value"]

        # Turn on buzzer
        if data_value == "on":
            rt.buzzer = True
        # Turn off buzzer
        elif data_value == "off":
            rt.buzzer = False

        return rt.buzzer 



if __name__ == "__main__":
    # while True:
    #     rt.buzzer = True
    #     print("BUZZER ON")

    #     sleep(2)

    #     rt.buzzer = False
    #     print("BUZZER OFF")

    #     sleep(2)

    buzzer = Buzzer(ACommand.Type.BUZZER, {'value': 'off'})

    while True:
        fake_fan_message_body = '{"value": "on"}'
        fake_fan_command = ACommand(
            ACommand.Type.BUZZER, fake_fan_message_body)

        # Turn on the buzzer
        if buzzer.validate_command(fake_fan_command):
            buzzer.control_actuator({'value': 'on'})

        sleep(2)

        fake_fan_message_body_off = '{"value": "off"}'
        fake_fan_command_off = ACommand(
            ACommand.Type.BUZZER, fake_fan_message_body_off)

        # Turn off the buzzer
        if buzzer.validate_command(fake_fan_command_off):
            buzzer.control_actuator({'value': 'off'})

        sleep(2)
    

