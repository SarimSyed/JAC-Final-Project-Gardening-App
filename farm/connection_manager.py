import asyncio
from typing import Callable

from interfaces.sensors import AReading
from interfaces.actuators import ACommand

from azure.iot.device.aio import IoTHubDeviceClient
from azure.iot.device import Message
from azure.iot.device import MethodResponse
from dotenv import dotenv_values
import os
import json
# from farm.subsystems.subsytem_controller import SubsystemController as con

class Sensor:
    """Sensor class used only to store list of AReadings to aid with the json formatting in send_readings() method
    """
    def __init__(self, readings : list[AReading]) -> None:
        self.sensors : list[AReading] = readings

"""
We are using the 'ConnectionConfig' class from Mauricio Andres Buschinelli: https://github.com/maujac.
"""
class ConnectionConfig:
    """Represents all information required to successfully connect client to cloud gateway.
    """

    # Key names for configuration values inside .env file. See .env.example
    DEVICE_CONN_STR = "IOTHUB_DEVICE_CONNECTION_STRING"

    def __init__(self, device_str: str) -> None:
        self._device_connection_str = device_str

"""
We are using the 'ConnectionManager' class from Mauricio Andres Buschinelli: https://github.com/maujac.
"""
class ConnectionManager:
    """Component of HVAC system responsible for communicating with cloud gateway.
    Includes registering command and reading endpoints and sending and receiving data.
    """

    TELEMETRY_INTERVAL_PROPERTY = "telemetryInterval"
    IS_ONLINE_DIRECT_METHOD = "is_online"
    DESIRED_PROPERTY_NAME = "desired"
    PROPERTY_KEY_NAME = "value"
    SECURITY_DOORLOCK_PROPERTY = "securityDoorLock"
    SECURITY_DOORLOCK_VALUES = ["unlock", "lock"]

    def __init__(self, subsystems_controller) -> None:
        """Constructor for ConnectionManager and initializes an internal cloud gateway client.
        """
        self._connected = False
        self._config: ConnectionConfig = self._load_connection_config()
        self._client = IoTHubDeviceClient.create_from_connection_string(
            self._config._device_connection_str)
        self.telemetry_interval = 5
        self.subsystems_controller = subsystems_controller

    def _load_connection_config(self) -> ConnectionConfig:
        """Loads connection credentials from .env file in the project's top-level directory.
        :return ConnectionConfig: object with configuration information loaded from .env file.
        """
        
        # The path to the .env file
        env_path = 'farm/.env'
        
        # Raise exception if the .env file is not present in the farm folder
        if not os.path.exists(env_path):
            raise Exception(f"The file '${env_path}' is not present in the farm directory of the project.")

        # Load the .env values into a config object
        config_values = dotenv_values(dotenv_path=env_path)

        # Raise exception if the user name is not in the .env file
        if ConnectionConfig.DEVICE_CONN_STR not in config_values:
            raise Exception(f"The expected key name '${ConnectionConfig.DEVICE_CONN_STR}' was not found in the '${env_path}' file.")

        # Get the .env data
        connection_string = str(config_values.get(ConnectionConfig.DEVICE_CONN_STR))
        
        return ConnectionConfig(connection_string)
        
    async def connect(self) -> None:
        """Connects to cloud gateway using connection credentials and setups up a message handler
        """
        await self._client.connect()
        self._connected = True
        print("Connected")
        
        # Set the method request handler on the client
        self._client.on_method_request_received = self.method_request_handler

        # Interval
        await self.device_connected_twin_handler(self._client) 

        # Set the twin patch handler on the client
        self._client.on_twin_desired_properties_patch_received = self.twin_patch_handler

    def register_command_callback(self, command_callback: Callable[[ACommand], None]) -> None:
        """Registers an external callback function to handle newly received commands.
        :param Callable[[ACommand], None] command_callback: function to be called whenever a new command is received.
        """
        self._command_callback = command_callback
    
    # Define behavior for direct method handling methods
    async def method_request_handler(self, method_request):

        # Determine how to respond to the method request based on the method name
        if method_request.name == ConnectionManager.IS_ONLINE_DIRECT_METHOD:
            payload = None
            status = 200  
        else:
            payload = {"details": "method name unknown"}  
            status = 400 

        # Send the response
        method_response = MethodResponse.create_from_method_request(method_request, status, payload)
        await self._client.send_method_response(method_response)

    # Callback to receive twin data 
    async def twin_patch_handler(self, patch):
        """Handles the twin patch of the device received from the IoT Hub.

        Args:
            patch (Unknown): The twin patch of the device (desired properties).
        """

        # Can remove
        print("the data in the desired properties patch was: {}".format(patch))

        await self._telemetry_property_handler(patch)

    async def _telemetry_property_handler(self, desired_properties):
        """Handles setting the telemetry interval value from the twin desired properties.
           Sets the telemetry value to the new one if in desire properties.

        Args:
            desired_properties (Unknown): The twin desired properties.
        """

        # If the property is in the desired properties
        if ConnectionManager.TELEMETRY_INTERVAL_PROPERTY in desired_properties:
            # Get the new telemetry value
            telemetry_property_value = desired_properties[ConnectionManager.TELEMETRY_INTERVAL_PROPERTY]
            print(f"New telemetry interval: {telemetry_property_value}")
            self.telemetry_interval = telemetry_property_value

            # Report twin property of telemetry interval
            await self._report_telemetry_interval_twin_property(self.telemetry_interval)

        #GEO LOCATION BUZZER
        if "geolocationBuzzer" in desired_properties:
            # Get the new telemetry value
            buzzer_value = desired_properties["geolocationBuzzer"]
            print(f"New telemetry interval: {buzzer_value}")

            if buzzer_value == "on":
                raw_message_body = '{"value": "on"}'
            else:
                raw_message_body = '{"value": "off"}'
            
            buzzer_command = ACommand(ACommand.Type.BUZZER, raw_message_body)

            self.subsystems_controller.control_actuator(self.subsystems_controller.geolocation, buzzer_command)

        #SECURITY DOOR LOCK
        if ConnectionManager.SECURITY_DOORLOCK_PROPERTY in desired_properties:
            # Get the new telemetry value
            doorlock_value = desired_properties[ConnectionManager.SECURITY_DOORLOCK_PROPERTY]
            print(f"New telemetry interval: {doorlock_value}")

            if doorlock_value in ConnectionManager.SECURITY_DOORLOCK_VALUES:
                raw_message_body = self.create_raw_message_body(ConnectionManager.PROPERTY_KEY_NAME, doorlock_value)
                doorlock_command = ACommand(ACommand.Type.DOORLOCK, raw_message_body)
                self.subsystems_controller.control_actuator(self.subsystems_controller.security , doorlock_command)

    
    async def _report_telemetry_interval_twin_property(self, telemetry_property_value):
        """Updates the telemetryInterval report properties when desired property is updated in IoT Hub.

        Args:
            telemetry_property_value (float): The desired telemtryInterval property value.
        """

        # Report twin properties
        reported_properties = {
            ConnectionManager.TELEMETRY_INTERVAL_PROPERTY: telemetry_property_value}
        print("Setting reported temperature to {}".format(
            reported_properties[ConnectionManager.TELEMETRY_INTERVAL_PROPERTY]))
        await self._client.patch_twin_reported_properties(reported_properties)

    # Get Twin updates when device connected
    async def device_connected_twin_handler(self, device_client):
        """Handles the twin patch upon device being connected.

        Args:
            device_client (Unknown): The device client.
        """

        # Get the twin
        twin = await device_client.get_twin()

        # Can remove
        print("Twin document:")
        print("{}".format(twin))

        await self._telemetry_property_handler(twin[ConnectionManager.DESIRED_PROPERTY_NAME])

    async def send_readings(self, readings: list[AReading]) -> None:
        """Send a list of sensor readings as messages to the cloud gateway.
        :param list[AReading] readings: List of readings to be sent.
        """


        json_list : list = []    
        

        for x in range(len(readings)):
                json_list.append(readings[x].export_json())

        sensors = Sensor(json_list)
        #json.dump use learned from : https://www.geeksforgeeks.org/serialize-and-deserialize-complex-json-in-python/
        message = json.dumps(sensors, default=lambda o: o.__dict__, indent=2)
        msg = Message(message)
        msg.content_encoding = "utf-8"
        msg.content_type = "application/json"

            
        await self._client.send_message(msg)

    def create_raw_message_body(self, key, value):
        """Create a json message body to set the state of the actuators.

        Args:
            key (_type_): The key value of the json object
            value (_type_): The value of the key

        Returns:
            _type_: The json message that can change the state of the actuator
        """
        return '{"'+ key  +'": "'+ value +'"}'