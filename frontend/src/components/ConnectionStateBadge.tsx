import { Badge } from "@/components/ui/badge";
import type { ConnectionState } from "../types/connection";

type Props = {
	state: ConnectionState;
};

const variantClasses: Record<ConnectionState, string> = {
	Active: "border-green-200 bg-green-50 text-green-700 hover:bg-green-50",
	Stale: "border-yellow-200 bg-yellow-50 text-yellow-800 hover:bg-yellow-50",
	Expired: "border-red-200 bg-red-50 text-red-700 hover:bg-red-50",
	Disconnected:
		"border-slate-200 bg-slate-100 text-slate-700 hover:bg-slate-100",
};

const ConnectionStateBadge = ({ state }: Props) => {
	return (
		<Badge variant="outline" className={variantClasses[state]}>
			{state}
		</Badge>
	);
};

export default ConnectionStateBadge;
