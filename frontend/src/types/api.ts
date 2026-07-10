export type ApiResponse<T> = {
	success: boolean;
	message: string | null;
	data: T;
};

export type ApiErrorResponse = {
	success?: boolean;
	message?: string;
	title?: string;
	errors?: Record<string, string[]>;
};
