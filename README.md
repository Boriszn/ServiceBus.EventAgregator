# ServiceBus EventAgregator

Logic container for collecting events and decouple Publisher/Sender and Subscriber logic and use it independently. Based on Azure Service Bus. 

The project inspired by [Martin Fowler Event Agregator Pattern](https://martinfowler.com/eaaDev/EventAggregator.html)

## Installation

1. Clone repository
2. Fill up valid service bus configuration option in `appsettings.json`
3. Build / Run console application
4. (optional) Run UnitTests

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request

## Hisotry
- Init project structure
- Added logic to work with
- Add unit/integration test

## Todo
- Add abstractions to work with Queues
- Add logger
- Migrate to .Net Core 2.0 Azure Service Bus library

## License

This project is licensed under the MIT License
