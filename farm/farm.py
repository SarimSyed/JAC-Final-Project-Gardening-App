import os
import asyncio
from azure.iot.device.aio import IoTHubDeviceClient
from azure.iot.device import MethodResponse
from interfaces.sensors import AReading
from connection_manager import ConnectionManager

async def main():

    connection_manager = ConnectionManager()
    await connection_manager.connect()

    while True:
        # print(connection_manager.telemetry_interval)
        await asyncio.sleep(connection_manager.telemetry_interval)

    # # Define behavior for halting the application
    # def stdin_listener():
    #     while True:
    #         selection = input("Press Q to quit\n")
    #         if selection == "Q" or selection == "q":
    #             print("Quitting...")
    #             break

    # # Run the stdin listener in the event loop
    # loop = asyncio.get_running_loop()
    # user_finished = loop.run_in_executor(None, stdin_listener)

    # # Wait for user to indicate they are done listening for method calls
    # await user_finished

    # Finally, shut down the client
    # await connection_manager._client.shutdown()


if __name__ == "__main__":
    asyncio.run(main())

    # If using Python 3.6 use the following code instead of asyncio.run(main()):
    # loop = asyncio.get_event_loop()
    # loop.run_until_complete(main())
    # loop.close()
