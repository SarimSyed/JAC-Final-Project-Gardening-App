import seeed_python_reterminal.core as rt
import seeed_python_reterminal.acceleration as rt_accel
from time import sleep
import math

from farm.interfaces.geoLocation.geoLocationSensors import ISensor, GeoLocationReading
from farm.interfaces.geoLocation.accelerometerCalculate import IAccelerometerCalculate


class Pitch(ISensor, IAccelerometerCalculate):
    """The pitch of the reTerminal accelerometer in the Geo-Location subsytem.

    Args:
        ISensor (ISensor): Implements the interface.
        IAccelerometerCalculate (IAccelerometerCalculate): Implements the interface.
    """

    VALUE_COUNT = 1

    def __init__(self, model: str, type: GeoLocationReading.Type):
        """Constructor for Pitch  class. May be called from childclass.
        :param str model: specific model of sensor hardware. Ex. GPS (Air530)
        :param ReadingType type: Type of reading this sensor produces. Ex. 'PITCH'
        """

        # Initialize object variables
        self._acceleration_device = rt.get_acceleration_device()
        self._sensor_model = model or "LIS3DHTR"
        self.reading_type = type or GeoLocationReading.Type.PITCH

    def read_sensor(self) -> GeoLocationReading:
        """Takes a reading form the sensor
        :return list[AReading]: List of readinds measured by the sensor. Most sensors return a list with a single item.
        """
        
        # Get the pitch value
        pitch = self._calculate_value()

        print(f"Pitch: {pitch}")
                
        return GeoLocationReading(GeoLocationReading.Type.PITCH,
                     GeoLocationReading.Unit.PITCH, {
                        'value': pitch
                     })
    
    def _calculate_value(self) -> float:
        """Calculates the pitch from the reTerminal accelerometer.

        Returns:
            float: The pitch from the reTerminal accelerometer.
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
                if len(x_values) >= Pitch.VALUE_COUNT and len(y_values) >= Pitch.VALUE_COUNT and len(z_values) >= Pitch.VALUE_COUNT:
                    # Get 'X' value
                    ax = x_values.pop()
                    # Get 'Y' value
                    ay = y_values.pop()
                    # Get 'Z' value
                    az = z_values.pop()

                    # Calculate the pitch value
                    pitch = 180 * math.atan(ay / math.sqrt(ax * ax + az * az)) / math.pi

                    return pitch
        return 0



if __name__ == "__main__":

    pitch_accelerometer = Pitch("LIS3DHTR", GeoLocationReading.Type.PITCH)

    while True:
        reading = pitch_accelerometer.read_sensor()
        print(f"{reading}\n")
        sleep(1)