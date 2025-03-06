using GloboticketWeb.Models;
using Azure.Storage.Blobs;
using System.Text.Json;
using QRCoder;

namespace GloboticketWeb.Utils;

public class QrCodeBuilder
{
    private readonly BlobServiceClient blobServiceClient;
    private const string ContainerName = "tickets";
    
    public QrCodeBuilder(BlobServiceClient blobServiceClient)
    {
        this.blobServiceClient = blobServiceClient;
    }

    public async Task<Uri> BuildQrCode(Ticket ticket)
    {
        string ticketData = CreateTicketMetadata(ticket);

        // Generate QR code from the metadata
        byte[] qrCodeBytes = GenerateQrCode(ticketData);

        // Create or get container reference
        var containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

        // Check if container exists, if not create it
        await containerClient.CreateIfNotExistsAsync();

        // Create unique blob name
        string blobName = $"ticket_{ticket.Id}_{Guid.NewGuid()}.png";
        var blobClient = containerClient.GetBlobClient(blobName);

        // Upload QR code to blob storage
        await blobClient.UploadAsync(new BinaryData(qrCodeBytes), overwrite: true);

        await SetBlobMetadata(ticket, blobClient);

        // Generate SAS URI with 1 week access
        var sasUri = blobClient.GenerateSasUri(
            Azure.Storage.Sas.BlobSasPermissions.Read, 
            DateTimeOffset.UtcNow.AddDays(7));

        return sasUri;
    }

    private static string CreateTicketMetadata(Ticket ticket)
    {
        // Create metadata with ticket information
        var metadata = new
        {
            TicketId = ticket.Id,
            EventId = ticket.EventId,
            EventName = ticket.Event.Name,
            EventDate = ticket.Event.Date,
            Venue = ticket.Event.Venue,
            NumberOfSeats = ticket.NumberOfSeats
        };

        // Convert metadata to JSON string
        string ticketData = JsonSerializer.Serialize(metadata);
        return ticketData;
    }

    private static async Task SetBlobMetadata(Ticket ticket, BlobClient blobClient)
    {
        var blobMetadata = new Dictionary<string, string>
        {
            { "TicketId", ticket.Id.ToString() },
            { "EventId", ticket.EventId.ToString() },
            { "EventName", ticket.Event.Name }
        };
        await blobClient.SetMetadataAsync(blobMetadata);
    }

    private static byte[] GenerateQrCode(string data)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }
}