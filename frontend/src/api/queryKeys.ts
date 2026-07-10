export const queryKeys = {
	connections: ["connections"] as const,

	connection: (locationId: string) => ["connections", locationId] as const,

	messagesByLocation: (locationId: string) => ["messages", locationId] as const,

	messages: (locationId: string, status?: string) =>
		["messages", locationId, status ?? "all"] as const,
};
