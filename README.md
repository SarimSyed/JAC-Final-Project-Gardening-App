# Grove Base Pi Hat Pins



|    Sensor     | Actuator  |     Grove Hat Pin      |  Subsystem   |
| :-----------: | :-------: | :--------------------: | :----------: |
| GPS (Air530)  |           |          UART          | Geo-Location |
|     Pitch     |           | Built-in accelerometer | Geo-Location |
|  Roll Angle   |           | Built-in accelerometer | Geo-Location |
|   Vibration   |           | Built-in accelerometer | Geo-Location |
|               |  Buzzer   | Built-in accelerometer | Geo-Location |
|   Humidity    |           |           26           |    Plants    |
|  Temperature  |           |           26           |    Plants    |
| Soil Moisture |           |           A2           |    Plants    |
| Liquid Level  |           |           A4           |    Plants    |
|               |    LED    |           18           |    Plants    |
|               |    Fan    |           22           |    Plants    |
|  Luminosity   |           | Built-in Raspberry Pi  |   Security   |
|    Motion     |           |          D16           |   Security   |
|     Noise     |           |           A0           |   Security   |
|     Door      |           |           D5           |   Security   |
|               | Door Lock |          PWM           |   Security   |
|               |  Buzzer   | Built-in accelerometer |   Security   |

# Controlling Actuators
## Fan

- Communication Strategy
    - Device Twins
    - Device twin was used for this actuator because this strategy allows the IoT Hub to know if the state of the actuator was changed after requesting it which is not available for D2C messages. Direct methods returns a response to the IoT Hub however,       it is better to use device twins since you can keep track of the current state of all the actuator with the reported twin properties which can be helpful in tracking the state of the actuators in the app.
- Code snippet
  - Set plantsFan to on
      ```
      az iot hub device-twin update -n {iothub_name} -d {device_id} --desired '{"plantsFan": "on"}'
      ```
   - Set plantsFan to off
      ```
      az iot hub device-twin update -n {iothub_name} -d {device_id} --desired '{"plantsFan": "off"}'
      ```

## Light

- Communication Strategy
    - Device Twins
    - Device twin was used for this actuator because this strategy allows the IoT Hub to know if the state of the actuator was changed after requesting it which is not available for D2C messages. Direct methods returns a response to the IoT Hub however, it is better to use device twins since you can keep track of the current state of all the actuator with the reported twin properties which can be helpful in tracking the state of the actuators in the app.
- Code snippet
  - Set plantsLED to on
      ```
      az iot hub device-twin update -n {iothub_name} -d {device_id} --desired '{"plantsLED": "lights-on"}'
      ```
   - Set plantsLED to off
      ```
      az iot hub device-twin update -n {iothub_name} -d {device_id} --desired '{"plantsLED": "lights-off"}'
      ```

## Buzzer (Geo-Location & Security)

- Communication Strategy

  - Device Twins
  - Device twin was used for this actuator because this strategy allows the IoT Hub to know if the state of the actuator was changed after requesting it which is not available for D2C messages. Direct methods returns a response to the IoT Hub however, it is better to use device twins since you can keep track of the current state of all the actuator with the reported twin properties which can be helpful in tracking the state of the actuators in the app.

- Code snippet
  - Set geolocationBuzzer to on

    ```
    az iot hub device-twin update -n {iothub_name} -d {device_id} --desired '{"geolocationBuzzer": "on"}'
    ```

  - Set geolocationBuzzer to off

    ```
    az iot hub device-twin update -n {iothub_name} -d {device_id} --desired '{"geolocationBuzzer": "off"}'
    ```

    

## Door Lock

- Communication Strategy
  - Device Twins
  - Device twin was used for this actuator because this strategy allows the IoT Hub to know if the state of the actuator was changed after requesting it which is not available for D2C messages. Direct methods returns a response to the IoT Hub however, it is better to use device twins since you can keep track of the current state of all the actuator with the reported twin properties which can be helpful in tracking the state of the actuators in the app.
  
- Code snippet

  - Set securityDoorLock to lock
    ```
    az iot hub device-twin update -n {iothub_name} -d {device_id} --desired '{"securityDoorLock": "lock"}'
    ```

  - Set securityDoorLock to unlock

    ```
    az iot hub device-twin update -n {iothub_name} -d {device_id} --desired '{"securityDoorLock": "unlock"}'
    ```

    

