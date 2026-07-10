import type { ApiErrorResponse } from "@/types/api";
import axios from "axios";

export const getApiErrorMessage = (
	error: unknown,
	fallbackMessage = "Something went wrong.",
) => {
	if (axios.isAxiosError<ApiErrorResponse>(error)) {
		const data = error.response?.data;

		if (!data) {
			return fallbackMessage;
		}

		if (data.message) {
			return data.message;
		}

		if (data.errors) {
			const validationMessages = Object.values(data.errors).flat();

			if (validationMessages.length > 0) {
				return validationMessages.join(" ");
			}
		}

		if (data.title) {
			return data.title;
		}

		return fallbackMessage;
	}

	if (error instanceof Error) {
		return error.message;
	}

	return fallbackMessage;
};
