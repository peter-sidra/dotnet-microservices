using System;
using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.EventProcessing {
    public class EventProcessor : IEventProcessor {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper) {
            _scopeFactory = scopeFactory;
            _mapper = mapper;

        }

        public void ProcessEvent(string message) {
            var eventType = DetermineEvent(message);

            switch (eventType) {
                case EventType.PlatformPublished:
                    addPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage) {
            Console.WriteLine("--> Determining event...");

            var eventType = JsonSerializer
                .Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event) {
                case "Platform_Published":
                    Console.WriteLine("--> PlatformPublished event detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private void addPlatform(string platformPublishedMessage) {
            // Because this class (EventProcessor) has a singleton lifespan and
            // the repository has a scoped lifespan, we need to create a new
            // scope in order to be able to inject the repository into this class
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

            var platformPublishedDto = JsonSerializer
                .Deserialize<PlatformPublishedDto>(platformPublishedMessage);

            try {
                var plat = _mapper.Map<Platform>(platformPublishedDto);
                if (!repo.ExternalPlatformExists(plat.ExternalID)) {
                    repo.CreatePlatform(plat);
                    repo.SaveChanges();
                    Console.WriteLine("--> Platform added!");
                }
                else {
                    Console.WriteLine("--> Platform already exists!");
                }
            }
            catch (System.Exception ex) {
                Console.WriteLine("--> Could not add Platform to the database: "
                    + ex.Message);
            }
        }
    }

    enum EventType {
        PlatformPublished,
        Undetermined
    }
}