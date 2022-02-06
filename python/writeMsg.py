import time
from sensorToQueueWrite import RabbitQueue
from BMPSensor import BMPSensor


queue_object = RabbitQueue("hello", "localhost")
sensor = BMPSensor()
measurement = sensor.read_preassure_and_temp()


body = f'27|lubie ziemniaczki|{measurement.pressure}|{measurement.temperature}' 
queue_object.basic_publish("hello", body)
print("Poszlo, " + body)


#def main():
    

#    if __name__ == '__main__':
#        try:
#            main()
#        except KeyboardInterrupt:
#            print('Interrupted')
#            try:
#                sys.exit(0)
#            except SystemExit:
#                os._exit(0)
            

