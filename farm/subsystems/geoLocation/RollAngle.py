import seeed_python_reterminal.core as rt
import seeed_python_reterminal.acceleration as rt_accel
from time import sleep
import math

from interfaces.sensors import ISensor, AReading
from interfaces.geoLocation.accelerometerCalculate import IAccelerometerCalculate


class RollAngle(ISensor, IAccelerometerCalculate):
    """The pitch of the reTerminal accelerometer in the Geo-Location subsytem.

    Args:
        ISensor (ISensor): Implements the interface.
        IAccelerometerCalculate (IAccelerometerCalculate): Implements the interface.
    """

    VALUE_COUNT = 1

    def __init__(self, model: str, type: AReading.Type):
        """Constructor for Pitch  class. May be called from childclass.
        :param str model: specific model of sensor hardware. Ex. GPS (Air530)
        :param ReadingType type: Type of reading this sensor produces. Ex. 'PITCH'
        """

        # Initialize object variables
        self._acceleration_device = rt.get_acceleration_device()
        self._sensor_model = model or "LIS3DHTR"
        self.reading_type = type or AReading.Type.ROLL_ANGLE

    def read_sensor(self) -> AReading:
        """Takes a reading form the sensor
        :return list[AReading]: List of readinds measured by the sensor. Most sensors return a list with a single item.
        """
        
        # Get the roll angle value
        roll_angle = self._calculate_value()

        print(f"Roll Angle: {roll_angle}")
                
        return AReading(AReading.Type.ROLL_ANGLE,
                     AReading.Unit.ROLL_ANGLE, {
                        'value': roll_angle
                     })
    
    def _calculate_value(self) -> float:
        """Calculates the rolling angle from the reTerminal accelerometer.

        Returns:
            float: The rolling angle from the reTerminal accelerometer.
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

                # Only calculate vibration if minimum of 1 X,Y,Z acceleration values to compare absolute acceleration
                if len(x_values) >= RollAngle.VALUE_COUNT and len(y_values) >= RollAngle.VALUE_COUNT and len(z_values) >= RollAngle.VALUE_COUNT:
                    # Get 'X' value
                    ax = x_values.pop()
                    # Get 'Y' value
                    ay = y_values.pop()
                    # Get 'Z' value
                    az = z_values.pop()
                    
                    # Calculate the rolling angle value
                    roll_angle =  180 * math.atan(ax / math.sqrt(ay * ay + az * az)) / math.pi

                    return roll_angle
        return 0



if __name__ == "__main__":

    roll_angle_accelerometer = RollAngle("LIS3DHTR", AReading.Type.ROLL_ANGLE)

    while True:
        reading = roll_angle_accelerometer.read_sensor()
        print(f"{reading}\n")
        sleep(1)