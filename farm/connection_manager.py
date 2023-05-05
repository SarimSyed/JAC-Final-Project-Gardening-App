import asyncio
from typing import Callable

from interfaces.sensors import AReading
from interfaces.actuators import ACommand

from azure.iot.device.aio import IoTHubDeviceClient
from azure.iot.device import Message
from azure.iot.device import MethodResponse


from dotenv import load_dotenv
from os import environ
import json


class ConnectionConfig:
    """Represents all information required to successfully connect client to cloud gateway.
    """

    # Key names for configuration values inside .env file. See .env.example
    # Constants included as static class property
    DEVICE_CONN_STR = "IOTHUB_DEVICE_CONNECTION_STRING"

    def __init__(self, device_str: str) -> None:
        self._device_connection_str = device_str


class ConnectionManager:
    """Component of HVAC system responsible for communicating with cloud gateway.
    Includes registering command and reading endpoints and sending and receiving data.
    """

    def __init__(self) -> None:
        """Constructor for ConnectionManager and initializes an internal cloud gateway client.
        """
        self._connected = False
        self._config: ConnectionConfig = self._load_connection_config()
        self._client = IoTHubDeviceClient.create_from_connection_string(
            self._config._device_connection_str)

    def _load_connection_config(self) -> ConnectionConfig:
        """Loads connection credentials from .env file in the project's top-level directory.
        :return ConnectionConfig: object with configuration information loaded from .env file.
        """
        load_dotenv()
        connectionConfig = ConnectionConfig("HostName=IoTHub-Payal-Rathod.azure-devices.net;DeviceId=Farm;SharedAccessKey=EL63oRIXpCn3XbJjbK0VIgVqlgZurW/ODuLK9sQ02ag=")
        return connectionConfig
        

    async def connect(self) -> None:
        """Connects to cloud gateway using connection credentials and setups up a message handler
        """
        await self._client.connect()
        self._connected = True
        print("Connected")
        
        # Set the method request handler on the client
        self._client.on_method_request_received = self.method_request_handler


    def register_command_callback(self, command_callback: Callable[[ACommand], None]) -> None:
        """Registers an external callback function to handle newly received commands.
        :param Callable[[ACommand], None] command_callback: function to be called whenever a new command is received.
        """
        self._command_callback = command_callback
    
    # Define behavior for direct method handling methods
    async def method_request_handler(self, method_request):

        # Determine how to respond to the method request based on the method name
        if method_request.name == "is_online":
            payload = None
            status = 200  # set return status code
        else:
            payload = {"details": "method name unknown"}  # set response payload
            status = 400  # set return status code

        # Send the response
        method_response = MethodResponse.create_from_method_request(method_request, status, payload)
        await self._client.send_method_response(method_response)

    async def send_readings(self, readings: list[AReading]) -> None:
        """Send a list of sensor readings as messages to the cloud gateway.
        :param list[AReading] readings: List of readings to be sent.
        """
        msg_dict = {}

        for x in range(len(readings)):
            msg_dict.__setitem__('value', readings[x].value)

            msg = Message(json.dumps(msg_dict))
            msg.content_encoding = "utf-8"
            msg.content_type = "application/json"

            msg.custom_properties['reading-type'] = readings[x].reading_type
            await self._client.send_message(msg)