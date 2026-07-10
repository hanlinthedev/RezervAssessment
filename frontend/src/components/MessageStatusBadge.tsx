import { Badge } from "@/components/ui/badge";
import type { MessageStatus } from "../types/message";

type Props = {
	status: MessageStatus;
};

const variantClasses: Record<MessageStatus, string> = {
	Queued: "border-slate-200 bg-slate-50 text-slate-700 hover:bg-slate-50",
	Sent: "border-blue-200 bg-blue-50 text-blue-700 hover:bg-blue-50",
	Delivered:
		"border-purple-200 bg-purple-50 text-purple-700 hover:bg-purple-50",
	Read: "border-green-200 bg-green-50 text-green-700 hover:bg-green-50",
	Failed: "border-red-200 bg-red-50 text-red-700 hover:bg-red-50",
};

const MessageStatusBadge = ({ status }: Props) => {
	return (
		<Badge variant="outline" className={variantClasses[status]}>
			{status}
		</Badge>
	);
};

export default MessageStatusBadge;
