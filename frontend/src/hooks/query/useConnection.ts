import { getConnectionByLocationId } from "@/api/connectionsApi";
import { queryKeys } from "@/api/queryKeys";
import { useSuspenseQuery } from "@tanstack/react-query";

export const useConnection = (locationId: string) => {
	return useSuspenseQuery({
		queryKey: queryKeys.connection(locationId),
		queryFn: () => getConnectionByLocationId(locationId),
	});
};
