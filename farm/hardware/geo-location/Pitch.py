import seeed_python_reterminal.core as rt
import seeed_python_reterminal.acceleration as rt_accel
from time import sleep
import math

from sensors import ISensor, AReading, IAccelerometerCalculate


class Pitch(ISensor, IAccelerometerCalculate):
    """The pitch of the reTerminal accelerometer in the Geo-Location subsytem.

    Args:
        ISensor (ISensor): Implements the interface.
        IAccelerometerCalculate (IAccelerometerCalculate): Implements the interface.
    """

    def __init__(self, model: str, type: AReading.Type):
        """Constructor for Pitch  class. May be called from childclass.
        :param str model: specific model of sensor hardware. Ex. GPS (Air530)
        :param ReadingType type: Type of reading this sensor produces. Ex. 'PITCH'
        """

        # Initialize object variables
        self._acceleration_device = rt.get_acceleration_device()
        self._sensor_model = model or "LIS3DHTR"
        self.reading_type = type or AReading.Type.PITCH

    def read_sensor(self) -> AReading:
        """Takes a reading form the sensor
        :return list[AReading]: List of readinds measured by the sensor. Most sensors return a list with a single item.
        """
        
        # Get the pitch value
        pitch = self._calculate_value()

        print(f"Pitch: {pitch}")
                
        return AReading(AReading.Type.PITCH,
                     AReading.Unit.PITCH, {
                        'value': pitch
                     })
    
    def _calculate_value(self) -> float:
        """Calculates the pitch from the reTerminal accelerometer.

        Returns:
            float: The pitch from the reTerminal accelerometer.
        """

        x_values = []
        y_values = []
        z_values = []

        for event in self._acceleration_device.read_loop():
            accelEvent = rt_accel.AccelerationEvent(event)

            if accelEvent.name != None:
                # print(f"name={str(accelEvent.name)} value={accelEvent.value}")

                if accelEvent.name == rt_accel.AccelerationName.X:
                    x_values.append(accelEvent.value)
                elif accelEvent.name == rt_accel.AccelerationName.Y:
                    y_values.append(accelEvent.value)
                elif accelEvent.name == rt_accel.AccelerationName.Z:
                    z_values.append(accelEvent.value)

                if len(x_values) >= 1 and len(y_values) >= 1 and len(z_values) >= 1:
                    ax = x_values.pop()
                    ay = y_values.pop()
                    az = z_values.pop()

                    pitch = 180 * math.atan(ay / math.sqrt(ax * ax + az * az)) / math.pi

                    return pitch
        return 0



if __name__ == "__main__":

    pitch_accelerometer = Pitch("LIS3DHTR", AReading.Type.PITCH)

    while True:
        reading = pitch_accelerometer.read_sensor()
        print(f"{reading}\n")
        sleep(1)

# x_values = []
# y_values = []
# z_values = []

# while True:
    # async def accel_coroutine(device):
    #     async for event in device.async_read_loop():
    #         accelEvent = rt_accel.AccelerationEvent(event)

    #         if accelEvent.name != None:
    #             print(f"name={str(accelEvent.name)} value={accelEvent.value}")

    #             if accelEvent.name == rt_accel.AccelerationName.X:
    #                 x_values.append(accelEvent.value)
    #             elif accelEvent.name == rt_accel.AccelerationName.Y:
    #                 y_values.append(accelEvent.value)
    #             elif accelEvent.name == rt_accel.AccelerationName.Z:
    #                 z_values.append(accelEvent.value)

    #             if len(x_values) >= 1 and len(y_values) >= 1 and len(z_values) >= 1:
    #                 ax = x_values.pop()
    #                 ay = y_values.pop()
    #                 az = z_values.pop()

    #                 # pitch = math.atan2(-ax, math.sqrt(ay*ay + az*az))
    #                 # roll_angle = math.atan2(ay, az)

    #                 pitch = 180 * math.atan(ax / math.sqrt(ay * ay + az * az)) / math.pi
    #                 roll_angle =  180 * math.atan(ay / math.sqrt(ax * ax + az * az)) / math.pi

    #                 print(f"Pitch: {pitch}")
    #                 print(f"Roll angle: {roll_angle}")

    #                 x_values.clear()
    #                 y_values.clear()
    #                 z_values.clear()

    # accel_device = rt.get_acceleration_device()

    # asyncio.ensure_future(accel_coroutine(accel_device))

    # loop = asyncio.get_event_loop()
    # loop.run_forever()
