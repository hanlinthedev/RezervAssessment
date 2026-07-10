import ConnectionSummaryCards from "@/components/ComponentSummaryCard";
import ConnectionStateBadge from "@/components/ConnectionStateBadge";
import { Button } from "@/components/ui/button";
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from "@/components/ui/card";
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from "@/components/ui/table";
import { useConnectionActions } from "@/hooks/mutation/useConnectionActions";
import { useConnections } from "@/hooks/query/useConnections";
import { formatDate, formatRelativeTime } from "@/lib/dateTime";
import type { ConnectionState } from "@/types/connection";
import { Plus, RefreshCw } from "lucide-react";
import { useMemo, useState } from "react";
import { Link } from "react-router-dom";

type StateFilter = ConnectionState | "All";

const stateFilterOptions: StateFilter[] = [
	"All",
	"Active",
	"Stale",
	"Expired",
	"Disconnected",
];

const ConnectionsPage = () => {
	const [stateFilter, setStateFilter] = useState<StateFilter>("All");
	const [actionLocationId, setActionLocationId] = useState<string | null>(null);

	const { reconnectMutation, disconnectMutation } = useConnectionActions();

	const { data: connections, refetch, isFetching } = useConnections();

	const filteredConnections = useMemo(() => {
		if (stateFilter === "All") {
			return connections;
		}

		return connections.filter(
			(connection) => connection.connectionState === stateFilter,
		);
	}, [connections, stateFilter]);

	return (
		<main className="min-h-screen bg-muted/40 px-6 py-8">
			<div className="mx-auto max-w-6xl space-y-6">
				<div className="flex items-start justify-between gap-4">
					<div>
						<p className="text-sm font-medium text-muted-foreground">
							Rezerv WhatsApp Integration
						</p>

						<h1 className="mt-1 text-3xl font-bold tracking-tight text-foreground">
							Location Connections
						</h1>

						<p className="mt-2 text-muted-foreground">
							Monitor WhatsApp connection status for each business location.
						</p>
					</div>

					<div className="flex items-center gap-3">
						<Link to="/connections/new">
							<Button variant="outline">
								<Plus data-icon="inline-start" className="mr-2 h-4 w-4" />
								New Connection
							</Button>
						</Link>

						<Button
							type="button"
							onClick={() => void refetch()}
							disabled={isFetching}
						>
							<RefreshCw
								className={`mr-2 h-4 w-4 ${isFetching ? "animate-spin" : ""}`}
							/>
							{isFetching ? "Refreshing" : "Refresh"}
						</Button>
					</div>
				</div>

				<ConnectionSummaryCards connections={connections} />

				<Card>
					<CardHeader className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
						<div>
							<CardTitle>Connections</CardTitle>
							<CardDescription>
								Seeded and newly created WhatsApp location connections.
							</CardDescription>
						</div>

						<Select
							value={stateFilter}
							onValueChange={(value) => setStateFilter(value as StateFilter)}
						>
							<SelectTrigger className="w-44">
								<SelectValue placeholder="Filter by state" />
							</SelectTrigger>

							<SelectContent>
								{stateFilterOptions.map((state) => (
									<SelectItem key={state} value={state}>
										{state}
									</SelectItem>
								))}
							</SelectContent>
						</Select>
					</CardHeader>

					<CardContent>
						<div className="overflow-x-auto rounded-md border">
							<Table>
								<TableHeader>
									<TableRow>
										<TableHead>Location</TableHead>
										<TableHead>Phone</TableHead>
										<TableHead>State</TableHead>
										<TableHead>Last Activity</TableHead>
										<TableHead className="text-right">Action</TableHead>
									</TableRow>
								</TableHeader>

								<TableBody>
									{filteredConnections.map((connection) => (
										<TableRow key={connection.id}>
											<TableCell>
												<div className="font-medium">
													{connection.displayName}
												</div>

												<div className="text-sm text-muted-foreground">
													{connection.locationId}
												</div>
											</TableCell>

											<TableCell>{connection.phoneNumber}</TableCell>

											<TableCell>
												<ConnectionStateBadge
													state={connection.connectionState}
												/>
											</TableCell>

											<TableCell>
												<div className="font-medium">
													{formatRelativeTime(connection.lastActivityAt)}
												</div>

												<div className="text-sm text-muted-foreground">
													{formatDate(connection.lastActivityAt)}
												</div>
											</TableCell>

											<TableCell className="text-right">
												<div className="flex items-center justify-end gap-2">
													{connection.connectionState === "Disconnected" ? (
														<Button
															type="button"
															size="sm"
															variant="outline"
															disabled={
																actionLocationId === connection.locationId
															}
															onClick={() => {
																setActionLocationId(connection.locationId);
																reconnectMutation.mutate(
																	connection.locationId,
																	{
																		onSettled: () => {
																			setActionLocationId(null);
																		},
																	},
																);
															}}
														>
															{actionLocationId === connection.locationId
																? "Reconnecting..."
																: "Reconnect"}
														</Button>
													) : (
														<Button
															type="button"
															size="sm"
															variant="destructive"
															disabled={
																actionLocationId === connection.locationId
															}
															onClick={() => {
																setActionLocationId(connection.locationId);
																disconnectMutation.mutate(
																	connection.locationId,
																	{
																		onSettled: () => {
																			setActionLocationId(null);
																		},
																	},
																);
															}}
														>
															{actionLocationId === connection.locationId
																? "Disconnecting..."
																: "Disconnect"}
														</Button>
													)}

													<Button size="sm" variant="outline" className="px-2 ">
														<Link to={`/connections/${connection.locationId}`}>
															Details
														</Link>
													</Button>
												</div>
											</TableCell>
										</TableRow>
									))}

									{filteredConnections.length === 0 && (
										<TableRow>
											<TableCell
												colSpan={5}
												className="h-24 text-center text-muted-foreground"
											>
												{stateFilter === "All"
													? "No connections found."
													: `No ${stateFilter} connections found.`}
											</TableCell>
										</TableRow>
									)}
								</TableBody>
							</Table>
						</div>
					</CardContent>
				</Card>
			</div>
		</main>
	);
};

export default ConnectionsPage;
