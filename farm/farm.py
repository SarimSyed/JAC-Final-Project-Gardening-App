from subsystems.plants.Fan import Fan
from time import sleep
from interfaces.actuators import ACommand

on = '{"value" : "on"}'
off = '{"value" : "off"}'
err = '{"value" : "blink"}'
    
fan = Fan(5, ACommand.Type.FAN, initial_state={"value" : Fan.FAN_ON})

while True:
        fan.control_actuator({"value": Fan.FAN_OFF})
        sleep(2)
        fan.control_actuator({'value' : Fan.FAN_ON})
        sleep(2)

