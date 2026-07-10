import type { ApiResponse } from "../types/api";
import type {
	MessageLog,
	MessageStatus,
	SendMessageRequest,
} from "../types/message";
import { api } from "./api";

export async function sendMessage(
	payload: SendMessageRequest,
): Promise<MessageLog> {
	const response = await api.post<ApiResponse<MessageLog>>(
		"/api/messages/send",
		payload,
	);

	return response.data.data;
}

export async function getMessagesByLocationId(
	locationId: string,
	status?: MessageStatus,
): Promise<MessageLog[]> {
	const response = await api.get<ApiResponse<MessageLog[]>>(
		`/api/messages/${locationId}`,
		{
			params: status ? { status } : undefined,
		},
	);

	return response.data.data;
}
