import { Toaster } from "@/components/ui/sonner";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { StrictMode, Suspense } from "react";
import { createRoot } from "react-dom/client";
import { ErrorBoundary } from "react-error-boundary";
import App from "./App";
import AppErrorFallback from "./components/AppErrorFallback";
import AppLoadingFallback from "./components/AppLoadingFallback";
import "./index.css";

const queryClient = new QueryClient({
	defaultOptions: {
		queries: {
			staleTime: 30_000,
			retry: 1,
			refetchOnWindowFocus: false,
			throwOnError: true,
		},
		mutations: {
			throwOnError: false,
		},
	},
});

createRoot(document.getElementById("root")!).render(
	<StrictMode>
		<QueryClientProvider client={queryClient}>
			<ErrorBoundary fallbackRender={AppErrorFallback}>
				<Suspense fallback={<AppLoadingFallback />}>
					<App />
				</Suspense>
			</ErrorBoundary>

			<Toaster richColors position="top-right" />
			<ReactQueryDevtools initialIsOpen={false} />
		</QueryClientProvider>
	</StrictMode>,
);
