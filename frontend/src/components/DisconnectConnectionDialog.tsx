import {
	AlertDialog,
	AlertDialogAction,
	AlertDialogCancel,
	AlertDialogContent,
	AlertDialogDescription,
	AlertDialogFooter,
	AlertDialogHeader,
	AlertDialogTitle,
	AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
import type { ReactNode } from "react";

type Props = {
	displayName: string;
	isPending: boolean;
	onConfirm: () => void;
	children: ReactNode;
};

const DisconnectConnectionDialog = ({
	displayName,
	isPending,
	onConfirm,
	children,
}: Props) => {
	return (
		<AlertDialog>
			<AlertDialogTrigger>{children}</AlertDialogTrigger>

			<AlertDialogContent>
				<AlertDialogHeader>
					<AlertDialogTitle>Disconnect WhatsApp connection?</AlertDialogTitle>
					<AlertDialogDescription>
						This will disconnect <strong>{displayName}</strong>. Messages cannot
						be sent from this location until the connection is reconnected.
					</AlertDialogDescription>
				</AlertDialogHeader>

				<AlertDialogFooter>
					<AlertDialogCancel disabled={isPending}>Cancel</AlertDialogCancel>

					<AlertDialogAction
						disabled={isPending}
						onClick={onConfirm}
						className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
					>
						{isPending ? "Disconnecting..." : "Disconnect"}
					</AlertDialogAction>
				</AlertDialogFooter>
			</AlertDialogContent>
		</AlertDialog>
	);
};

export default DisconnectConnectionDialog;
