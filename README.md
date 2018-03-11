# ServiceBus EventAggregator

Logic container for collecting events and decouple Publisher/Sender and Subscriber logic and use it independently. Based on Azure Service Bus.

The project inspired by [Martin Fowler Event Aggregator Pattern](https://martinfowler.com/eaaDev/EventAggregator.html) and [easynetq.com](http://easynetq.com/)

## Architecture overview

![alt text](https://raw.githubusercontent.com/Boriszn/ServiceBus.EventAgregator/feature/SB-5-Update-Readme-Add-ReleaseNotes/assets/img/EventAggregator-Architecture.png "Logo Title Text 1")

* *Main entry point* - Bootstraps application configuration, Runs event aggregation, Sends tests messages
* *Event Aggregator* - [Key Component] Starts message aggregating process and Contains configuration for Event labels to Event handlers

```javascript
private Dictionary<string, Func<Message, bool>> SubscriptionToLabelFuncs => new Dictionary<string, Func<Message, bool>>
{
    { "First", DoFirstHandler },
    { "Second", DoSecondHandler }
};
```

**Event Aggregator example uses simple methods for demonstration purposes , in production ready code I recomend user Services clases which should be registered via IoC container and injected to EventAggrator.cs**

* *Service Bus Manager* - encapsulates logic which related to Azure Service Bus (Sending/Receiving messages)

## Installation

1. Clone repository
2. Fill up valid service bus configuration option in `appsettings.json`
`"connectionString": "Endpoint=sb://<INSTANCE-NAME>.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=<KEY>",`
3. Build / Run console application
4. (optional) Run UnitTests

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request

## History

All changes can be easily found in [RELEASENOTES](ReleaseNotes.md)

## Related options

* [MediatR](https://github.com/jbogard/MediatR)
* [Brighter](https://brightercommand.github.io/Brighter/)
* [MassTransit](http://masstransit-project.com/MassTransit/)

## License

This project is licensed under the MIT License