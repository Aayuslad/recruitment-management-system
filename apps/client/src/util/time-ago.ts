export function timeAgo(isoString: string): string {
    const now = new Date();
    const past = new Date(isoString);

    if (isNaN(past.getTime())) {
        throw new Error('Invalid ISO date string');
    }

    const diffMs = now.getTime() - past.getTime();
    if (diffMs < 0) return 'just now';

    const DAY = 24 * 60 * 60 * 1000;
    const MONTH = 30 * DAY; // standard approximation
    const YEAR = 365 * DAY;

    const years = Math.floor(diffMs / YEAR);
    const months = Math.floor((diffMs % YEAR) / MONTH);
    const days = Math.floor((diffMs % MONTH) / DAY);

    if (years > 0) {
        return months > 0
            ? `${years} year${years > 1 ? 's' : ''}, ${months} month${months > 1 ? 's' : ''} ago`
            : `${years} year${years > 1 ? 's' : ''} ago`;
    }

    if (months > 0) {
        return days > 0
            ? `${months} month${months > 1 ? 's' : ''}, ${days} day${days > 1 ? 's' : ''} ago`
            : `${months} month${months > 1 ? 's' : ''} ago`;
    }

    if (days > 0) {
        return `${days} day${days > 1 ? 's' : ''} ago`;
    }

    return 'today';
}
