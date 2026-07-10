import { Button } from "@/components/ui/button";
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { useSendMessage } from "@/hooks/mutation/useSendMessage";
import { useState } from "react";
import type { ConnectionState } from "../types/connection";

type Props = {
	locationId: string;
	connectionState: ConnectionState;
};

const SendMessageForm = ({ locationId, connectionState }: Props) => {
	const [recipientPhone, setRecipientPhone] = useState("+959999999999");
	const [messageContent, setMessageContent] = useState("");

	const isBlocked =
		connectionState === "Expired" || connectionState === "Disconnected";

	const sendMessageMutation = useSendMessage(locationId);

	const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
		event.preventDefault();

		if (isBlocked || !recipientPhone.trim() || !messageContent.trim()) {
			return;
		}

		sendMessageMutation.mutate({
			locationId,
			recipientPhone,
			messageContent,
		});

		setMessageContent("");
	};

	return (
		<Card className="h-full">
			<CardHeader>
				<CardTitle>Send Message</CardTitle>
				<CardDescription>
					Send a WhatsApp message using this location connection.
				</CardDescription>
			</CardHeader>

			<CardContent className="space-y-4">
				{/* {connectionState === "Stale" && (
					<Alert className="border-yellow-200 bg-yellow-50 text-yellow-800">
						<AlertCircle className="h-4 w-4" />
						<AlertTitle>Stale connection</AlertTitle>
						<AlertDescription>
							Messages can still be sent, but the WhatsApp Business App should
							be opened soon.
						</AlertDescription>
					</Alert>
				)}

				{isBlocked && (
					<Alert variant="destructive">
						<AlertCircle className="h-4 w-4" />
						<AlertTitle>Message sending blocked</AlertTitle>
						<AlertDescription>
							Messages cannot be sent while this connection is {connectionState}
							.
						</AlertDescription>
					</Alert>
				)} */}

				<form onSubmit={handleSubmit} className="space-y-4">
					<div className="space-y-2">
						<label
							htmlFor="recipientPhone"
							className="text-sm font-medium text-foreground"
						>
							Recipient Phone
						</label>

						<Input
							id="recipientPhone"
							type="text"
							value={recipientPhone}
							onChange={(event) => setRecipientPhone(event.target.value)}
							disabled={isBlocked || sendMessageMutation.isPending}
							placeholder="+959999999999"
						/>
					</div>

					<div className="space-y-2">
						<label
							htmlFor="messageContent"
							className="text-sm font-medium text-foreground"
						>
							Message Content
						</label>

						<Textarea
							id="messageContent"
							value={messageContent}
							onChange={(event) => setMessageContent(event.target.value)}
							disabled={isBlocked || sendMessageMutation.isPending}
							rows={4}
							placeholder="Hello! Your booking has been confirmed."
						/>
					</div>

					<div className="flex justify-end">
						<Button
							type="submit"
							disabled={
								isBlocked ||
								sendMessageMutation.isPending ||
								!recipientPhone.trim() ||
								!messageContent.trim()
							}
						>
							{sendMessageMutation.isPending ? "Sending..." : "Send Message"}
						</Button>
					</div>
				</form>
			</CardContent>
		</Card>
	);
};

export default SendMessageForm;
