import os
import asyncio
from azure.iot.device.aio import IoTHubDeviceClient
from azure.iot.device import MethodResponse


async def main():
    # Gets the connection string for the device from the .env file
    conn_str = "HostName=IoTHub-Payal-Rathod.azure-devices.net;DeviceId=Farm;SharedAccessKey=EL63oRIXpCn3XbJjbK0VIgVqlgZurW/ODuLK9sQ02ag="

    # The client object is used to interact with your Azure IoT hub.
    device_client = IoTHubDeviceClient.create_from_connection_string(conn_str)

    # connect the client.
    await device_client.connect()

    #------------------------- DIRECT METHOD -------------------------

    # Define behavior for direct method handling methods
    async def method_request_handler(method_request):

        # Determine how to respond to the method request based on the method name
        if method_request.name == "is_online":
            payload = None
            status = 200  # set return status code
        else:
            payload = {"details": "method name unknown"}  # set response payload
            status = 400  # set return status code

        # Send the response
        method_response = MethodResponse.create_from_method_request(method_request, status, payload)
        await device_client.send_method_response(method_response)

    # Set the method request handler on the client
    device_client.on_method_request_received = method_request_handler

    #-----------------------------------------------------------------


    # Define behavior for halting the application
    def stdin_listener():
        while True:
            selection = input("Press Q to quit\n")
            if selection == "Q" or selection == "q":
                print("Quitting...")
                break

    # Run the stdin listener in the event loop
    loop = asyncio.get_running_loop()
    user_finished = loop.run_in_executor(None, stdin_listener)

    # Wait for user to indicate they are done listening for method calls
    await user_finished

    # Finally, shut down the client
    await device_client.shutdown()


if __name__ == "__main__":
    asyncio.run(main())

    # If using Python 3.6 use the following code instead of asyncio.run(main()):
    # loop = asyncio.get_event_loop()
    # loop.run_until_complete(main())
    # loop.close()
