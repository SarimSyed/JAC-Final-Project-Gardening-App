import seeed_python_reterminal.core as rt
from time import sleep
from interfaces.actuators import IActuator, ACommand

class Buzzer(IActuator):
    """A Buzzer represents an alarm in the security subsytem.
    Args:
        IActuator (IActuator): Implements the interface.
    """
    ONSTATE = "on"
    OFFSTATE = "off"

    def __init__(self, initial_state: dict, type: ACommand.Type = ACommand.Type.BUZZER) -> None:
        """Constructor for Buzzer class. Must d, efine interface's class properties
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

        return command.target_type == self.type and str(command.data.get("value")).lower() == Buzzer.OFFSTATE or str(command.data.get("value")).lower() == Buzzer.ONSTATE

    def control_actuator(self, data: dict) -> bool:
        """Sets the actuator to the value passed as argument.
        :param dict value: dictionary containing keys and values with command information.
        :return bool: True if the state of the actuator changed, false otherwise.
        """

        #Check if value changed
        isChanged = self._current_state["value"] != data["value"]
        self._current_state = data

        # Get the value from the dictionnary
        data_value = data["value"]

        # Turn on fan
        if data_value == Buzzer.ONSTATE:
            rt.buzzer = True
        # Turn off fan
        elif data_value == Buzzer.OFFSTATE:
            rt.buzzer = False

        return isChanged




if __name__ == "__main__":
    

    buzzer = Buzzer({'value': 'off'})

    while True:
        fake_buzzer_message_body = '{"value": "on"}'
        fake_buzzer_command = ACommand(
            ACommand.Type.BUZZER, fake_buzzer_message_body)

        if buzzer.validate_command(fake_buzzer_command):
            buzzer.control_actuator({'value': 'on'})

        sleep(2)

        fake_buzzer_message_body_off = '{"value": "off"}'
        fake_buzzer_command_off = ACommand(
            ACommand.Type.BUZZER, fake_buzzer_message_body_off)

        if buzzer.validate_command(fake_buzzer_command_off):
            buzzer.control_actuator({'value': 'off'})

        sleep(2)