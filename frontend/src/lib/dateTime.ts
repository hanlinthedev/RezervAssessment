export function formatDate(value: string) {
	return new Intl.DateTimeFormat("en-GB", {
		dateStyle: "medium",
		timeStyle: "short",
	}).format(new Date(value));
}

export const formatRelativeTime = (value: string | null | undefined) => {
	if (!value) {
		return "-";
	}

	const date = new Date(value);
	const now = new Date();

	const diffInMilliseconds = now.getTime() - date.getTime();
	const diffInMinutes = Math.floor(diffInMilliseconds / 1000 / 60);
	const diffInHours = Math.floor(diffInMinutes / 60);
	const diffInDays = Math.floor(diffInHours / 24);

	if (diffInMinutes < 1) {
		return "Just now";
	}

	if (diffInMinutes < 60) {
		return `${diffInMinutes} minute${diffInMinutes === 1 ? "" : "s"} ago`;
	}

	if (diffInHours < 24) {
		return `${diffInHours} hour${diffInHours === 1 ? "" : "s"} ago`;
	}

	return `${diffInDays} day${diffInDays === 1 ? "" : "s"} ago`;
};
