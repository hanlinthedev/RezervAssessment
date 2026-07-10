export type ConnectionState = "Active" | "Stale" | "Expired" | "Disconnected";

export type LocationConnection = {
	id: string;
	locationId: string;
	phoneNumber: string;
	displayName: string;
	connectionState: ConnectionState;
	lastActivityAt: string;
	connectedAt: string;
	updatedAt: string;
};

export type CreateConnectionRequest = {
	locationId: string;
	phoneNumber: string;
	displayName: string;
	lastActivityAt?: string;
};
