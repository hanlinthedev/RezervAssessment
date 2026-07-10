import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Activity, AlertTriangle, Ban, CheckCircle2, Plug } from "lucide-react";
import type { ConnectionState, LocationConnection } from "../types/connection";

type Props = {
	connections: LocationConnection[];
};

const countByState = (
	connections: LocationConnection[],
	state: ConnectionState,
) => {
	return connections.filter(
		(connection) => connection.connectionState === state,
	).length;
};

const ConnectionSummaryCards = ({ connections }: Props) => {
	const total = connections.length;
	const active = countByState(connections, "Active");
	const stale = countByState(connections, "Stale");
	const expired = countByState(connections, "Expired");
	const disconnected = countByState(connections, "Disconnected");

	const items = [
		{
			label: "Total",
			value: total,
			icon: Plug,
			className: "text-slate-600",
		},
		{
			label: "Active",
			value: active,
			icon: CheckCircle2,
			className: "text-green-600",
		},
		{
			label: "Stale",
			value: stale,
			icon: AlertTriangle,
			className: "text-yellow-600",
		},
		{
			label: "Expired",
			value: expired,
			icon: Ban,
			className: "text-red-600",
		},
		{
			label: "Disconnected",
			value: disconnected,
			icon: Activity,
			className: "text-slate-600",
		},
	];

	return (
		<div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-5">
			{items.map((item) => {
				const Icon = item.icon;

				return (
					<Card key={item.label}>
						<CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
							<CardTitle className="text-sm font-medium text-muted-foreground">
								{item.label}
							</CardTitle>
							<Icon className={`h-4 w-4 ${item.className}`} />
						</CardHeader>

						<CardContent>
							<div className="text-2xl font-bold">{item.value}</div>
						</CardContent>
					</Card>
				);
			})}
		</div>
	);
};

export default ConnectionSummaryCards;
