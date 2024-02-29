export interface PacRequest
{
     Id: string,
     UserId: string,
     RequestedUrl: string,
     PartitionKey: string,
     RowKey: string,
     Timestamp?: number
}
