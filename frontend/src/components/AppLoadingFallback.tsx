import { Card, CardContent } from "./ui/card";
import { Skeleton } from "./ui/skeleton";

const AppLoadingFallback = () => {
	return (
		<main className="min-h-screen bg-muted/40 px-6 py-8">
			<div className="mx-auto max-w-6xl space-y-6">
				<div className="space-y-3">
					<Skeleton className="h-4 w-48" />
					<Skeleton className="h-9 w-80" />
					<Skeleton className="h-5 w-96" />
				</div>

				<Card>
					<CardContent className="space-y-4 p-6">
						<Skeleton className="h-8 w-full" />
						<Skeleton className="h-8 w-full" />
						<Skeleton className="h-8 w-full" />
						<Skeleton className="h-8 w-full" />
					</CardContent>
				</Card>
			</div>
		</main>
	);
};

export default AppLoadingFallback;
