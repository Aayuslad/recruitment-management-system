export function durationFormatConverter(duration: number) {
    const hours = Math.floor(duration / 60);
    const minutes = duration % 60;
    return `${hours} Hour ${minutes} Minutes`;
}
