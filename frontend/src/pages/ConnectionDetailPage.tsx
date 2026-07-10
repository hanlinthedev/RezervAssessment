import ConnectionStateBadge from "@/components/ConnectionStateBadge";
import DisconnectConnectionDialog from "@/components/DisconnectConnectionDialog";
import MessageStatusBadge from "@/components/MessageStatusBadge";
import SendMessageForm from "@/components/SendMessageForm";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { Button } from "@/components/ui/button";
import {
	Card,
	CardContent,
	CardDescription,
	CardFooter,
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
import { Separator } from "@/components/ui/separator";
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from "@/components/ui/table";
import { useConnectionActions } from "@/hooks/mutation/useConnectionActions";
import { useConnection } from "@/hooks/query/useConnection";
import { useMessages } from "@/hooks/query/useMessages";
import { formatDate, formatRelativeTime } from "@/lib/dateTime";
import type { MessageStatus } from "@/types/message";
import { AlertCircle, ArrowLeft, RefreshCw } from "lucide-react";
import { useState } from "react";
import { Link, useParams } from "react-router-dom";

const messageStatusOptions: Array<MessageStatus | "All"> = [
	"All",
	"Queued",
	"Sent",
	"Delivered",
	"Read",
	"Failed",
];

const ConnectionDetailPage = () => {
	const { locationId } = useParams<{ locationId: string }>();

	const [statusFilter, setStatusFilter] = useState<MessageStatus | "All">(
		"All",
	);

	if (!locationId) {
		throw new Error("Location ID is required.");
	}

	const { data: connection } = useConnection(locationId);

	const {
		data: messages,
		refetch: refetchMessages,
		isFetching: isFetchingMessages,
	} = useMessages(
		locationId,
		statusFilter === "All" ? undefined : statusFilter,
	);

	const { reconnectMutation, disconnectMutation } = useConnectionActions();

	const isActionLoading =
		reconnectMutation.isPending || disconnectMutation.isPending;

	const isMessageBlocked =
		connection.connectionState === "Expired" ||
		connection.connectionState === "Disconnected";

	return (
		<main className="min-h-screen bg-muted/40 px-6 py-8">
			<div className="mx-auto max-w-6xl space-y-6">
				<div>
					<Link to="/">
						<Button variant="ghost" className="-ml-3">
							<ArrowLeft data-icon="inline-start" className="mr-2 h-4 w-4" />
							Back to connections
						</Button>
					</Link>

					<h1 className="mt-3 text-3xl font-bold tracking-tight text-foreground">
						Connection Details
					</h1>

					<p className="mt-2 text-muted-foreground">
						View connection status, manage connection state, send messages, and
						inspect message history.
					</p>
				</div>

				{connection.connectionState === "Stale" && (
					<Alert className="border-yellow-200 bg-yellow-50 text-yellow-800">
						<AlertCircle className="h-4 w-4" />
						<AlertTitle>Stale connection</AlertTitle>
						<AlertDescription>
							Messages can still be sent, but the WhatsApp Business App should
							be opened soon.
						</AlertDescription>
					</Alert>
				)}

				{isMessageBlocked && (
					<Alert variant="destructive">
						<AlertCircle className="h-4 w-4" />
						<AlertTitle>Message sending blocked</AlertTitle>
						<AlertDescription>
							This connection cannot send messages while it is{" "}
							{connection.connectionState}.
						</AlertDescription>
					</Alert>
				)}

				<div className="grid gap-6 lg:grid-cols-3">
					<div className="lg:col-span-2">
						<Card className="h-full ">
							<CardHeader className="flex flex-row items-start justify-between gap-4 space-y-0">
								<div>
									<CardTitle>{connection.displayName}</CardTitle>
									<CardDescription>{connection.locationId}</CardDescription>
								</div>

								<ConnectionStateBadge state={connection.connectionState} />
							</CardHeader>

							<CardContent className="space-y-6">
								<div className="grid gap-6 md:grid-cols-2">
									<div>
										<p className="text-sm font-medium text-muted-foreground">
											Phone Number
										</p>
										<p className="mt-1 text-foreground">
											{connection.phoneNumber}
										</p>
									</div>

									<div>
										<p className="text-sm text-muted-foreground">
											Last Activity
										</p>

										<p className="font-medium">
											{formatRelativeTime(connection.lastActivityAt)}
										</p>

										<p className="text-sm text-muted-foreground">
											{formatDate(connection.lastActivityAt)}
										</p>
									</div>

									<div>
										<p className="text-sm font-medium text-muted-foreground">
											Connected At
										</p>
										<p className="mt-1 text-foreground">
											{formatDate(connection.connectedAt)}
										</p>
									</div>

									<div>
										<p className="text-sm font-medium text-muted-foreground">
											Updated At
										</p>
										<p className="mt-1 text-foreground">
											{formatDate(connection.updatedAt)}
										</p>
									</div>
								</div>

								<Separator />
							</CardContent>
							<Separator className="opacity-0" />
							<CardFooter className="flex flex-col items-end gap-3 sm:flex-row sm:justify-end ">
								<div className="flex justify-end gap-3">
									{connection.connectionState !== "Disconnected" && (
										<DisconnectConnectionDialog
											displayName={connection.displayName}
											isPending={disconnectMutation.isPending}
											onConfirm={() => disconnectMutation.mutate(locationId)}
										>
											<Button
												type="button"
												variant="destructive"
												disabled={isActionLoading}
											>
												{disconnectMutation.isPending
													? "Disconnecting..."
													: "Disconnect"}
											</Button>
										</DisconnectConnectionDialog>
									)}

									{connection.connectionState === "Disconnected" && (
										<Button
											type="button"
											onClick={() => reconnectMutation.mutate(locationId)}
											disabled={isActionLoading}
										>
											{reconnectMutation.isPending
												? "Reconnecting..."
												: "Reconnect"}
										</Button>
									)}
								</div>
							</CardFooter>
						</Card>
					</div>

					<div className="lg:col-span-1">
						<SendMessageForm
							locationId={connection.locationId}
							connectionState={connection.connectionState}
						/>
					</div>
				</div>

				<Card>
					<CardHeader className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
						<div>
							<CardTitle>Message Logs</CardTitle>
							<CardDescription>
								View message delivery history and lifecycle timestamps for this
								location.
							</CardDescription>
						</div>

						<div className="flex items-center gap-3">
							<Select
								value={statusFilter}
								onValueChange={(value) =>
									setStatusFilter(value as MessageStatus | "All")
								}
							>
								<SelectTrigger className="w-40">
									<SelectValue placeholder="Filter status" />
								</SelectTrigger>

								<SelectContent>
									{messageStatusOptions.map((status) => (
										<SelectItem key={status} value={status}>
											{status}
										</SelectItem>
									))}
								</SelectContent>
							</Select>

							<Button
								type="button"
								variant="outline"
								onClick={() => void refetchMessages()}
								disabled={isFetchingMessages}
							>
								<RefreshCw
									className={`mr-2 h-4 w-4 ${
										isFetchingMessages ? "animate-spin" : ""
									}`}
								/>
								Refresh
							</Button>
						</div>
					</CardHeader>

					<CardContent>
						<div className="overflow-x-auto rounded-md border">
							<Table>
								<TableHeader>
									<TableRow>
										<TableHead>Recipient</TableHead>
										<TableHead>Message</TableHead>
										<TableHead>Status</TableHead>
										<TableHead>Sent</TableHead>
										<TableHead>Delivered</TableHead>
										<TableHead>Read</TableHead>
										<TableHead>Failed</TableHead>
									</TableRow>
								</TableHeader>

								<TableBody>
									{messages.map((message) => (
										<TableRow key={message.id}>
											<TableCell className="whitespace-nowrap">
												{message.recipientPhone}
											</TableCell>

											<TableCell className="max-w-sm">
												<p className="line-clamp-2">{message.messageContent}</p>

												<p className="mt-1 text-xs text-muted-foreground">
													Created: {formatDate(message.createdAt)}
												</p>

												{message.failureReason && (
													<p className="mt-1 text-xs text-destructive">
														{message.failureReason}
													</p>
												)}
											</TableCell>

											<TableCell>
												<MessageStatusBadge status={message.status} />
											</TableCell>

											<TableCell className="whitespace-nowrap text-sm">
												{message.sentAt ? formatDate(message.sentAt) : "N/A"}
											</TableCell>

											<TableCell className="whitespace-nowrap text-sm">
												{message.deliveredAt
													? formatDate(message.deliveredAt)
													: "N/A"}
											</TableCell>

											<TableCell className="whitespace-nowrap text-sm">
												{message.readAt ? formatDate(message.readAt) : "N/A"}
											</TableCell>

											<TableCell className="whitespace-nowrap text-sm">
												{message.failedAt
													? formatDate(message.failedAt)
													: "N/A"}
											</TableCell>
										</TableRow>
									))}

									{messages.length === 0 && (
										<TableRow>
											<TableCell
												colSpan={7}
												className="h-24 text-center text-muted-foreground"
											>
												{statusFilter === "All"
													? "No message logs found."
													: `No ${statusFilter} message logs found.`}
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

export default ConnectionDetailPage;
