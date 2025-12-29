export function durationFormatConverter(durationMinutes: number) {
    if (durationMinutes <= 0) return '0 Minutes';

    const hours = Math.floor(durationMinutes / 60);
    const minutes = durationMinutes % 60;

    const parts: string[] = [];

    if (hours > 0) {
        parts.push(`${hours} ${hours === 1 ? 'Hour' : 'Hours'}`);
    }

    if (minutes > 0) {
        parts.push(`${minutes} ${minutes === 1 ? 'Minute' : 'Minutes'}`);
    }

    return parts.join(' ');
}
