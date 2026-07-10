import type { FallbackProps } from "react-error-boundary";
import { Button } from "./ui/button";
import { Card, CardContent } from "./ui/card";

const AppErrorFallback = ({ error, resetErrorBoundary }: FallbackProps) => {
	return (
		<main className="flex min-h-screen items-center justify-center bg-muted/40 px-6">
			<Card className="w-full max-w-md">
				<CardContent className="p-6 text-center">
					<h1 className="text-lg font-semibold text-foreground">
						Something went wrong
					</h1>

					<p className="mt-2 text-sm text-muted-foreground">
						{error instanceof Error
							? error.message
							: "Unexpected error occurred."}
					</p>

					<Button type="button" onClick={resetErrorBoundary} className="mt-4">
						Try again
					</Button>
				</CardContent>
			</Card>
		</main>
	);
};

export default AppErrorFallback;
