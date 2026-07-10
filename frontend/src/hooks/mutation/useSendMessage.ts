import { sendMessage } from "@/api/messagesApi";
import { queryKeys } from "@/api/queryKeys";
import { getApiErrorMessage } from "@/lib/apiError";
import type { SendMessageRequest } from "@/types/message";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";

export const useSendMessage = (locationId: string) => {
	const queryClient = useQueryClient();

	return useMutation({
		mutationFn: (payload: SendMessageRequest) => sendMessage(payload),

		onSuccess: async (message) => {
			if (message.status === "Failed") {
				toast.error("Message failed at Meta mock API", {
					description: message.failureReason ?? "Simulated Meta API failure.",
				});
			} else {
				toast.success("Message sent successfully", {
					description: `Current status: ${message.status}`,
				});
			}

			await queryClient.invalidateQueries({
				queryKey: queryKeys.messagesByLocation(locationId),
			});
		},

		onError: (error) => {
			toast.error("Failed to send message", {
				description: getApiErrorMessage(
					error,
					"Please check the connection state and try again.",
				),
			});
		},
	});
};
