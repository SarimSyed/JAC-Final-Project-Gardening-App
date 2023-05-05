import seeed_python_reterminal.core as rt
import seeed_python_reterminal.acceleration as rt_accel
from time import sleep
import math

from interfaces.sensors import ISensor, AReading
from interfaces.geoLocation.accelerometerCalculate import IAccelerometerCalculate


class Vibration(ISensor, IAccelerometerCalculate):
    """The vibration of the reTerminal accelerometer in the Geo-Location subsytem.

    Args:
        ISensor (ISensor): Implements the interface.
        IAccelerometerCalculate (IAccelerometerCalculate): Implements the interface.
    """

    VALUE_COUNT = 2

    def __init__(self, model: str, type: AReading.Type):
        """Constructor for Vibration  class. May be called from childclass.
        :param str model: specific model of sensor hardware. Ex. GPS (Air530)
        :param ReadingType type: Type of reading this sensor produces. Ex. 'VIBRATION'
        """

        # Initialize object variables
        self._acceleration_device = rt.get_acceleration_device()
        self._sensor_model = model or "LIS3DHTR"
        self.reading_type = type or AReading.Type.ROLL_ANGLE

    def read_sensor(self) -> AReading:
        """Takes a reading form the sensor
        :return list[AReading]: List of readinds measured by the sensor. Most sensors return a list with a single item.
        """

        # Get the vibration value
        vibration = self._calculate_value()

        print(f"Vibration: {vibration}")

        return AReading(AReading.Type.VIBRATION,
                        AReading.Unit.VIBRATION, {
                            'value': vibration
                        })
    
    def _calculate_value(self) -> float:
        """Calculates the vibration from the reTerminal accelerometer.

        Returns:
            float: The vibration from the reTerminal accelerometer.
        """

        # Initialize acceleration value arrays
        x_values = []
        y_values = []
        z_values = []

        # Loop through the acceleration readings
        for event in self._acceleration_device.read_loop():

            # Get the current accelerometer reading
            accelEvent = rt_accel.AccelerationEvent(event)

            if accelEvent.name != None:

                # Add the 'X' acceleration value
                if accelEvent.name == rt_accel.AccelerationName.X:
                    x_values.append(accelEvent.value)
                # Add the 'Y' acceleration value
                elif accelEvent.name == rt_accel.AccelerationName.Y:
                    y_values.append(accelEvent.value)
                # Add the 'Z' acceleration value
                elif accelEvent.name == rt_accel.AccelerationName.Z:
                    z_values.append(accelEvent.value)

                # Only calculate vibration if minimum of 2 X,Y,Z acceleration values to compare absolute acceleration
                if len(x_values) >= Vibration.VALUE_COUNT and len(y_values) >= Vibration.VALUE_COUNT and len(z_values) >= Vibration.VALUE_COUNT:
                    # Get 2 'X' values
                    ax2 = x_values.pop()
                    ax1 = x_values.pop()

                    # Get 2 'Y' values
                    ay2 = y_values.pop()
                    ay1 = y_values.pop()

                    # Get 2 'Z' values
                    az2 = z_values.pop()
                    az1 = z_values.pop()
                    
                    # Calculate the vibration value
                    vibration = math.sqrt(
                          math.pow((ax2 - ax1), 2)
                        + math.pow((ay2 - ay1), 2)
                        + math.pow((az2 - az1), 2)
                    )

                    return vibration
        return 0



if __name__ == "__main__":

    vibration_accelerometer = Vibration("LIS3DHTR", AReading.Type.VIBRATION)

    while True:
        reading = vibration_accelerometer.read_sensor()
        print(f"{reading}\n")
        sleep(1)
