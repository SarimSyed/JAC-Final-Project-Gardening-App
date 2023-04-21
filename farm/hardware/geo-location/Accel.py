import seeed_python_reterminal.core as rt
import seeed_python_reterminal.acceleration as rt_accel
from time import sleep
import math
import evdev

# from smbus2 import SMBus, i2c_msg

# # Open i2c bus 1 and read one byte from address 80, offset 0
# with SMBus(1) as bus:
#     msg = i2c_msg.read(0x19, 80)
#     bus.i2c_rdwr(msg)

# import smbus
# import time
# import adafruit_lis3dh

# # Initialize I2C bus
# i2c = smbus.SMBus(1)

# Initialize LIS3DH sensor object
# lis3dh = adafruit_lis3dh.LIS3DH_I2C(i2c)

# # Set the range of the accelerometer (optional)
# # lis3dh.range = adafruit_lis3dh.RANGE_4_G

# # Main loop
# while True:
#     # Read the acceleration values
#     accel_x, accel_y, accel_z = lis3dh.acceleration

#     # Print the acceleration values
#     print("Acceleration (m/s^2): X={0:.2f}, Y={1:.2f}, Z={2:.2f}".format(accel_x, accel_y, accel_z))

#     # Wait for some time before reading again
#     time.sleep(0.1)


device = rt.get_acceleration_device() 


while True:
    for event in device.read_loop():
        accelEvent = rt_accel.AccelerationEvent(event)

        if accelEvent.name != None:
            print(f"name={str(accelEvent.name)} value={accelEvent.value}")

        # pitch = math.atan2(accelEvent.value)

        # sleep(0.5)

    

    g: float = 9.81
    roll = math.atan2(11,-1161)
    srqt = math.sqrt(math.pow(11, 2) + math.pow(-1161, 2))
    pitch = math.atan2(-(-42), srqt)
    vibration = math.sqrt(math.pow(-42, 2) + math.pow(11, 2) + math.pow(-1161, 2))

    print(f"roll: {roll}  |  pitch: {pitch}  |  vibration: {vibration}")

    # sleep(0.5)


