import { Button } from "@/components/ui/button";
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { useConnectionActions } from "@/hooks/mutation/useConnectionActions";
import { ArrowLeft } from "lucide-react";
import { useState } from "react";
import { Link } from "react-router-dom";
import { toast } from "sonner";

const CreateConnectionPage = () => {
	const [locationId, setLocationId] = useState("");
	const [phoneNumber, setPhoneNumber] = useState("+959");
	const [displayName, setDisplayName] = useState("");

	const { createConnectionMutation } = useConnectionActions();
	const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
		event.preventDefault();

		if (!locationId.trim() || !phoneNumber.trim() || !displayName.trim()) {
			toast.error("Missing required fields", {
				description:
					"Location ID, phone number, and display name are required.",
			});

			return;
		}

		createConnectionMutation.mutate({
			locationId,
			phoneNumber,
			displayName,
		});
	};

	return (
		<main className="min-h-screen bg-muted/40 px-6 py-8">
			<div className="mx-auto max-w-3xl space-y-6">
				<div>
					<Link to="/">
						<Button variant="ghost" className="-ml-3">
							<ArrowLeft data-icon="inline-start" className="mr-2 h-4 w-4" />
							Back to connections
						</Button>
					</Link>

					<h1 className="mt-3 text-3xl font-bold tracking-tight text-foreground">
						Create Connection
					</h1>

					<p className="mt-2 text-muted-foreground">
						Register a new WhatsApp connection for a physical business location.
					</p>
				</div>

				<Card>
					<CardHeader>
						<CardTitle>Connection Details</CardTitle>
						<CardDescription>
							Each location can have only one WhatsApp connection.
						</CardDescription>
					</CardHeader>

					<CardContent>
						<form onSubmit={handleSubmit} className="space-y-5">
							<div className="space-y-2">
								<label
									htmlFor="locationId"
									className="text-sm font-medium text-foreground"
								>
									Location ID
								</label>

								<Input
									id="locationId"
									value={locationId}
									onChange={(event) => setLocationId(event.target.value)}
									disabled={createConnectionMutation.isPending}
									placeholder="loc-yangon-001"
								/>

								<p className="text-xs text-muted-foreground">
									Example: loc-yangon-001
								</p>
							</div>

							<div className="space-y-2">
								<label
									htmlFor="phoneNumber"
									className="text-sm font-medium text-foreground"
								>
									WhatsApp Phone Number
								</label>

								<Input
									id="phoneNumber"
									value={phoneNumber}
									onChange={(event) => setPhoneNumber(event.target.value)}
									disabled={createConnectionMutation.isPending}
									placeholder="+959123456789"
								/>

								<p className="text-xs text-muted-foreground">
									Use E.164 format, for example +959123456789.
								</p>
							</div>

							<div className="space-y-2">
								<label
									htmlFor="displayName"
									className="text-sm font-medium text-foreground"
								>
									Display Name
								</label>

								<Input
									id="displayName"
									value={displayName}
									onChange={(event) => setDisplayName(event.target.value)}
									disabled={createConnectionMutation.isPending}
									placeholder="Yangon Fitness Studio"
								/>
							</div>

							<div className="flex justify-end gap-3 pt-2">
								<Button type="button" variant="outline">
									<Link to="/">Cancel</Link>
								</Button>

								<Button
									type="submit"
									disabled={
										createConnectionMutation.isPending ||
										!locationId.trim() ||
										!phoneNumber.trim() ||
										!displayName.trim()
									}
								>
									{createConnectionMutation.isPending
										? "Creating..."
										: "Create Connection"}
								</Button>
							</div>
						</form>
					</CardContent>
				</Card>
			</div>
		</main>
	);
};

export default CreateConnectionPage;
