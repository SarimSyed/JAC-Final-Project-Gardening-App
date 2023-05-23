from abc import ABC, abstractmethod
from .sensors import AReading
from .actuators import ACommand


class ISubsystem(ABC):
    """Interface for subsystem controllers
    """
    @abstractmethod
    def __init__(self) -> None:
        """Constructor for ISubsystem
        """

    @abstractmethod
    def read_sensors(self) -> list[AReading]:
        """returns a list of all readings from sensor

        Returns:
            list[AReading]: List of all sensor readings from subsystem.
        """
        pass

    @abstractmethod
    def control_actuators(self, command: ACommand) -> bool:
        """Control actuators with the specified command

        Args:
            command (ACommand): The command to be executed

        Returns:
            bool: Whether an actuators state was changed or not
        """
        pass