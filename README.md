# TemperatureMeter
#### Technical Exercise Pub-Sub 
## Joao Pedro Antunes 

---

### Solution

This repository contains a solution for the backend engineering technical exercise from M-KOPA. The solution consists of five projects:

* `TemperatureMeter.Api`: The .NET API that is responsible for receiving the *input of data* (CreateTemperatureReadingCommandApi) to be published to the subscribers.
* `TemperatureMeter.Api.Contracts`: A project that only contain the contracts of the API that can be shared for future SDK.
* `TemperatureMeter.Application`: A .NET Library that handles and processes the logic for TemperatureMeter.
* `TemperatureMeter.Application.Tests`: The respective Unit Tests of TemperatureMeter.Application Library.
* `TemperatureMeter.Domain`: A .NET Library that holds the Domain logic of TemperatureMeter.
* `TemperatureMeter.Domain.Tests`: The respective Unit Tests of TemperatureMeter.Domain Library.
* `Kernel`: A Shared Kernel that can be shared across all projects that contains generic basic common logic, for example IoC registration logic.
* `Messaging`: Library resposible for Messaging logic, in this case Pub-Sub logic.
* `Messaging.Tests`: The respective Unit Tests of Messaging Library.
* `TemperatureMeterSubscribers.ConsoleApp`: A Console Application that subscribes *TemperatureMeteringCreated* events and display thens on a Console Window.
* `TemperatureMonitor.ConsoleApp`: A Console Application that subscribes *TemperatureMeteringCreated* events and display thens on a Console Window only if the temperatures are too low.

TODO explain how the flow works.
TODO add screenshots with examples

### Requirement to RUN 

The pub-sub is using Message Broker that implements the MQTT protocol called Mosquitto. 

You can download it from here: https://mosquitto.org/download/

### What I would do if I more time was invested
* Improved Error Handling and how is returned response on API.
* Improved how things like configuration could be shared between projects.
* Use interfaces behind the Serialization, so it could be easier changed in the future.
* Extend Unit Tests and add other type of tests like integration tests.
* Improved Pub/Sub topics.
* Add Documentation Diagrams.
