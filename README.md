# TemperatureMeter
#### Technical Exercise Pub-Sub 
## Joao Pedro Antunes 

---

### Solution

This repository contains a solution for Technical Exercise (Publish/Subscribe pattern) from E-conomic (Visma). 
The solution is based on the principles of *Clean Architecture* and consists of the following projects:

* `TemperatureMeter.Api`: The .NET API responsible for receiving temperature readings (CreateTemperatureReadingCommandApi) to be published to the subscribers.
* `TemperatureMeter.Api.Contracts`: This project contains contracts/interfaces defining the API. It's intended to be shared for future SDK development, ensuring consistency in communication.
* `TemperatureMeter.Application`: A .NET Library that handles and processes the logic for TemperatureMeter, adhering to the principles of Clean Architecture.
* `TemperatureMeter.Application.Tests`: Unit tests for the logic defined in TemperatureMeter.Application Library.
* `TemperatureMeter.Domain`: A .NET Library that holds the domain logic of TemperatureMeter, designed following the principles of Clean Architecture.
* `TemperatureMeter.Domain.Tests`: Unit tests for the logic defined in TemperatureMeter.Domain.
* `Kernel`: : A Shared Kernel that contains generic basic common logic, such as IoC registration logic.
* `Messaging`: Library resposible for Messaging logic, in this case Pub-Sub logic.
* `Messaging.Tests`: The respective Unit Tests of Messaging Library.
* `TemperatureMeterSubscribers.ConsoleApp`: : A Console Application that subscribes to *TemperatureMeteringCreated* events and displays them on the console window.
* `TemperatureMonitor.ConsoleApp`: A Console Application that subscribes to *TemperatureMeteringCreated* events and displays them on the console window only if the temperatures are too low.

### How the Flow Works:
Data Ingestion: Temperature readings are sent to TemperatureMeter.Api by posting CreateTemperatureReadingCommandApi

API Processing: TemperatureMeter.Api processes the incoming temperature readings, likely validating and formatting them before passing them on.

Publishing: Once validated, the temperature readings are published using the messaging library (Messaging). This library handles the communication with the message broker (Mosquitto) and publishes the readings as events.

Subscription: Applications interested in temperature readings subscribe to these events. TemperatureMeterSubscribers.ConsoleApp and TemperatureMonitor.ConsoleApp are two such subscribers in this solution.

Event Handling: When a temperature reading event is received, the subscribing applications handle it according to their logic. TemperatureMeterSubscribers.ConsoleApp simply displays all readings, while TemperatureMonitor.ConsoleApp filters readings and displays only those that are too low.

### Requirement to RUN 

**Mosquitto Installation:** The solution utilizes a lightweight Message Broker implementing the MQTT protocol called Mosquitto.
 Before running the example, ensure that Mosquitto is installed. 
 It can be downloaded from Mosquitto's official website: https://mosquitto.org/download/

### What I would do if I more time was invested
* **Error Handling:** Enhanced error handling mechanisms, ensuring robustness and resilience in the system.
* **Configuration Management:** Improved ways to manage and share configurations across projects for better maintainability and flexibility.
* **Interface-based Serialization:** Using interfaces for serialization to facilitate easier changes in the serialization mechanism in the future.
* **Test Coverage:** Extending unit tests and adding integration tests to cover a broader range of scenarios and ensure comprehensive testing.
* **Pub/Sub Topics:** Enhancing the topic structure for better organization and efficiency in message distribution.
* **Documentation:** Adding diagrams and comprehensive documentation to aid understanding and usage of the solution. This could include architectural diagrams, sequence diagrams, and API documentation.
