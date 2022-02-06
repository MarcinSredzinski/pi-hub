import pika

class RabbitQueue:
    def __init__(self, queue_name, host_address):
        self.queue_name = queue_name
        self.host_address = host_address
        self.initialize_queue()
        print("initialized constructor")


    def initialize_queue(self):
        self.connection = pika.BlockingConnection(
            pika.ConnectionParameters(host=self.host_address))
        self.channel = self.connection.channel()
        self.channel.queue_declare(queue=self.queue_name, durable=True)
        print("initialized queue")


    def basic_publish(self, my_routing_key, msg_body):
        print("called basic publish")
        self.channel.basic_publish(exchange='', routing_key= my_routing_key, body = msg_body)
        print("Sent: " + msg_body)

