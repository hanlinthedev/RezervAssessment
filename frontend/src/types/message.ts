export type MessageStatus = "Queued" | "Sent" | "Delivered" | "Read" | "Failed";

export type MessageLog = {
	id: string;
	locationId: string;
	recipientPhone: string;
	messageContent: string;
	metaMessageId: string | null;
	status: MessageStatus;
	createdAt: string;
	sentAt: string | null;
	deliveredAt: string | null;
	readAt: string | null;
	failedAt: string | null;
	failureReason: string | null;
};

export type SendMessageRequest = {
	locationId: string;
	recipientPhone: string;
	messageContent: string;
};
