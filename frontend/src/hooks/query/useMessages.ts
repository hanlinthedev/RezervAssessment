import { getMessagesByLocationId } from "@/api/messagesApi";
import { queryKeys } from "@/api/queryKeys";
import type { MessageStatus } from "@/types/message";
import { useSuspenseQuery } from "@tanstack/react-query";

export const useMessages = (locationId: string, status?: MessageStatus) => {
	return useSuspenseQuery({
		queryKey: queryKeys.messages(locationId, status),
		queryFn: () => getMessagesByLocationId(locationId, status),
		refetchInterval: 10_000,
	});
};
