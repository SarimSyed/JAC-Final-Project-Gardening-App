from abc import abstractmethod

class IAccelerometerCalculate():
    """Interface to calculate accelerometer values.
    """
    @abstractmethod
    def _calculate_value(self) -> float:
        """Calculates the value (pitch, roll, vibration) from the accelerometer.
        :return float: The value (pitch, roll, vibration) from the accelerometer.
        """
        pass