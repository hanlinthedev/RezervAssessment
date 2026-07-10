import {
	createConnection,
	disconnectConnection,
	reconnectConnection,
} from "@/api/connectionsApi";
import { queryKeys } from "@/api/queryKeys";
import { getApiErrorMessage } from "@/lib/apiError";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";
import { toast } from "sonner";

export const useConnectionActions = () => {
	const queryClient = useQueryClient();
	const navigate = useNavigate();

	const reconnectMutation = useMutation({
		mutationFn: reconnectConnection,

		onSuccess: async (connection) => {
			toast.success("Connection reconnected", {
				description: `${connection.displayName} is now active.`,
			});

			await queryClient.invalidateQueries({
				queryKey: queryKeys.connections,
			});

			await queryClient.invalidateQueries({
				queryKey: queryKeys.connection(connection.locationId),
			});
		},

		onError: (error) => {
			toast.error("Failed to reconnect connection", {
				description: getApiErrorMessage(error),
			});
		},
	});

	const disconnectMutation = useMutation({
		mutationFn: disconnectConnection,

		onSuccess: async (_, locationId) => {
			toast.success("Connection disconnected", {
				description: "Message sending is now disabled for this location.",
			});

			await queryClient.invalidateQueries({
				queryKey: queryKeys.connections,
			});

			await queryClient.invalidateQueries({
				queryKey: queryKeys.connection(locationId),
			});
		},

		onError: (error) => {
			toast.error("Failed to disconnect connection", {
				description: getApiErrorMessage(error),
			});
		},
	});

	const createConnectionMutation = useMutation({
		mutationFn: createConnection,

		onSuccess: async (connection) => {
			console.log("Connection created:", connection);
			toast.success("Connection created", {
				description: `${connection.displayName} is now active.`,
			});

			await queryClient.invalidateQueries({
				queryKey: queryKeys.connections,
			});

			navigate(`/connections/${connection.locationId}`);
		},

		onError: (error) => {
			toast.error("Failed to create connection", {
				description: getApiErrorMessage(error),
			});
		},
	});

	return {
		reconnectMutation,
		disconnectMutation,
		createConnectionMutation,
	};
};
