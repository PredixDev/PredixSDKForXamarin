#  Database API Examples

The DatabaseAPI examples consist of the following:

1. DatabaseDemo

	This demo shows basic PredixSDKForXamarin database tasks, including opening the database, reading and writing documents.
	
	*Note:* Fruit images will not appear since we don't have any reference to images locally.
	
2. ReplicationDemo

	This demo covers syncing data from a PredixSync backend, and then displaying document information from that synced data.
	
	Since we interact with a PredixSync backend and UAA, keep note of how the constants in `ReplicationDemo.Constants` are used (mainly in `MainPageViewModel.cs`). After a successful replication, you will see the images associated with each document retrieved from our backend.
	
3. IndexAndQueryDemo

	This demo covers creating an index, and then querying the index to return values.



