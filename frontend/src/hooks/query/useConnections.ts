import { getConnections } from "@/api/connectionsApi";
import { queryKeys } from "@/api/queryKeys";
import { useSuspenseQuery } from "@tanstack/react-query";

export const useConnections = () => {
	return useSuspenseQuery({
		queryKey: queryKeys.connections,
		queryFn: getConnections,
	});
};
