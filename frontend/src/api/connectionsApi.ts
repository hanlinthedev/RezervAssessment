import type { ApiResponse } from "../types/api";
import type {
	CreateConnectionRequest,
	LocationConnection,
} from "../types/connection";
import { api } from "./api";

export async function getConnections(): Promise<LocationConnection[]> {
	const response =
		await api.get<ApiResponse<LocationConnection[]>>("/api/connections");

	return response.data.data;
}

export async function getConnectionByLocationId(
	locationId: string,
): Promise<LocationConnection> {
	const response = await api.get<ApiResponse<LocationConnection>>(
		`/api/connections/${locationId}`,
	);

	return response.data.data;
}

export async function createConnection(
	payload: CreateConnectionRequest,
): Promise<LocationConnection> {
	const response = await api.post<ApiResponse<LocationConnection>>(
		"/api/connections",
		payload,
	);
	console.log("API response:", response); // Log the entire response data for debugging
	return response.data.data;
}

export async function disconnectConnection(locationId: string): Promise<void> {
	await api.delete(`/api/connections/${locationId}`);
}

export async function reconnectConnection(
	locationId: string,
): Promise<LocationConnection> {
	const response = await api.post<ApiResponse<LocationConnection>>(
		`/api/connections/${locationId}/reconnect`,
	);

	return response.data.data;
}
