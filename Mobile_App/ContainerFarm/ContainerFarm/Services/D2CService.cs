using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;


namespace ContainerFarm.Services
{
    public static class D2CService
    {
        public static async Task Initialize()
        {
            // Get the IoT Hub information
            var storageConnectionString = App.Settings.StorageConnectionString;
            var blobContainerName = App.Settings.BlobContainerName;
            var eventHubsConnectionString = App.Settings.EventHubConnectionString;
            var eventHubName = App.Settings.EventHubName;
            var consumerGroup = App.Settings.ConsumerGroup;

            // Create the storage client
            var storageClient = new BlobContainerClient(
                storageConnectionString,
                blobContainerName);

            // Create the processor
            var processor = new EventProcessorClient(
                storageClient,
                consumerGroup,
                eventHubsConnectionString,
                eventHubName);

            var partitionEventCount = new ConcurrentDictionary<string, int>();

            // Processes the events
            async Task processEventHandler(ProcessEventArgs args)
            {
                try
                {
                    // Don't continue if cancelled
                    if (args.CancellationToken.IsCancellationRequested)
                        return;

                    string partition = args.Partition.PartitionId;
                    byte[] eventBody = args.Data.EventBody.ToArray();

                    string eventBodyString = System.Text.Encoding.Default.GetString(eventBody);

                    Console.WriteLine(eventBodyString);

                    int eventsSinceLastCheckpoint = partitionEventCount.AddOrUpdate(
                        key: partition,
                        addValue: 1,
                        updateValueFactory: (_, currentCount) => currentCount + 1);

                    if (eventsSinceLastCheckpoint >= 50)
                    {
                        await args.UpdateCheckpointAsync();
                        partitionEventCount[partition] = 0;
                    }
                }
                catch
                {
                    // It is very important that you always guard against
                    // exceptions in your handler code; the processor does
                    // not have enough understanding of your code to
                    // determine the correct action to take.  Any
                    // exceptions from your handlers go uncaught by
                    // the processor and will NOT be redirected to
                    // the error handler.
                }
            }

            // Processes the Errors
            Task processErrorHandler(ProcessErrorEventArgs args)
            {
                try
                {
                    Debug.WriteLine("Error in the EventProcessorClient");
                    Debug.WriteLine($"\tOperation: {args.Operation}");
                    Debug.WriteLine($"\tException: {args.Exception}");
                    Debug.WriteLine("");
                }
                catch
                {
                    // It is very important that you always guard against
                    // exceptions in your handler code; the processor does
                    // not have enough understanding of your code to
                    // determine the correct action to take.  Any
                    // exceptions from your handlers go uncaught by
                    // the processor and will NOT be handled in any
                    // way.
                }

                return Task.CompletedTask;
            }

            try
            {
                using var cancellationSource = new CancellationTokenSource();
                cancellationSource.CancelAfter(TimeSpan.FromSeconds(30));

                processor.ProcessEventAsync += processEventHandler;
                processor.ProcessErrorAsync += processErrorHandler;

                try
                {
                    await processor.StartProcessingAsync(cancellationSource.Token);
                    await Task.Delay(Timeout.Infinite, cancellationSource.Token);
                }
                catch (TaskCanceledException)
                {
                    // This is expected if the cancellation token is
                    // signaled.
                }
                finally
                {
                    // This may take up to the length of time defined
                    // as part of the configured TryTimeout of the processor;
                    // by default, this is 60 seconds.

                    await processor.StopProcessingAsync();
                }
            }
            catch
            {
                // The processor will automatically attempt to recover from any
                // failures, either transient or fatal, and continue processing.
                // Errors in the processor's operation will be surfaced through
                // its error handler.
                //
                // If this block is invoked, then something external to the
                // processor was the source of the exception.
            }
            finally
            {
                // It is encouraged that you unregister your handlers when you have
                // finished using the Event Processor to ensure proper cleanup.  This
                // is especially important when using lambda expressions or handlers
                // in any form that may contain closure scopes or hold other references.

                processor.ProcessEventAsync -= processEventHandler;
                processor.ProcessErrorAsync -= processErrorHandler;
            }
        }
    }
}
